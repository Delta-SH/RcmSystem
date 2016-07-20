using Rcm.Core.Domain;
using Rcm.Core.Enum;
using Rcm.Data;
using System;
using System.Collections.Generic;

namespace Rcm.Service {
    public partial class UserService {

        #region Fields

        private readonly UserRepository _userRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public UserService() {
            this._userRepository = new UserRepository();
        }

        #endregion

        #region Methods

        public virtual User GetUser(string uid) {
            return _userRepository.GetEntity(uid);
        }

        public virtual List<User> GetUsers() {
            return _userRepository.GetEntities();
        }

        #endregion

        #region Validate & Password

        public virtual LoginResult Validate(string uid, string password) {
            var current = this.GetUser(uid);

            if(current == null)
                return LoginResult.NotExist;

            if(!current.Enabled)
                return LoginResult.NotEnabled;

            if(current.LimitTime < DateTime.Today)
                return LoginResult.Expired;

            if(!current.Password.Equals(password))
                return LoginResult.WrongPassword;

            return LoginResult.Successful;
        }

        #endregion

    }
}
