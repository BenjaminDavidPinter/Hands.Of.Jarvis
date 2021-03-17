using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hands.Of.Jarvis.Attributes;

namespace Hands.Of.Jarvis.Services.SqlGeneration
{
    public abstract class BaseSqlGenerator: ISqlGenerator
    {
        public IEnumerable<KeyValuePair<string, object>> GetParamsForObject<T>(T Obj)
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<Key>() == null)
                .Select(x =>
                {
                    return new KeyValuePair<string, object>(x.Name, x.GetValue(Obj, null));
                });
        }

        public IEnumerable<KeyValuePair<string, object>> GetKeysForObject<T>(T Obj)
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.GetCustomAttribute<Key>() != null)
                .Select(x =>
                {
                    return new KeyValuePair<string, object>(x.Name, x.GetValue(Obj, null));
                });
        }

        public string GetTableNameForObject<T>(T Obj)
        {
            return typeof(T).Name;
        }

        public string Generate<T>(T obj)
        {
            return GenerateSql(GetParamsForObject(obj), GetKeysForObject(obj), GetTableNameForObject(obj));
        }

        public string Generate<T>(T obj, Func<T, IEnumerable<KeyValuePair<string, object>>> byKey)
        {
            return GenerateSql(GetParamsForObject(obj), byKey(obj), GetTableNameForObject(obj));
        }

        public abstract string GenerateSql(IEnumerable<KeyValuePair<string, object>> Parameters, IEnumerable<KeyValuePair<string, object>> Keys, string TableName);
    }
}
