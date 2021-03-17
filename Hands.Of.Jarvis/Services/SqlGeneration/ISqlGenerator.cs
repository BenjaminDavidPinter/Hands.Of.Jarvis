using System;
using System.Collections.Generic;

namespace Hands.Of.Jarvis.Services.SqlGeneration
{
    public interface ISqlGenerator
    {
        string Generate<T>(T obj);
        string Generate<T>(T obj, Func<T, IEnumerable<KeyValuePair<string, object>>> byKey);
    }

    
}
