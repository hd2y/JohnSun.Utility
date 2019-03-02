using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 数据库配置文件清单
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 生成 DbConfig.json 文件内容
        /// </summary>
        /// <param name="configFiles">配置内容</param>
        /// <returns>json 格式配置内容</returns>
        public static string GenerateDbConfigFile(params ConfigFile[] configFiles)
        {
            DbConfig dbConfig = new DbConfig() { ConfigFiles = configFiles };
            return Newtonsoft.Json.JsonConvert.SerializeObject(dbConfig, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// 生成 DbDetail.json 文件内容
        /// </summary>
        /// <param name="databases">配置内容</param>
        /// <returns>json 格式配置内容</returns>
        public static string GenerateDbDetailFile(params Database[] databases)
        {
            DbDetail dbDetail = new DbDetail() { Databases = databases };
            return Newtonsoft.Json.JsonConvert.SerializeObject(dbDetail, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// 数据库连接配置文件
        /// </summary>
        public ConfigFile[] ConfigFiles { get; set; }
    }
}
