using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
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

            return trimmedTerm.Any(char.IsWhiteSpace)
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

        public string BuildExactMatchSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return string.Empty;
            }

            if (searchTerm.Split(' ').Count() > 1)
            {
                return "\"" + searchTerm + "\" ";
            }

            return string.Empty;
        }

        private static string CreateFuzzyAndContainTerm(string trimmedTerm)
        {
            return $"/.*{trimmedTerm}.*/ {trimmedTerm}~";
        }
    }
}