using Rcm.Core.Domain;
using Rcm.Data;
using System.Collections.Generic;

namespace Rcm.Service {
    public class GVideoService {

        #region Fields
        private readonly GVideoRepository _gvideorepository;
        #endregion

        #region Ctor
        public GVideoService() {
            this._gvideorepository = new GVideoRepository();
        }
        #endregion

        #region Methods
        public List<GVideo> GetEntities() {
            return _gvideorepository.GetEntities();
        }
        #endregion

    }
}