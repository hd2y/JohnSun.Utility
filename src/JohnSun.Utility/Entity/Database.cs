using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class Database
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 数据库连接工厂
        /// </summary>
        public string DbProviderFactory { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 连接字符串是否加密
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Describe { get; set; }
    }
}
