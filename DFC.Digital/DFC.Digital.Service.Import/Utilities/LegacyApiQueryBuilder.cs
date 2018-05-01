using DFC.Digital.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Import.Utilities
{
    public class LegacyApiQueryBuilder : IODataQueryBuilder
    {
        private const string BAUJOBPROFILEENDPOINT = "DFC.Digital.BauJobprofileEndPoint";
        private readonly IConfigurationProvider config;

        public LegacyApiQueryBuilder(IConfigurationProvider config)
        {
            this.config = config;
        }

        public string AddFilter(string field, IEnumerable<string> values, OdataFilterOperator binaryOperator)
        {
            throw new NotImplementedException();
        }

        public void Expand(string field)
        {
            throw new NotImplementedException();
        }

        public void Expand(string field, IEnumerable<string> selectExpandedFields)
        {
            throw new NotImplementedException();
        }

        public Uri GetBaseUri()
        {
            return config.GetConfig<Uri>(BAUJOBPROFILEENDPOINT);
        }

        public Uri GetUri()
        {
            throw new NotImplementedException();
        }

        public void OrderBy(string field)
        {
            throw new NotImplementedException();
        }

        public void Select(IEnumerable<string> fields)
        {
            throw new NotImplementedException();
        }
    }
}
