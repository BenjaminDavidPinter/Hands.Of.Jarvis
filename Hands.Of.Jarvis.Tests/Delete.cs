using System;
using Hands.Of.Jarvis.DAO;
using NUnit.Framework;

namespace Hands.Of.Jarvis.Tests
{
    [TestFixture]
    public class Delete
    {
        public JarvisClient cli { get; set; }

        public Delete()
        {
            cli = new JarvisClient(Configurations.TestDbLocation);

            //Because this is highly dependant on checking string validity, we're going to go ahead and truncate the test table here
            cli.ExecuteNonQuery("delete from ScalarTester").GetAwaiter().GetResult();

            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into ScalarTester (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData', 123, '1/1/2021', 12.23)").GetAwaiter().GetResult();

            //We're going to put known test data into the table
            cli.ExecuteNonQuery("insert into ScalarTester (TextColumn, IntegerColumn, DateColumn, FloatColumn) values ('testData2', 1234, '1/2/2021', 12.24)").GetAwaiter().GetResult();
        }

        public void DE_Should_Remove_Single_Record()
        {

        }
    }
}
