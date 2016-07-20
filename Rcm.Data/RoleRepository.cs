using iPem.Data.Common;
using Rcm.Core.Data;
using Rcm.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class RoleRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public RoleRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual Role GetEntity(int id, bool autoload) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.Int) };
            parms[0].Value = id;

            Role entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Role_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new Role();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    if(autoload) entity.Values = this.GetValues(entity.Id);
                }
            }
            return entity;
        }

        public virtual List<Role> GetEntities(bool autoload) {
            var entities = new List<Role>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Role_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Role();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    if(autoload) entity.Values = this.GetValues(entity.Id);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<int> GetValues(int id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.Int) };
            parms[0].Value = id;

            var entities = new List<int>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Role_Repository_GetValues, parms)) {
                while(rdr.Read()) {
                    entities.Add(SqlTypeConverter.DBNullInt32Handler(rdr["Value"]));
                }
            }
            return entities;
        }

        #endregion

    }
}
