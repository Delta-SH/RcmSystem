using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class User {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Uid { get; set; }

        public string Password { get; set; }

        public int GroupId { get; set; }

        public int DeptId { get; set; }

        public DateTime LimitTime { get; set; }

        public Boolean Enabled { get; set; }
    }
}
