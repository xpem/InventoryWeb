using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Resps
{
    public class ApiResp
    {
        public bool Success { get; set; }

        public object? Content { get; set; }

        public ErrorTypes? Error { get; set; }

        public bool TryRefreshToken { get; set; }
    }
}
