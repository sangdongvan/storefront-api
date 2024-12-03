namespace StoreFront.Config;

public static class GlobalConfigExtensions
{
    public static GlobalConfig AddGlobalConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var globalConfig = new GlobalConfig();
        configuration.GetSection("GlobalConfig").Bind(globalConfig);

        services.AddSingleton(s => globalConfig);

        return globalConfig;
    }
}
