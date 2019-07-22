using Rcm.Core.Enum;
using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class Point {
        public int Id { get; set; }

        public EnmScType Type { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string AuxSet { get; set; }

        public int PId { get; set; }

        public bool Enabled { get; set; }
    }
}
