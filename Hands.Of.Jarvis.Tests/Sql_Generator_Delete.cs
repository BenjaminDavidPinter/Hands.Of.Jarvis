using System;
using System.Collections.Generic;
using Hands.Of.Jarvis.DAO;
using Hands.Of.Jarvis.Services.SqlGeneration;
using Hands.Of.Jarvis.Tests.TestModels;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture]
    public class Sql_Generator_Delete
    {
        JarvisClient cli;
        private readonly string correctStr = "DELETE FROM TestClass WHERE ScalarColumn = 1";

        public Sql_Generator_Delete()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);
        }

        [Test]
        public void Gen_Delete_For_TestClass()
        {
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };

            SqlDeleteGenerator delr = new SqlDeleteGenerator();
            var command = delr.Generate(T);

            Assert.AreEqual(command, correctStr);
        }

        [Test]
        public void Gen_Delete_Test_Keyless()
        {
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };

            SqlDeleteGenerator delr = new SqlDeleteGenerator();
            try
            {
                var command = delr.Generate(T, new Func<TestClass, IEnumerable<KeyValuePair<string, object>>>(x => {
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
        public void Gen_Delete_Custom_Keys()
        {
            string correctStatement = "DELETE FROM TestClass WHERE TextColumn = 'Blah' AND ScalarColumn = 1";
            TestClass T = new TestClass()
            {
                DateColumn = DateTime.Parse("1/1/2021"),
                FloatColumn = 12.12,
                IntegerColumn = 123,
                ScalarColumn = 1,
                TextColumn = "Tomato"
            };
            SqlDeleteGenerator delr = new SqlDeleteGenerator();
            var statement = delr.Generate(T, new Func<TestClass, IEnumerable<KeyValuePair<string, object>>>(x =>
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
