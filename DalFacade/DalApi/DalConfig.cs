namespace DalApi;
using System.Xml.Linq;

/// <summary>
/// Loads and manages DAL configuration data from an XML file,
/// including the active DAL implementation and available packages.
/// </summary>
static class DalConfig
{
    /// <summary>
    /// Represents metadata describing a DAL implementation,
    /// including its package, namespace, and class name.
    /// </summary>
    internal record DalImplementation
    (
        string Package,
        string Namespace,
        string Class
    );

    /// <summary>
    /// Stores the name of the active DAL implementation.
    /// </summary>
    internal static string s_dalName;

    /// <summary>
    /// Stores all available DAL implementations mapped by configuration key.
    /// </summary>
    internal static Dictionary<string, DalImplementation> s_dalPackages;

    /// <summary>
    /// Initializes the DAL configuration by loading and parsing
    /// the dal-config.xml file.
    /// </summary>
    static DalConfig()
    {
        XElement dalConfig = XElement.Load(@"..\xml\dal-config.xml") ??
            throw new DalConfigException("dal-config.xml file is not found");

        s_dalName =
            dalConfig.Element("dal")?.Value ??
            throw new DalConfigException("<dal> element is missing");

        var packages = dalConfig.Element("dal-packages")?.Elements() ??
            throw new DalConfigException("<dal-packages> element is missing");

        s_dalPackages =
            (from item in packages
             let pkg = item.Value
             let ns = item.Attribute("namespace")?.Value ?? "Dal"
             let cls = item.Attribute("class")?.Value ?? pkg
             select (item.Name, new DalImplementation(pkg, ns, cls))
            ).ToDictionary(p => "" + p.Name, p => p.Item2);
    }
}

/// <summary>
/// Represents errors related to loading or parsing the DAL configuration.
/// </summary>
[Serializable]
public class DalConfigException : Exception
{
    /// <summary>
    /// Initializes a new exception with a specific error message.
    /// </summary>
    public DalConfigException(string msg) : base(msg) { }

    /// <summary>
    /// Initializes a new exception with a specific error message
    /// and an inner exception.
    /// </summary>
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}
