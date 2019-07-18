using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class HisValue {
        public int PointID { get; set; }

        public float Value { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
