using System.Collections.Generic;
using System.IO;

namespace Wizard.Assets
{
    public interface IAsset
    {
        string Key { get; }
        AssetType Type { get; }
        IList<Dependency> Dependencies { get; }
        int SortOrder { get; }

        void WriteYaml(StreamWriter writer, int indent = 0);
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