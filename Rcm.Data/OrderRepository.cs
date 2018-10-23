using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Data.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class OrderRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public OrderRepository() {
            this._databaseConnectionString = SqlHelper.RcmsHisConnection;
        }

        #endregion

        #region Methods

        public virtual void SetEntities(List<Order> entities) {
            SqlParameter[] parms = { new SqlParameter("@NodeID", SqlDbType.Int),
                                     new SqlParameter("@NodeType", SqlDbType.Int),
                                     new SqlParameter("@OpType", SqlDbType.Int),
                                     new SqlParameter("@OpValue", SqlDbType.Real) };

            using(var conn = new SqlConnection(this._databaseConnectionString)) {
                conn.Open();
                var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    foreach(var entity in entities) {
                        parms[0].Value = entity.NodeId;
                        parms[1].Value = (int)entity.NodeType;
                        parms[2].Value = (int)entity.OpType;
                        parms[3].Value = SqlTypeConverter.DBNullDoubleChecker(entity.OpValue);
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlCommands_His.Sql_Order_Repository_SetEntities, parms);
                    }
                    trans.Commit();
                } catch {
                    trans.Rollback();
                    throw;
                }
            }
        }

        #endregion

    }
}
