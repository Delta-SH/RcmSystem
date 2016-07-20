using System;
using System.Collections.Generic;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class Role {
        public int Id { get; set; }

        public string Name { get; set; }

        public int LastId { get; set; }

        public bool Enabled { get; set; }

        public List<int> Values { get; set; }
    }
}
