using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public class StationService {

        #region Fields

        private readonly StationRepository _stationRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public StationService() {
            this._stationRepository = new StationRepository();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        public virtual Station GetStation(int id) {
            return _stationRepository.GetEntity(id);
        }

        public virtual List<Station> GetAllStations() {
            return _stationRepository.GetEntities();
        }

        public virtual List<Station> GetEntities(int pid) {
            return _stationRepository.GetEntities(pid);
        }

        public virtual List<Station> GetStations(int gid) {
            if(gid == 10078) return this.GetAllStations();
            return _stationRepository.GetGroupEntities(gid);
        }

        public virtual List<Station> GetStations(int gid, int pid) {
            if(gid == 10078) return this.GetEntities(pid);
            return _stationRepository.GetGroupEntities(gid, pid);
        }

        public virtual Dictionary<int, string> GetTypes() {
            return _stationRepository.GetTypes();
        }

        #endregion

    }
}
