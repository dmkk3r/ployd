using CliWrap;
using CliWrap.Buffered;

namespace Ployd.Deploy.Services.Docker;

public abstract class DockerCli {
    public static async Task Build(string workingDirectory, string dockerfile = "Dockerfile", string imageName = "latest",
        Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments($"build . -f {dockerfile} -t {imageName}")
            .WithWorkingDirectory(workingDirectory)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    public static async Task Run(string imageName, string containerName, (int outside, int inside) portMapping, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        if (await ContainerExists(containerName))
        {
            await Stop(containerName);
            await Remove(containerName);
        }

        await Cli.Wrap("docker")
            .WithArguments($"run -d --name {containerName} -p {portMapping.outside}:{portMapping.inside} {imageName}")
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    public static async Task Stop(string containerName, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments($"stop {containerName}")
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    public static async Task Remove(string containerName, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments($"rm {containerName}")
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    private static async Task<bool> ContainerExists(string containerName) {
        var result = await Cli.Wrap("docker")
            .WithArguments($"ps -a --format '{{{{.Names}}}}'")
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync();

        return result.StandardOutput.Contains(containerName);
    }

    public static async Task ComposeUp(string workingDirectory, string composeFile, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments("compose")
            .WithArguments($"up -f {composeFile} -d")
            .WithWorkingDirectory(workingDirectory)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    public static async Task ComposeDown(string workingDirectory, string composeFile, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments("compose")
            .WithArguments($"down -f {composeFile}")
            .WithWorkingDirectory(workingDirectory)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }

    public static async Task ComposeBuild(string workingDirectory, string composeFile, Action<string>? outputStream = null) {
        outputStream ??= Console.WriteLine;

        await Cli.Wrap("docker")
            .WithArguments("compose")
            .WithArguments($"-f {composeFile} build")
            .WithWorkingDirectory(workingDirectory)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(outputStream))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(outputStream))
            .ExecuteAsync();
    }
}