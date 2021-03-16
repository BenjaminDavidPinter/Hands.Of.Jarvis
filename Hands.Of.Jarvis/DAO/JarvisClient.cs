using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hands.Of.Jarvis.Attributes;
using Hands.Of.Jarvis.Objects;
using Hands.Of.Jarvis.Services.SqlGeneration;
using Microsoft.Data.Sqlite;

namespace Hands.Of.Jarvis.DAO
{
    public class JarvisClient
    {

        private string SqliteDatabaseFile;
        private SqlUpdateGenerator SqlUpdateGenerator;

        public JarvisClient(string SqliteDatabaseFile)
        {
            this.SqliteDatabaseFile = SqliteDatabaseFile;
            this.SqlUpdateGenerator = new SqlUpdateGenerator();
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
                if(typeof(T) == typeof(DateTime))
                {
                    var result = (string)command.ExecuteScalar();
                    return (T)(Object)DateTime.Parse(result);
                }
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
            return await this.ExecuteNonQuery(SqlUpdateGenerator.Generate(obj));
        }
    }
}
