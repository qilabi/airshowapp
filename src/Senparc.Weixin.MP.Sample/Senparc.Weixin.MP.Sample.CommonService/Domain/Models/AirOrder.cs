using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.MP.Sample.CommonService.Domain.Models
{
    public class AirOrder : PostOrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
    }
}
