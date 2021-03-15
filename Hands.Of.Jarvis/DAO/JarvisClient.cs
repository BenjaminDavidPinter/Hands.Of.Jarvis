using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hands.Of.Jarvis.Attributes;
using Hands.Of.Jarvis.Objects;
using Microsoft.Data.Sqlite;

namespace Hands.Of.Jarvis.DAO
{
    public class JarvisClient
    {

        private string SqliteDatabaseFile;

        public JarvisClient(string SqliteDatabaseFile)
        {
            this.SqliteDatabaseFile = SqliteDatabaseFile;
        }

        public async Task<int> ExecuteNonQuery(string Sql)
        {
            using (var connection = new SqliteConnection($@"Data Source = {SqliteDatabaseFile}"))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = Sql;
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<T> ExecuteScalar<T>(string Sql)
        {
            
            using (var connection = new SqliteConnection($@"Data Source = {SqliteDatabaseFile}"))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = Sql;
                return (T)command.ExecuteScalar();
                
            }
        }

        public async Task<List<T>> Read<T>(string query) where T : StringIndexedObject, new()
        {
            List<T> results = new List<T>();

            using (var connection = new SqliteConnection($@"Data Source = {SqliteDatabaseFile}"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = query;
                using (SqliteDataReader r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        T result = new T();
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            var getFieldValue = typeof(SqliteDataReader)
                                            .GetMethod("GetFieldValue")
                                            .MakeGenericMethod(r.GetFieldType(i));
                            result[r.GetName(i)] = getFieldValue.Invoke(r, new object[] { i });
                        }
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        public async Task<int> Update<T>(T obj)
        {
            string tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<Key>() == null)
                .Select(x =>
                {
                    return new KeyValuePair<string, object>(x.Name, x.GetValue(obj, null));
                });

            StringBuilder query = new StringBuilder();
            query.Append($"UPDATE {tableName} ");
            query.Append("SET ");
            foreach (var item in properties)
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

            var keys = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<Key>() != null)
                .Select(x =>
                {
                    return new KeyValuePair<string, object>(x.Name, x.GetValue(obj, null));
                });

            foreach (var item in keys)
            {
                Type valueType = item.Value.GetType();
                string wrapQuote = ((valueType == typeof(String))
                    || (valueType == typeof(DateTime)) ? "'" : ""); //I'll support it, but you better not be using dates as keys

                query.Append($"{item.Key} = {wrapQuote}{item.Value}{wrapQuote} AND");
            }


            //TODO: May be a cleaner way to do this?
            //Do not need to add the final comma above
            preparedQuery = String.Join("", query.ToString().Take(query.ToString().Length - 3)).Trim();

            return await this.ExecuteNonQuery(preparedQuery);
        }
    }
}
