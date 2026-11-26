using System.Reflection;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
public static class VersionHelper
{
    /// <summary>
    /// Applikationens version fra SharedAssemblyInfo.cs, fx "1.1.2".
    /// </summary>
    public static readonly string Version = ReadVersion();

    public static string AppVersion(this HtmlHelper html)
    {
        return Version;
    }

    private static string ReadVersion()
    {
        var assembly = typeof(VersionHelper).Assembly;
        var info = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (info != null && !string.IsNullOrWhiteSpace(info.InformationalVersion))
        {
            return info.InformationalVersion;
        }

        var version = assembly.GetName().Version;
        return version.ToString(3);
    }
}
