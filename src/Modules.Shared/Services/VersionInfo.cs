using Modules.Shared.Interfaces;

namespace Modules.Shared.Services;

public class VersionInfo : IVersionInfo
{
    public string Version { get; set; }
}
