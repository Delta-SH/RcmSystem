using System;

namespace Rcm.Site.Models {
    public class ComboItem<T1, T2> {
        public T1 id { get; set; }

        public T2 text { get; set; }

        public string comment { get; set; }
    }
}