using System;
using System.Collections.Generic;

namespace DFC.Digital.Core
{
    public interface IODataQueryBuilder
    {
        Uri GetBaseUri();

        Uri GetQuery();

        void Select(IEnumerable<string> fields);

        void Expand(string field);

        void Expand(string field, IEnumerable<string> selectExpandedFields);

        string AddFilter(string field, IEnumerable<string> values, OdataFilterOperator binaryOperator);

        void OrderBy(string field);
    }
}