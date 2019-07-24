using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Wizard.Assets
{
    public class AzureSubscription: IAsset
    {
        [PropertyPath("global/subscriptionId")]
        public Guid SubscriptionId { get; set; }


        [Required, PropertyPath("global/subscriptionName")]
        public string SubscriptionName { get; set; }

        [PropertyPath("global/tenantId")]
        public Guid TenantId { get; set; }

        [JsonIgnore]
        public string Key => SubscriptionId.ToString();

        public AssetType Type => AssetType.Subscription;

        public IList<Dependency> Dependencies => new List<Dependency>();


        public int SortOrder { get; }
    }
}