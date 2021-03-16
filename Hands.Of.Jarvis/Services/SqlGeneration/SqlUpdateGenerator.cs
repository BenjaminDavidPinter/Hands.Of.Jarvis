using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hands.Of.Jarvis.Services.SqlGeneration
{
    public class SqlUpdateGenerator: BaseSqlGenerator, ISqlGenerator
    {
        public SqlUpdateGenerator()
        {
        }


        public string Generate<T>(T obj)
        {
            var TableName = GetTableNameForObject(obj);
            var Parameters = GetParamsForObject(obj);
            var Keys = GetKeysForObject(obj);

            StringBuilder query = new StringBuilder();
            query.Append($"UPDATE {TableName} ");
            query.Append("SET ");
            foreach (var item in Parameters)
            {
                Type valueType = item.Value.GetType();
                string wrapQuote = ((valueType == typeof(String))
                    || (valueType == typeof(DateTime)) ? "'" : "");

                query.Append($"{item.Key} = {wrapQuote}{item.Value}{wrapQuote},");
            }

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

                query.Append($"{item.Key} = {wrapQuote}{item.Value}{wrapQuote} AND");
            }


            //TODO: May be a cleaner way to do this?
            //Do not need to add the final comma above
            return String.Join("", query.ToString().Take(query.ToString().Length - 3)).Trim();
        }
    }
}
