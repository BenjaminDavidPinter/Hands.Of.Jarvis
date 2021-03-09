using System;
using System.Threading.Tasks;
using Hands.Of.Jarvis.DAO;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture]
    public class NonQuery
    {
        JarvisClient cli;

        [SetUp]
        public void SetUp()
        {
            cli = new JarvisClient("../../../TestDb/TestDb.db");
        }

        [Test]
        public async Task Scalar_Returns_Neg1_On_Single_Record_Select()
        {
            var result = await cli.ExecuteNonQuery("select * from scalartester limit 1");
            Assert.IsTrue(result == -1);
        }
    }
}
