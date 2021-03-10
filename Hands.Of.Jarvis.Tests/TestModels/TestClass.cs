﻿using System;
using Hands.Of.Jarvis.Objects;

namespace Hands.Of.Jarvis.Tests.TestModels
{
    public class TestClass : StringIndexedObject
    {
        public string TextColumn { get; set; }
        public long IntegerColumn { get; set; }
        public DateTime DateColumn { get; set; }
        public double FloatColumn { get; set; }
    }
}
