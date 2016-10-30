using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Senparc.Weixin.MP.Sample.Models
{
    public class PostTicketOrder
    {
        public string replies { get; set; }
        public string realNames { get; set; }
        public double unitPrice { get; set; }
        public double Quantity { get; set; }
        public int hc { get; set; }
        public int productId { get; set; }
        public string telephone { get; set; }
        public string date { get; set; }
    }
}