using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Wizard.Assets
{
    public class AzureSubscription: BaseAsset
    {
        [PropertyPath("global/subscriptionId")]
        public Guid SubscriptionId { get; set; }


        [Required, PropertyPath("global/subscriptionName")]
        public string SubscriptionName { get; set; }

        [PropertyPath("global/tenantId")]
        public Guid TenantId { get; set; }

        public override AssetType Type => AssetType.Subscription;

        public override IList<Dependency> Dependencies => new List<Dependency>();


        public override int SortOrder { get; }

        public override void WriteYaml(StreamWriter writer, AssetManager assetManager, ILoggerFactory loggerFactory, int indent = 0)
        {
            var spaces = "".PadLeft(indent);
            writer.Write($"{spaces}subscriptionName: {SubscriptionName}");
            writer.Write($"{spaces}subscriptionId: {SubscriptionId}");
        }
    }
}