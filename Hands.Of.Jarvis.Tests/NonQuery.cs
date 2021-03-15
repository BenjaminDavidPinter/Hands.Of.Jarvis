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

        public NonQuery()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);
        }

        [Test, Order(1)]
        public async Task NQ_Returns_Neg1_On_Single_Record_Select()
        {
            var result = await cli.ExecuteNonQuery("select * from scalartester limit 1");
            Assert.IsTrue(result == -1);
        }

        [Test, Order(2)]
        public async Task NQ_Returns_1_On_Single_Record_Insert()
        {
            var result = await cli.ExecuteNonQuery("insert into ScalarTester (TextColumn) values('hello!')");
            Assert.IsTrue(result == 1);
        }

        [Test, Order(3)]
        public async Task NQ_Returns_2_On_Multi_Record_Insert()
        {
            var result = await cli.ExecuteNonQuery("insert into ScalarTester (TextColumn) select 'hello!' union all select 'hello again!'");
            Assert.IsTrue(result == 2);
        }

        [Test, Order(4)]
        public async Task NQ_Returns_GT1_On_Delete()
        {
            var result = await cli.ExecuteNonQuery("delete from Scalartester");
            Assert.IsTrue(result > 2);
        }
    }
}
