using System.Collections.Generic;

namespace Peppy.Core
{
    public interface IApiInfo
    {
        /// <summary>
        /// server-side-bound listener address
        /// </summary>
        string BindAddress { get; }

        /// <summary>
        /// service-side-bound listening ports
        /// </summary>
        int BindPort { get; }

        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 版本
        /// </summary>
        string Version { get; }

        /// <summary>
        /// swagger信息
        /// </summary>
        SwaggerInfo SwaggerInfo { get; }

        /// <summary>
        /// 资源名称
        /// </summary>
        string[] Scopes { get; }

        /// <summary>
        /// 链接数据库名称
        /// </summary>
        string ConnectionStringName { get; }
    }

    public class SwaggerInfo
    {
        public SwaggerInfo(
            List<string> xmlFiles,
            string title,
            string version)
        {
            XmlFiles = xmlFiles;
            Title = title;
            Version = version;
        }

        /// <summary>
        /// xml地址
        /// </summary>
        public List<string> XmlFiles { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    public class SwaggerAuthInfo
    {
        public string ClientId { get; }
        public string Secret { get; }
        public string Realm { get; }

        public SwaggerAuthInfo(
            string clientId,
            string secret,
            string realm
            )
        {
            ClientId = clientId;
            Secret = secret;
            Realm = realm;
        }
    }
}