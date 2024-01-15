namespace OLT.Core.Common.Tests.Assets;

public class TestHostService : OltHostServiceBase
{
    public TestHostService(string envName, string appName)
    {
        EnvironmentName = envName;
        ApplicationName = appName;
    }

    public override string EnvironmentName { get; }
    public override string ApplicationName { get; }

    public override string ResolveRelativePath(string filePath)
    {            
        return filePath.Replace("~/", $"/{EnvironmentName}/{ApplicationName}/");
    }
}