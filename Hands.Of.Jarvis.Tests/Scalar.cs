using System;
using System.Threading.Tasks;
using Hands.Of.Jarvis.DAO;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture] 
    public class Scalar
    {
        JarvisClient cli;

        [SetUp]
        public void Setup()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);

            //Because this is highly dependant on checking string validity, we're going to go ahead and truncate the test table here
            cli.ExecuteNonQuery("delete from ScalarTester").GetAwaiter().GetResult();

            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into ScalarTester (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData', 123, '1/1/2021', 12.23)").GetAwaiter().GetResult();
        }

        [Test, Order(1)]
        public async Task SC_Returns_String()
        {
            var ScalarStringResult = await cli.ExecuteScalar<string>("select TextColumn from ScalarTester limit 1");
            Assert.IsTrue(ScalarStringResult == "testData");
        }

        [Test, Order(2)]
        public async Task SC_Returns_Long()
        {
            var ScalarIntegerResult = await cli.ExecuteScalar<long>("select IntegerColumn from ScalarTester limit 1");
            Assert.IsTrue(ScalarIntegerResult == 123);
        }

        /*
         Turns out, SQLite doesn't support DateTime objects, they're just strings stored in the file.
        So for now, DT conversion needs to happen on the client side
         */
        [Test, Order(3)]
        public async Task SC_Returns_Date()
        {
            var ScalarDateResult = await cli.ExecuteScalar<string>("select DateColumn from ScalarTester limit 1");
            Assert.IsTrue((DateTime.Parse("1/1/2021") - DateTime.Parse(ScalarDateResult)).Seconds == 0);
        }

        [Test, Order(4)]
        public async Task SC_Returns_Float()
        {
            var ScalarFloatResult = await cli.ExecuteScalar<double>("select FloatColumn from scalartester limit 1");
            Assert.IsTrue(ScalarFloatResult == 12.23);
        }
    }
}
