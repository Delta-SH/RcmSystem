using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Data.Common;
using System.Collections.Generic;
using System.Data;

namespace Rcm.Data {
    public class GVideoRepository {

        #region Fields
        private readonly string _databaseConnectionString;
        #endregion

        #region Cotr
        public GVideoRepository() {
            _databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }
        #endregion

        #region Methods
        public List<GVideo> GetEntities() {
            var entities = new List<GVideo>();
            using (var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Gvideo_Repository_GetEntities)) {
                while (rdr.Read()) {
                    var entity = new GVideo();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["ID"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Type = SqlTypeConverter.DBNullInt32Handler(rdr["Type"]);
                    entity.Ip = SqlTypeConverter.DBNullStringHandler(rdr["IP"]);
                    entity.Port = SqlTypeConverter.DBNullInt32Handler(rdr["Port"]);
                    entity.Uid = SqlTypeConverter.DBNullStringHandler(rdr["Uid"]);
                    entity.Pwd = SqlTypeConverter.DBNullStringHandler(rdr["Pwd"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.ImgPort = SqlTypeConverter.DBNullInt32Handler(rdr["ImgPort"]);
                    entity.PortId = SqlTypeConverter.DBNullInt32Handler(rdr["PortId"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}