using System;
using Hands.Of.Jarvis.Services.SqlGeneration;
using Hands.Of.Jarvis.Tests.TestModels;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture]
    public class Sql_Generator_Update
    {
        public SqlUpdateGenerator gen = new SqlUpdateGenerator();
        public string CorrectStatement = "UPDATE TestClass SET TextColumn = 'Tomato',IntegerColumn = 123,DateColumn = '01/01/2021 00:00:00',FloatColumn = 12.12 WHERE ScalarColumn = 1";

        [Test]
        public void Gen_Update_For_TestClass()
        {
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };

            var statement = gen.Generate(T);

            Assert.AreEqual(CorrectStatement, statement);
        }
    }
}
