using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class StaticValue {
        public int PointID { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public float AvgValue { get; set; }

        public float MaxValue { get; set; }

        public DateTime MaxTime { get; set; }

        public float MinValue { get; set; }

        public DateTime MinTime { get; set; }
    }
}
