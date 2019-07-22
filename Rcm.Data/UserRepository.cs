using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Rcm.Data {
    public class UserRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public UserRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual User GetEntity(string uid) {
            SqlParameter[] parms = { new SqlParameter("@Uid", SqlDbType.VarChar, 20) };
            parms[0].Value = SqlTypeConverter.DBNullStringChecker(uid);

            User entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_User_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new User();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Uid = SqlTypeConverter.DBNullStringHandler(rdr["Uid"]);
                    entity.Password = SqlTypeConverter.DBNullStringHandler(rdr["Password"]);
                    entity.GroupId = SqlTypeConverter.DBNullInt32Handler(rdr["GroupId"]);
                    entity.DeptId = SqlTypeConverter.DBNullInt32Handler(rdr["DeptId"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                    entity.LimitTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["LimitTime"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                }
            }
            return entity;
        }

        public virtual List<User> GetEntities() {
            var entities = new List<User>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_User_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new User();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Uid = SqlTypeConverter.DBNullStringHandler(rdr["Uid"]);
                    entity.Password = SqlTypeConverter.DBNullStringHandler(rdr["Password"]);
                    entity.GroupId = SqlTypeConverter.DBNullInt32Handler(rdr["GroupId"]);
                    entity.DeptId = SqlTypeConverter.DBNullInt32Handler(rdr["DeptId"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                    entity.LimitTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["LimitTime"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

        #region Password

        /// <summary>
        /// 生成强随机盐值
        /// </summary>
        /// <returns>返回强随机盐值</returns>
        public virtual String GenerateSalt() {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 密码加密（采用SHA1加密方式）
        /// </summary>
        /// <param name="password">待加密的密码</param>
        /// <param name="salt">加密盐值</param>
        /// <returns>返回已经加密的密码</returns>
        public virtual String EncodePassword(String password, String salt) {
            var bytes = Encoding.Unicode.GetBytes(password);
            var src = Convert.FromBase64String(salt);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            var algorithm = HashAlgorithm.Create("SHA1");
            var inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        #endregion

    }
}
