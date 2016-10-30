using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.MP.Sample.CommonService.Data;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using ServiceStack.OrmLite;

namespace Senparc.Weixin.DB.Tests
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = MSSQLProvider.CreateConnection().Open())
            {
                var orders= db.Select<orders>()
                         .Where(s => s.order_no.Equals("B15071411270483"));
                Console.WriteLine(orders.Count());
            }
        }
    }
}
