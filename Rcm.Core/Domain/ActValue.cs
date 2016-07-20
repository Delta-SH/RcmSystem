using Rcm.Core.Enum;
using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class ActValue {
        public int DevId { get; set; }

        public int NodeId { get; set; }

        public EnmScType NodeType { get; set; }

        public double Value { get; set; }

        public EnmPointStatus State { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
