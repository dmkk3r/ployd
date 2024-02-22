namespace Ployd.Deploy.Services.OperatingSystem;

public class Unix : IOperatingSystem {
    public string NormalizePath(string path) {
        return path.Replace("\\", "/");
    }
}