using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DFC.Digital.Service.AzureSearch
{
    public class DfcSearchQueryBuilder : ISearchQueryBuilder
    {
        private const string AzureSearchSpecialChar = @"[*+^""~?<>/]|( -)|[!]|[()]|[{}]|[\[\]]|&&|&|\|\|";
        private IMapper mapper;

        public DfcSearchQueryBuilder()
        {
            var configure = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SearchProperties, SearchProperties>();
            });

            this.mapper = configure.CreateMapper();
        }

        public SearchProperties BuildExclusiveExactMatch(string searchTerm, SearchProperties properties)
        {
            var returnProperties = this.mapper.Map<SearchProperties>(properties ?? new SearchProperties());
            var cleanSearchTerm = EscapeSpecialCharactersInSearchTerm(searchTerm, returnProperties);

            //Escape single quotes
            cleanSearchTerm = cleanSearchTerm.Replace("'", "''");

            if (string.IsNullOrWhiteSpace(returnProperties.FilterBy))
            {
                returnProperties.FilterBy =
                    $"(FilterableTitle eq '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}' or FilterableAlternativeTitle eq '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}')";
            }

            return returnProperties;
        }

        public SearchProperties BuildExclusivePartialMatch(string searchTerm, SearchProperties properties, int exactMatchCount)
        {
            var returnProperties = this.mapper.Map<SearchProperties>(properties ?? new SearchProperties());
            returnProperties.Count -= exactMatchCount;
            returnProperties.ExactMatchCount = exactMatchCount;

            var cleanSearchTerm = EscapeSpecialCharactersInSearchTerm(searchTerm, returnProperties);

            //Escape single quotes
            cleanSearchTerm = cleanSearchTerm.Replace("'", "''");

            if (string.IsNullOrWhiteSpace(returnProperties.FilterBy))
            {
                returnProperties.FilterBy =
                    $"(FilterableTitle ne '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}' and FilterableAlternativeTitle ne '{cleanSearchTerm.TrimStart('\"').TrimEnd('\"')}')";
            }

            return returnProperties;
        }

        public string BuildContainPartialSearch(string cleanedSearchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return cleanedSearchTerm;
            }

            var newSearchTerm = string.Empty;
            var trimmedTerm = Regex.Replace(cleanedSearchTerm, @"\s+", " ").Trim();

            return
                trimmedTerm.Any(char.IsWhiteSpace)
                ? trimmedTerm
                    .Split(' ')
                    .Aggregate(
                        newSearchTerm,
                        (current, term) => current + (term.Contains("-") ? term.Trim() : CreateFuzzyAndContainTerm(term)))
                : trimmedTerm.Contains("-") ? trimmedTerm : CreateFuzzyAndContainTerm(trimmedTerm);
        }

        public string RemoveSpecialCharactersFromTheSearchTerm(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }
            else
            {
                var regex = new Regex(AzureSearchSpecialChar);
                var result = regex.Replace(searchTerm, string.Empty);
                return $"{Regex.Replace(result.Trim(), @"\s+", " ")}";
            }
        }

        public string EscapeSpecialCharactersInSearchTerm(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }
            else
            {
                var regex = new Regex(AzureSearchSpecialChar);
                return regex.Replace(searchTerm, (match) => $"\\{match.Value}");
            }
        }

        public string TrimCommonWordsAndSuffixes(string searchTerm, SearchProperties properties)
        {
            if (properties?.UseRawSearchTerm == true)
            {
                return searchTerm;
            }

            var result = searchTerm.Any(char.IsWhiteSpace)
                ? searchTerm
                   .Split(' ')
                   .Aggregate(string.Empty, (current, term) =>
                   {
                       if (IsCommonWord(term))
                       {
                           return $"{current}";
                       }
                       else
                       {
                           return $"{current} {TrimAndReplaceSuffixOnCurrentTerm(term)}";
                       }
                   })
                : TrimAndReplaceSuffixOnCurrentTerm(searchTerm);

            return result.Trim();
        }

        private static string TrimAndReplaceSuffixOnCurrentTerm(string term)
        {
            var trimmedWord = TrimSuffixFromSingleWord(term);
            var replaceSuffix = ReplaceSuffixFromSingleWord(trimmedWord);
            return replaceSuffix;
        }

        private static bool IsCommonWord(string term)
        {
            var commonWords = new[]
            {
                "and",
                "or",
                "the"
            };

            return commonWords.Any(w => w.Equals(term, StringComparison.OrdinalIgnoreCase));
        }

        private static string CreateFuzzyAndContainTerm(string trimmedTerm)
        {
            return $"/.*{trimmedTerm}.*/ {trimmedTerm}~";
        }

        private static string TrimSuffixFromSingleWord(string searchTerm)
        {
            var suffixes = new[]
            {
                "er",
                "ers",
                "est",
                "ing",
                "ment",
                "al",
                "ation",
                "or",
                "ology",
                "metry",
                "ics",
                "ette",
                "ance",
                "ist",
                "ies",
                "macy",
            };

            var suffixToBeTrimmed = suffixes.FirstOrDefault(s => searchTerm.EndsWith(s, StringComparison.OrdinalIgnoreCase));
            var trimmedResult = suffixToBeTrimmed is null ? searchTerm : searchTerm.Substring(0, searchTerm.LastIndexOf(suffixToBeTrimmed, StringComparison.OrdinalIgnoreCase));
            return trimmedResult.Length < 3 ? searchTerm : trimmedResult;
        }

        private static string ReplaceSuffixFromSingleWord(string trimmedWord)
        {
            var replaceSuffixDictionary = new Dictionary<string, string>
            {
                ["therapy"] = "thera",
                ["ology"] = "olo",
                ["techn"] = "technolo",
            };

            var suffixToBeTrimmed = replaceSuffixDictionary.FirstOrDefault(s => trimmedWord.EndsWith(s.Key, StringComparison.OrdinalIgnoreCase));
            var trimmedResult = suffixToBeTrimmed.Key is null
                ? trimmedWord
                : $"{trimmedWord.Substring(0, trimmedWord.LastIndexOf(suffixToBeTrimmed.Key, StringComparison.OrdinalIgnoreCase))}{suffixToBeTrimmed.Value}";
            return trimmedResult.Length < 3 ? trimmedWord : trimmedResult;
        }
    }
}