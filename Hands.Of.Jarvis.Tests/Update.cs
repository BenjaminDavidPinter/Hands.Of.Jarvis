using System;
using System.Linq;
using System.Threading.Tasks;
using Hands.Of.Jarvis.DAO;
using Hands.Of.Jarvis.Tests.TestModels;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture]
    public class Update
    {
        JarvisClient cli;
        private long updatedClassIndex = 0;

        public Update()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);

            //Because this is highly dependant on checking string validity, we're going to go ahead and truncate the test table here
            cli.ExecuteNonQuery("delete from TestClass").GetAwaiter().GetResult();

            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into TestClass (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData', 123, '1/1/2021', 12.23)").GetAwaiter().GetResult();
            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into TestClass (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData2', 1232, '1/2/2021', 13.23)").GetAwaiter().GetResult();
        }

        [Test, Order(1)]
        public async Task SimpleObjectUpdate()
        {
            var testClass = await cli.Read<TestClass>("Select ScalarColumn, TextColumn, IntegerColumn, DateColumn, FloatColumn from TestClass Limit 1");

            var updatedObject = testClass.First();

            updatedClassIndex = updatedObject.ScalarColumn;

            updatedObject.FloatColumn = 9.99;

            var totalRecords = await cli.Update(updatedObject);

            Assert.AreEqual(totalRecords, 1);
        }

        [Test, Order(2)]
        public async Task CheckUpdateCorrectness()
        {
            var testClass = await cli.Read<TestClass>("Select ScalarColumn, TextColumn, IntegerColumn, DateColumn, FloatColumn from TestClass Limit 1");

            var updatedObject = testClass.Where(x => x.ScalarColumn == updatedClassIndex).First();

            Assert.AreEqual(updatedObject.FloatColumn, 9.99);
        }
    }
}
