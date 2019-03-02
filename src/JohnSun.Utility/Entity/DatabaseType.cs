using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Access/Excel等
        /// </summary>
        OleDb,

        /// <summary>
        /// Oracle
        /// </summary>
        OracleClient,

        /// <summary>
        /// SQLite
        /// </summary>
        SQLite,

        /// <summary>
        /// MySQL
        /// </summary>
        MySqlClient,

        /// <summary>
        /// Sql Server
        /// </summary>
        SqlClient,

        /// <summary>
        /// Sql Server CE
        /// </summary>
        SqlClientCE,

        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSqlClient,

        /// <summary>
        /// Firebase
        /// </summary>
        Firebase,

        /// <summary>
        /// Redis
        /// </summary>
        Redis,
    }
}
