using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class HisValue {
        public int PointID { get; set; }

        public double Value { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
