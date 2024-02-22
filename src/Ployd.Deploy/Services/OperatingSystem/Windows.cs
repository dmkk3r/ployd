namespace Ployd.Deploy.Services.OperatingSystem;

public class Windows : IOperatingSystem {
    public string NormalizePath(string path) {
        return path.Replace("/", "\\");
    }
}