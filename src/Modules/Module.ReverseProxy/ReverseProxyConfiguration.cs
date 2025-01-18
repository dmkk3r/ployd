// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Yarp.ReverseProxy.Configuration;

namespace Module.ReverseProxy;

public class ReverseProxyConfiguration
{
    public Guid Id { get; set; }
    public IReadOnlyList<ClusterConfig> Clusters { get; set; } = Array.Empty<ClusterConfig>();
    public IReadOnlyList<RouteConfig> Routes { get; set; } = Array.Empty<RouteConfig>();
}
