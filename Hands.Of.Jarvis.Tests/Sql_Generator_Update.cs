using System;
using System.Collections.Generic;
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

        [Test]
        public void Gen_Update_Test_Keyless()
        {
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };
            try
            {
                var statement = gen.Generate(T, new Func<TestClass, IEnumerable<KeyValuePair<string, object>>>(x =>
                {
                    return new List<KeyValuePair<string, object>>();
                }));
            }
            catch
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Gen_Update_Custom_Keys()
        {
            string correctStatement = "UPDATE TestClass SET TextColumn = 'Tomato',IntegerColumn = 123,DateColumn = '01/01/2021 00:00:00',FloatColumn = 12.12 WHERE TextColumn = 'Blah' AND ScalarColumn = 1";
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };

            var statement = gen.Generate(T, new Func<TestClass, IEnumerable<KeyValuePair<string, object>>>(x =>
            {
                return new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("TextColumn", "Blah"),
                    new KeyValuePair<string, object>("ScalarColumn", 1)
                };
            }));

            Assert.AreEqual(correctStatement, statement);
        }
    }
}
