using System.Text.Json.Serialization;

namespace Ployd.Core.Models.Deployments.Parameter;

[JsonDerivedType(typeof(DockerComposeParameter), typeDiscriminator: "dockercompose")]
[JsonDerivedType(typeof(DockerfileParameter), typeDiscriminator: "dockerfile")]
[JsonDerivedType(typeof(GithubParameter), typeDiscriminator: "github")]
[JsonDerivedType(typeof(DeploymentParameter), typeDiscriminator: "base")]
public class DeploymentParameter { }