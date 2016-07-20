using Rcm.Core.Domain;
using Rcm.Core.Enum;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class RoleService {

        #region Fields

        private readonly RoleRepository _roleRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public RoleService() {
            this._roleRepository = new RoleRepository();
        }

        #endregion

        #region Methods

        public virtual Role GetRole(int id, bool autoload = true) {
            return _roleRepository.GetEntity(id, autoload);
        }

        public virtual List<Role> GetRoles(bool autoload = false) {
            return _roleRepository.GetEntities(autoload);
        }

        public virtual List<int> GetValues(int id) {
            return _roleRepository.GetValues(id);
        }

        #endregion

        #region Validate

        public virtual LoginResult Validate(int id) {
            var role = this.GetRole(id);

            if(role == null)
                return LoginResult.RoleNotExist;
            if(!role.Enabled)
                return LoginResult.RoleNotEnabled;

            return LoginResult.Successful;
        }

        #endregion

    }
}
