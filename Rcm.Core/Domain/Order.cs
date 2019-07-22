using Rcm.Core.Enum;
using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class Order {
        public int NodeId { get; set; }

        public EnmScType NodeType { get; set; }

        public EnmSetValue OpType { get; set; }

        public double OpValue { get; set; }
    }
}
