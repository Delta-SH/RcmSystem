using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class AlarmService {

        #region Fields

        private readonly AlarmRepository _alarmRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AlarmService() {
            this._alarmRepository = new AlarmRepository();
        }

        #endregion

        #region Methods

        public virtual List<Alarm> GetAllActAlarms() {
            return _alarmRepository.GetActEntities();
        }

        public virtual List<Alarm> GetActAlarms(DateTime start, DateTime end) {
            return _alarmRepository.GetActEntities(start, end);
        }

        public virtual List<Alarm> GetHisAlarms(DateTime start, DateTime end) {
            return _alarmRepository.GetHisEntities(start, end);
        }

        #endregion

    }
}
