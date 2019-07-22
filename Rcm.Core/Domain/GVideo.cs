using System;

namespace Rcm.Core.Domain {
    /// <summary>
    /// gvideo
    /// </summary>
    [Serializable]
    public class GVideo {
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string Uid { get; set; }

        public string Pwd { get; set; }

        public string AuxSet { get; set; }

        public int ImgPort { get; set; }

        public int PortId { get; set; }
    }
}
