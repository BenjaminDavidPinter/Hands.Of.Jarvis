using System;
using System.Linq;
using System.Threading.Tasks;
using Hands.Of.Jarvis.DAO;
using Hands.Of.Jarvis.Tests.TestModels;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    public class Read
    {
        JarvisClient cli;

        public Read()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);

            //Because this is highly dependant on checking string validity, we're going to go ahead and truncate the test table here
            cli.ExecuteNonQuery("delete from ScalarTester").GetAwaiter().GetResult();

            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into ScalarTester (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData', 123, '1/1/2021', 12.23)").GetAwaiter().GetResult();
            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into ScalarTester (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData2', 1232, '1/2/2021', 13.23)").GetAwaiter().GetResult();
        }

        [Test, Order(1)]
        public async Task RD_SingleRecord()
        {
            var basicReadResult = await cli.Read<TestClass>("SELECT TextColumn, IntegerColumn, DateColumn, FloatColumn from ScalarTester limit 1");
            Assert.IsTrue(
                basicReadResult.Count() == 1
                && basicReadResult.First().TextColumn == "testData"
                && basicReadResult.First().IntegerColumn == 123
                && basicReadResult.First().DateColumn.Day == 1
                && basicReadResult.First().DateColumn.Month == 1
                && basicReadResult.First().DateColumn.Year == 2021
                && basicReadResult.First().FloatColumn == 12.23);
        }

        [Test, Order(1)]
        public async Task RD_MultiRecord()
        {
            var basicReadResult = await cli.Read<TestClass>("SELECT TextColumn, IntegerColumn, DateColumn, FloatColumn from ScalarTester order by IntegerColumn ASC limit 2");
            Assert.IsTrue(
                basicReadResult.Count() == 2
                && basicReadResult.First().TextColumn == "testData"
                && basicReadResult.First().IntegerColumn == 123
                && basicReadResult.First().DateColumn.Day == 1
                && basicReadResult.First().DateColumn.Month == 1
                && basicReadResult.First().DateColumn.Year == 2021
                && basicReadResult.First().FloatColumn == 12.23);
        }
    }
}
