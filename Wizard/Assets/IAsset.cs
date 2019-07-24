using System;
using System.Collections.Generic;

namespace Wizard.Assets
{
    public interface IAsset
    {
        string Key { get; }
        AssetType Type { get; }
        IList<Dependency> Dependencies { get; }
        int SortOrder { get; }
    }

    public enum AssetType
    {
        Global,
        Subscription,
        ResourceGroup,
        KeyVault,
        CosmosDb,
        ContainerRegistry,
        KubernetesCluster,
        Dns,
        Prodct,
        Web,
        Api,
        Job,
        ExternalService,
    }
}