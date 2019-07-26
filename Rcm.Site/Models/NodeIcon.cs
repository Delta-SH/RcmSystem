using System;

namespace Rcm.Site.Models {
    [Serializable]
    public class NodeIcon<T> {
        public T id { get; set; }

        public int level { get; set; }
    }
}