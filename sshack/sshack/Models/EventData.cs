using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sshack
{
    [Serializable]
    public class EventData
    {
        public string LINE { get; set; }
        public string AREA { get; set; }
        public string EQP_ID { get; set; }
        public string EQP_INDEX { get; set; }
        public string PARAM_VALUE { get; set; }
        public string UPPER_SPEC { get; set; }
        public string LOWER_SPEC { get; set; }
        public string ERD_PARAM_INDEX { get; set; }
        public string COL_NAME { get; set; }
        public string CH_PARAM_NAME { get; set; }
        public string PARAM_INDEX { get; set; }
        public string PARAM_NAME { get; set; }
        public string ACT_TIME { get; set; }
        public string SLOT_NO { get; set; }
        public string CH_STEP { get; set; }
        public string LOT_ID { get; set; }
        public string PROC_PLAN_ID { get; set; }
        public string PART_ID { get; set; }
        public string PPID { get; set; }
        public string RECIPEID { get; set; }
        public string STEPSEQ { get; set; }
        public string WAFER_NO { get; set; }
        public string CH_ID { get; set; }
        public string CH_NAME { get; set; }
        public string PARRERN { get; set; }
        public string DATA_INDEX { get; set; }
    }
}
