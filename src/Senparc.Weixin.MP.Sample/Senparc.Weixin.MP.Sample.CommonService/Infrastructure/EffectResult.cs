using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.MP.Sample.CommonService.Infrastructure
{
    public class EffectResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }

    public class EffectResult : EffectResult<string>
    {
        
    }
}
