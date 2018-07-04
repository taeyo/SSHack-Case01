using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sshack
{ 
    public class BaseInfo
    {
        public string RequestID { get; set; }
        public QueryCommand Command { get; set; }
    }

    public class RequestInfo : BaseInfo
    {
        public int Div { get; set; }
    }
    
    public class QueryCommand
    {
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
    }
}
