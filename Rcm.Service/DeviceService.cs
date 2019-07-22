using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public class DeviceService {

        #region Fields

        private readonly DeviceRepository _deviceRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public DeviceService() {
            this._deviceRepository = new DeviceRepository();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        public virtual Device GetDevice(int id) {
            return _deviceRepository.GetEntity(id);
        }

        public virtual List<Device> GetAllDevices() {
            return _deviceRepository.GetEntities();
        }

        public virtual List<Device> GetAllDevices(int gid) {
            if(gid == 10078) return this.GetAllDevices();
            return _deviceRepository.GetGroupEntities(gid);
        }

        public virtual List<Device> GetDevices(int pid) {
            return _deviceRepository.GetChildEntities(pid);
        }

        public virtual Dictionary<int, string> GetTypes() {
            return _deviceRepository.GetTypes();
        }

        #endregion

    }
}
