using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class Area {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string MID { get; set; }

        public string ExpSet { get; set; }

        public int LastId { get; set; }
    }
}
