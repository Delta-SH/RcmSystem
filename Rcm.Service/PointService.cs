using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class PointService {

        #region Fields

        private readonly PointRepository _pointRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public PointService() {
            this._pointRepository = new PointRepository();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        public virtual List<Point> GetPoints(bool aic,bool aoc,bool dic,bool doc) {
            var points = new List<Point>();

            if(aic)
                points.AddRange(_pointRepository.GetAI());

            if(aoc)
                points.AddRange(_pointRepository.GetAO());

            if(dic)
                points.AddRange(_pointRepository.GetDI());

            if(doc)
                points.AddRange(_pointRepository.GetDO());

            return points;
        }

        public virtual List<Point> GetPoints(int pid, bool aic, bool aoc, bool dic, bool doc) {
            var points = new List<Point>();

            if(aic)
                points.AddRange(_pointRepository.GetAIP(pid));

            if(aoc)
                points.AddRange(_pointRepository.GetAOP(pid));

            if(dic)
                points.AddRange(_pointRepository.GetDIP(pid));

            if(doc)
                points.AddRange(_pointRepository.GetDOP(pid));

            return points;
        }

        #endregion

    }
}
