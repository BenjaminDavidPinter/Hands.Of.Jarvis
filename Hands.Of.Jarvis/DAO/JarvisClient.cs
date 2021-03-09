﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hands.Of.Jarvis.Objects;
using Microsoft.Data.Sqlite;

namespace Hands.Of.Jarvis.DAO
{
    public class BaseSQLLiteClient
    {

        private string SqliteDatabaseFile;

        public BaseSQLLiteClient(string SqliteDatabaseFile)
        {
            this.SqliteDatabaseFile = SqliteDatabaseFile;
        }

        public async Task ExecuteNonQuery(string Sql)
        {
            using (var connection = new SqliteConnection(SqliteDatabaseFile))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = Sql;
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> ExecuteScalar(string Sql)
        {
            
            using (var connection = new SqliteConnection(SqliteDatabaseFile))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = Sql;
                return command.ExecuteScalar();
            }

            return new object();
        }

        public async Task<List<T>> ExecuteRead<T>(string query) where T : StringIndexedObject, new()
        {
            List<T> results = new List<T>();

            using (var connection = new SqliteConnection(SqliteDatabaseFile))
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
                            Type fieldType = r.GetFieldType(i);
                            var mi = typeof(SqliteDataReader).GetMethod("GetFieldValue");
                            var fooRef = mi.MakeGenericMethod(fieldType);
                            result[r.GetName(i)] = fooRef.Invoke(r, new object[] { i });
                        }
                        results.Add(result);
                    }
                }
            }

            return results;
        }
    }
}
