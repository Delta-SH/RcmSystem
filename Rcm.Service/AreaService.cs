using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rcm.Service {
    public partial class AreaService {

        #region Fields

        private readonly AreaRepository _areaRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AreaService() {
            this._areaRepository = new AreaRepository();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        public virtual Area GetArea(int id) {
            return _areaRepository.GetEntity(id);
        }

        public virtual List<Area> GetAreas() {
            return _areaRepository.GetEntities();
        }

        public virtual List<Area> GetAreas(int gid) {
            if(gid == 10078) return this.GetAreas();

            var key = string.Format("rcms-group-{0}", gid);
            if(_cacheManager.IsSet(key))
                return _cacheManager.Get<List<Area>>(key);

            var result = new List<Area>();
            var areas = _areaRepository.GetEntities();
            var groups = _areaRepository.GetGroupKeys(gid);
            foreach(var gk in groups) {
                var current = areas.Find(a => a.Id == gk);
                if(current != null) {
                    if(!result.Contains(current))
                        result.Add(current);

                    var index = 1;
                    while(true) {
                        current = areas.Find(a => a.Id == current.LastId);
                        if(index > 10 || current == null) break;

                        if(!result.Contains(current))
                            result.Add(current);

                        index++;
                    }
                }
            }

            if(result.Count > 0) {
                result = result.OrderBy(a => a.Name).ToList();
                _cacheManager.Set<List<Area>>(key, result);
            }

            return result;
        }

        #endregion

    }
}
