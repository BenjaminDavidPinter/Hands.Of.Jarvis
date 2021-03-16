using System;
using System.Collections.Generic;

namespace Hands.Of.Jarvis.Services.SqlGeneration
{
    public interface ISqlGenerator
    {
        public string Generate<T>(T obj);
    }

    
}
