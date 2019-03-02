using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 数据库访问帮助类（基于Dapper）
    /// </summary>
    public class DbHelper : IDisposable
    {
        #region 连接属性
        /// <summary>
        /// 数据库连接字符串 未配置默认读取LIS
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库引擎工厂
        /// </summary>
        public DbProviderFactory ProviderFactory { get; set; }

        /// <summary>
        /// 是否执行事务
        /// </summary>
        public bool IsBeginTransaction { get; set; }

        /// <summary>
        /// 获取事务实体
        /// </summary>
        public DbTransaction Transaction { get; set; }

        private DbConnection _dbConnection;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = ProviderFactory.CreateConnection();
                    _dbConnection.ConnectionString = ConnectionString;
                }
                return _dbConnection;
            }
            set
            {
                _dbConnection = value;
            }
        }

        /// <summary>
        /// 当前数据库连接配置信息（通过配置配置名称初始化DbHelper对象）
        /// </summary>
        public Database Database { get; set; }
        #endregion

        #region 静态属性字段与私有静态方法
        /// <summary>
        /// 配置文件路径，默认基目录
        /// </summary>
        public static string ConfigFilePath = "DbConfig.json";

        /// <summary>
        /// 数据库连接字符串RSA加密公钥与私钥
        /// </summary>
        public static KeyValuePair<string, string> SecurityConfigKeys;

        /// <summary>
        /// 获取字符串加密后或加密前的格式
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="isEncrypt">操作：true-传入的是加密后的数据库连接字符串 false-传入的是未加密的数据库连接字符串</param>
        /// <returns>转换结果</returns>
        public static string GetSecurityConnectionString(string connectionString, bool isEncrypt = true)
        {
            if (isEncrypt)
            {
                return SecurityHelper.DecryptByRSAFromXmlString(connectionString, SecurityConfigKeys.Key);
            }
            else
            {
                return SecurityHelper.EncryptByRSAFromXmlString(connectionString, SecurityConfigKeys.Value);
            }
        }

        private static DbDetail _dbDetail = null;

        /// <summary>
        /// 当前加载的配置项
        /// </summary>
        public static DbDetail DbDetail
        {
            get
            {
                if (_dbDetail == null)
                {
                    if (!File.Exists(ConfigFilePath))
                    {
                        ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(ConfigFilePath));
                    }
                    if (File.Exists(ConfigFilePath))
                    {
                        DbConfig config = Newtonsoft.Json.JsonConvert.DeserializeObject<DbConfig>(File.ReadAllText(ConfigFilePath, Encoding.UTF8));
                        ConfigFile configFile = config.ConfigFiles.ToList().Find(c => c.Active);
                        if (configFile != null)
                        {
                            if (!File.Exists(configFile.FileName))
                            {
                                configFile.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(configFile.FileName));
                            }
                            if (File.Exists(configFile.FileName))
                            {
                                _dbDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<DbDetail>(File.ReadAllText(configFile.FileName, Encoding.UTF8));
                            }
                            else
                            {
                                throw new FileNotFoundException("无法获取DbConfig配置的数据库清单文件。");
                            }
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException("无法获取数据库配置文件。");
                    }
                }
                return _dbDetail;
            }
        }

        private static Dictionary<string, DbProviderFactory> _dbProviderFactories = new Dictionary<string, DbProviderFactory>();

        private static DbProviderFactory GetDbProviderFactory(string dbName)
        {
            if (!_dbProviderFactories.ContainsKey(dbName))
            {
                Database database = DbDetail.Databases.ToList().Find(db => string.Equals(db.Name, dbName, StringComparison.OrdinalIgnoreCase));
                if (database != null)
                {
                    _dbProviderFactories[dbName] = Type.GetType(database.DbProviderFactory).GetField("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as DbProviderFactory;
                }
            }
            return _dbProviderFactories[dbName];
        }

        private static Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();

        private static string GetConnectionString(string dbName)
        {
            if (!_connectionStrings.ContainsKey(dbName))
            {
                Database database = DbDetail.Databases.ToList().Find(db => string.Equals(db.Name, dbName, StringComparison.OrdinalIgnoreCase));
                if (database != null)
                {
                    if (string.IsNullOrEmpty(database.ConnectionString))
                    {
                        throw new Exception("未解析到配置文件ConnectionString节点对于数据库连接的配置信息！");
                    }
                    if (database.Encrypt)
                    {
                        try
                        {
                            _connectionStrings[dbName] = GetSecurityConnectionString(database.ConnectionString);
                        }
                        catch
                        {
                            throw new Exception("加密后的数据库连接字符串解密失败！");
                        }
                    }
                    else
                    {
                        _connectionStrings[dbName] = database.ConnectionString;
                    }
                }
            }
            return _connectionStrings[dbName];
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory">数据库连接提供工厂</param>
        /// <param name="connectionString">连接字符串</param>
        public DbHelper(DbProviderFactory factory, string connectionString)
        {
            ProviderFactory = factory;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 构造函数：DbDetail数据库配置文件中指定的数据库连接名
        /// </summary>
        /// <param name="dbName">数据库配置文件名</param>
        public DbHelper(string dbName)
        {
            ProviderFactory = GetDbProviderFactory(dbName);
            ConnectionString = GetConnectionString(dbName);
            Database = DbDetail.Databases.ToList().Find(db => db.Name == dbName);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                Transaction = Connection.BeginTransaction();
                IsBeginTransaction = true;
            }
            catch
            {
                IsBeginTransaction = false;
                if (Connection.State == ConnectionState.Open)
                {
                    Transaction.Dispose();
                    Connection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (IsBeginTransaction && Transaction != null)
                {
                    Transaction.Commit();
                }
            }
            catch
            {
                try
                {
                    Transaction.Rollback();
                    throw;
                }
                catch
                {
                    throw;
                }
            }
            finally
            {
                IsBeginTransaction = false;
                if (Connection.State == ConnectionState.Open)
                {
                    Transaction.Dispose();
                    Connection.Close();
                }
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (IsBeginTransaction)
            {
                Transaction.Rollback();
            }
            IsBeginTransaction = false;
            if (Connection.State == ConnectionState.Open)
            {
                Transaction.Dispose();
                Connection.Close();
            }
        }

        private T ExecuteWithTransaction<T>(Func<T> func)
        {
            if (IsBeginTransaction)
            {
                try
                {
                    return func.Invoke();
                }
                catch
                {
                    RollbackTransaction();
                    throw;
                }

            }
            else
            {
                bool wasClose = Connection.State == ConnectionState.Closed;
                try
                {
                    T result = func.Invoke();
                    return result;
                }
                finally
                {
                    if (wasClose)
                        Connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，并获取影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>受影响行数</returns>
        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteWithTransaction(() => Connection.Execute(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType));
        }

        /// <summary>
        /// 执行SQL语句，并获取IDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            return ExecuteWithTransaction(() => Connection.ExecuteReader(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType));
        }

        /// <summary>
        /// 执行SQL语句，并获取一行结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>第一行结果</returns>
        public object ExecuteScalar(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return ExecuteWithTransaction(() => Connection.ExecuteScalar(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType));
        }

        /// <summary>
        /// 执行SQL语句，并获取一行结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>第一行结果</returns>
        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return (T)Convert.ChangeType(ExecuteScalar(sql, param, commandTimeout, commandType), typeof(T));
        }

        /// <summary>
        /// 查询DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <param name="tables">一个字符串，从该数组 Load 方法检索表名称信息。</param>
        /// <returns>查询结果</returns>
        public DataSet GetDataSet(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null, params string[] tables)
        {
            return ExecuteWithTransaction(() =>
            {
                DataSet dataSet = new DataSet();
                IDataReader reader = Connection.ExecuteReader(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
                dataSet.Load(reader, loadOption ?? LoadOption.PreserveChanges, tables);
                return dataSet;
            });
        }

        /// <summary>
        /// 查询DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <returns>查询结果</returns>
        public DataTable GetDataTable(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null)
        {
            return ExecuteWithTransaction(() =>
            {
                DataTable dataTable = new DataTable();
                IDataReader reader = Connection.ExecuteReader(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
                dataTable.Load(reader, loadOption ?? LoadOption.PreserveChanges);
                return dataTable;
            });
        }

        /// <summary>
        /// 查询DataRow
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="index">指定行索引</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <returns>查询结果</returns>
        public DataRow GetDataRow(string sql, object param = null, int index = 0, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null)
        {
            return ExecuteWithTransaction(() =>
            {
                DataTable dataTable = GetDataTable(sql, param, commandTimeout, commandType, loadOption);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[index];
                }
                return null;
            });
        }

        /// <summary>
        /// 执行SQL，查询返回一组数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Query<T>(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// 查询一组具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Query(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public T QueryFirst<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QueryFirst<T>(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public dynamic QueryFirst(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QueryFirst(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public T QueryFirstOrDefault<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QueryFirstOrDefault<T>(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public dynamic QueryFirstOrDefault(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QueryFirstOrDefault(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public T QuerySingle<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QuerySingle<T>(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public dynamic QuerySingle(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QuerySingle(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public T QuerySingleOrDefault<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QuerySingleOrDefault<T>(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public dynamic QuerySingleOrDefault(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QuerySingleOrDefault(sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout, commandType);
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键值，必须标识了“[Key]”特性</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>查询到的实体</returns>
        public T Get<T>(int id, int? commandTimeout = null) where T : class
        {
            return Connection.Get<T>(id, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">主键值，必须标识了“[Key]”特性</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>查询到的实体</returns>
        public T Get<T>(string id, int? commandTimeout = null) where T : class
        {
            return Connection.Get<T>(id, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TResult">查询实体类型</typeparam>
        /// <param name="page">当前页码</param>
        /// <param name="take">每页记录数</param>
        /// <param name="sql">分页sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的分页对象</returns>
        public Paged<TResult> QueryPage<TResult>(int page, int take, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where TResult : class
        {
            return Connection.QueryPage<TResult>(page, take, sql, param, IsBeginTransaction && Transaction != null ? Transaction : null, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// 获取与 T 类型相关的全部实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>实体集合</returns>
        public IEnumerable<T> GetAll<T>(int? commandTimeout = null) where T : class
        {
            return Connection.GetAll<T>(Transaction, commandTimeout);
        }

        /// <summary>
        /// 插入实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToInsert">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>实体Id或受影响行数</returns>
        public long Insert<T>(T entityToInsert, int? commandTimeout = null) where T : class
        {
            return Connection.Insert<T>(entityToInsert, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 删除实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToDelete">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public bool Delete<T>(T entityToDelete, int? commandTimeout = null) where T : class
        {
            return Connection.Delete<T>(entityToDelete, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 删除与 T 类型相关的全部实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public bool DeleteAll<T>(int? commandTimeout = null) where T : class
        {
            return Connection.DeleteAll<T>(IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 更新实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToUpdate">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public bool Update<T>(T entityToUpdate, int? commandTimeout = null) where T : class
        {
            return Connection.Update<T>(entityToUpdate, IsBeginTransaction && Transaction != null ? Transaction : null, commandTimeout);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                CommitTransaction();
                Connection.Dispose();
            }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 执行SQL语句，并获取影响行数
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>受影响行数</returns>
        public static int Execute(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Execute(sql, param, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行SQL语句，并获取IDataReader
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            DbHelper helper = new DbHelper(db);
            return helper.ExecuteReader(sql, param, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行SQL语句，并获取一行结果
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>第一行结果</returns>
        public static object ExecuteScalar(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.ExecuteScalar(sql, param, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行SQL语句，并获取一行结果
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>第一行结果</returns>
        public static T ExecuteScalar<T>(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.ExecuteScalar<T>(sql, param, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询DataSet
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <param name="tables">一个字符串，从该数组 Load 方法检索表名称信息。</param>
        /// <returns>查询结果</returns>
        public static DataSet GetDataSet(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null, params string[] tables)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.GetDataSet(sql, param, commandTimeout, commandType, loadOption, tables);
            }
        }

        /// <summary>
        /// 查询DataTable
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <returns>查询结果</returns>
        public static DataTable GetDataTable(string db, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.GetDataTable(sql, param, commandTimeout, commandType, loadOption);
            }
        }

        /// <summary>
        /// 查询DataRow
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="index">指定行索引</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="loadOption">取值范围为 System.Data.LoadOption 枚举，指示如何中的现有行 System.Data.DataTable 实例内 DataSet 组合在一起共享相同的主键的传入行。</param>
        /// <returns>查询结果</returns>
        public static DataRow GetDataRow(string db, string sql, object param = null, int index = 0, int? commandTimeout = null, CommandType? commandType = null, LoadOption? loadOption = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.GetDataRow(sql, param, index, commandTimeout, commandType, loadOption);
            }
        }

        /// <summary>
        /// 执行SQL，查询返回一组数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public static IEnumerable<T> Query<T>(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Query<T>(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询一组具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public static IEnumerable<dynamic> Query(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Query(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public static T QueryFirst<T>(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QueryFirst<T>(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public static dynamic QueryFirst(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QueryFirst(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public static T QueryFirstOrDefault<T>(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QueryFirstOrDefault<T>(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public static dynamic QueryFirstOrDefault(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QueryFirstOrDefault(sql, param, buffered, commandTimeout, commandType);
            }
        }
        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public static T QuerySingle<T>(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QuerySingle<T>(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public static dynamic QuerySingle(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QuerySingle(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行SQL，查询返回一个数据类型对象 T
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 T 对象</returns>
        public static T QuerySingleOrDefault<T>(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QuerySingleOrDefault<T>(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询一个具有与列匹配的属性的 dynamic 对象。
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的 dynamic 对象</returns>
        public static dynamic QuerySingleOrDefault(string db, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QuerySingleOrDefault(sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="id">主键值，必须标识了“[Key]”特性</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>查询到的实体</returns>
        public static T Get<T>(string db, int id, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Get<T>(id, commandTimeout);
            }
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="id">主键值，必须标识了“[Key]”特性</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>查询到的实体</returns>
        public static T Get<T>(string db, string id, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Get<T>(id, commandTimeout);
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TResult">查询实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="page">当前页码</param>
        /// <param name="take">每页记录数</param>
        /// <param name="sql">分页sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="buffered">是否将结果缓冲在内存中。</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>查询返回的分页对象</returns>
        public static Paged<TResult> QueryPage<TResult>(string db, int page, int take, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where TResult : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.QueryPage<TResult>(page, take, sql, param, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 获取与 T 类型相关的全部实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>实体集合</returns>
        public static IEnumerable<T> GetAll<T>(string db, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.GetAll<T>(commandTimeout);
            }
        }

        /// <summary>
        /// 插入实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="entityToInsert">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>实体Id或受影响行数</returns>
        public static long Insert<T>(string db, T entityToInsert, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Insert(entityToInsert, commandTimeout);
            }
        }

        /// <summary>
        /// 删除实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="entityToDelete">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public static bool Delete<T>(string db, T entityToDelete, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Delete(entityToDelete, commandTimeout);
            }
        }

        /// <summary>
        /// 删除与 T 类型相关的全部实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public static bool DeleteAll<T>(string db, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.DeleteAll<T>(commandTimeout);
            }
        }

        /// <summary>
        /// 更新实体对象或实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="db">数据库名</param>
        /// <param name="entityToUpdate">实体或实体集合</param>
        /// <param name="commandTimeout">执行超时时间</param>
        /// <returns>是否操作成功</returns>
        public static bool Update<T>(string db, T entityToUpdate, int? commandTimeout = null) where T : class
        {
            using (DbHelper helper = new DbHelper(db))
            {
                return helper.Update(entityToUpdate, commandTimeout);
            }
        }

        /// <summary>
        /// DML SQL 批处理
        /// </summary>
        /// <param name="db">数据库名</param>
        /// <param name="sqls">需要处理的SQL语句对象集合 （DML 操作）</param>
        /// <returns>执行结果</returns>
        public static bool BatchExecute(string db, List<BatchExcuteObject> sqls)
        {
            using (DbHelper helper = new DbHelper(db))
            {
                helper.BeginTransaction();
                try
                {
                    foreach (BatchExcuteObject sql in sqls)
                    {
                        int rows = helper.Execute(sql.Sql, sql.Param, sql.CommandTimeout, sql.CommandType);
                        sql.AffectedRows = rows;
                        if (sql.NoAffectRollback && rows == 0)
                        {
                            Exception exc = new Exception("sql批处理，发现未更改数据库的sql命令。");
                            exc.Data.Add("Error_Batch_Excute_Object", sql);
                            throw exc;
                        }
                    }

                    helper.CommitTransaction();
                    return true;
                }
                catch (Exception)
                {
                    helper.RollbackTransaction();
                    throw;
                }
            }
        }
        #endregion
    }
}
