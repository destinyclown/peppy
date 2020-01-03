namespace Peppy.Core
{
    public class ApiInfo : IApiInfo
    {
        public static IApiInfo Instantiate()
        {
            Instance = new ApiInfo();
            return Instance;
        }

        public static IApiInfo Instance { get; private set; }

        public string BindAddress => ConfigManagerConf.GetValue("ServiceDiscovery:BindAddress");

        public int BindPort => int.Parse(ConfigManagerConf.GetValue("ServiceDiscovery:BindPort"));

        public string ServiceName => ConfigManagerConf.GetValue("ServiceDiscovery:ServiceName");

        public string Version => ConfigManagerConf.GetValue("ServiceDiscovery:Version");

        public SwaggerInfo SwaggerInfo
            =>
            new SwaggerInfo(
                ConfigManagerConf.GetReferenceValue("ServiceDiscovery:SwaggerInfo:XmlFiles"),
                ConfigManagerConf.GetValue("ServiceDiscovery:SwaggerInfo:Title"),
                Version
                );

        public string[] Scopes => ConfigManagerConf.GetReferenceValue("ServiceDiscovery:Scopes").ToArray();

        public string ConnectionStringName => ConfigManagerConf.GetValue("ServiceDiscovery:ConnectionStringName");
    }
}