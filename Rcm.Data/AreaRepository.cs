using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Data.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class AreaRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AreaRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual Area GetEntity(int id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.Int) };
            parms[0].Value = id;

            Area entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Area_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new Area();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                }
            }
            return entity;
        }

        public virtual List<Area> GetEntities() {
            var entities = new List<Area>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Area_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Area();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.LastId = SqlTypeConverter.DBNullInt32Handler(rdr["LastId"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<int> GetGroupKeys(int gid) {
            SqlParameter[] parms = { new SqlParameter("@GroupId", SqlDbType.Int) };
            parms[0].Value = gid;

            var entities = new List<int>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Area_Repository_GetGroupKeys, parms)) {
                while(rdr.Read()) {
                    var id = SqlTypeConverter.DBNullInt32Handler(rdr["AreaID"]);
                    if(!entities.Contains(id))
                        entities.Add(id);
                }
            }
            return entities;
        }

        #endregion

    }
}
