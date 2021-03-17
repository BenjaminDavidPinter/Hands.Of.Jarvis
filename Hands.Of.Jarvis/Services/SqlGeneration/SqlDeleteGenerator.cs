using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hands.Of.Jarvis.Services.SqlGeneration
{
    public class SqlDeleteGenerator: BaseSqlGenerator
    {
        public override string GenerateSql(IEnumerable<KeyValuePair<string, object>> Parameters, IEnumerable<KeyValuePair<string, object>> Keys, string TableName)
        {
            if(Keys.Count() == 0)
            {
                throw new InvalidDataException("Not enough keys were provided to perform an update.");
            }

            StringBuilder query = new StringBuilder();
            query.Append($"DELETE FROM {TableName} ");

            //TODO: May be a cleaner way to do this?
            //Do not need to add the final comma above
            var preparedQuery = String.Join("", query.ToString().Take(query.ToString().Length - 1));
            query.Clear();
            query.Append(preparedQuery);
            query.Append(" WHERE ");

            foreach (var item in Keys)
            {
                Type valueType = item.Value.GetType();
                string wrapQuote = ((valueType == typeof(String))
                    || (valueType == typeof(DateTime)) ? "'" : ""); //I'll support it, but you better not be using dates as keys

                query.Append($"{item.Key} = {wrapQuote}{item.Value}{wrapQuote} AND ");
            }


            //TODO: May be a cleaner way to do this?
            //Do not need to add the final comma above
            return String.Join("", query.ToString().Take(query.ToString().Length - 4)).Trim();
        }
    }
}
