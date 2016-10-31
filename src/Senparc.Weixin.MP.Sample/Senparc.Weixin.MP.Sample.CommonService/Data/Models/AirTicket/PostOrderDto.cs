using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.MP.Sample.CommonService.Data.Models
{
    public class PostOrderDto
    {
        public PostOrderDto()
        {
            Lines = new List<PostOrderLineDto>();
        }
        public string Title { get; set; }
        public string OrderNo { get; set; }
        public string OpenId { get; set; }
        public string Telephone { get; set; }
        public DateTime Dtime { get; set; }
        

        /// <summary>
        /// 票數量
        /// </summary>
        public int TicketCount{get { return Lines.Count; }}
        /// <summary>
        /// 總價
        /// </summary>
        public decimal Amount { get; set; }

        public List<PostOrderLineDto> Lines { get; set; }
    }

    public class PostOrderLineDto
    { 
        public string Name { get; set; }
        public string IdentityCardNo { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public double UnitPrice { get; set; }
    }
}
