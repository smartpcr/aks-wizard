using System;
using System.Collections.Generic;

namespace Wizard.Assets
{
    public interface IAzureComponent
    {
        AzureComponentType Type { get; }
        IList<Type> Dependencies { get; }
    }

    public enum AzureComponentType
    {
        Subscription,
        ResourceGroup,
        KeyVault,
        CosmosDb,
        ContainerRegistry,
        KubernetesCluster,
    }
}