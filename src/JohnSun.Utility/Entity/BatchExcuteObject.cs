using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 批处理提交对象
    /// </summary>
    public class BatchExcuteObject
    {
        /// <summary>
        /// 批处理提交对象构造函数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="zeroRowRollback">sql语句影响行数0行是否回滚</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">sql语句类型</param>
        public BatchExcuteObject(string sql, object param = null, bool zeroRowRollback = false, int? commandTimeout = null, CommandType? commandType = null)
        {

        }

        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Param { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// 语句类型
        /// </summary>
        public CommandType? CommandType { get; set; }

        /// <summary>
        /// 影响行数0行回滚并抛异常
        /// </summary>
        public bool NoAffectRollback { get; set; }

        /// <summary>
        /// 受影响行数
        /// </summary>
        public int? AffectedRows { get; set; }
    }
}
