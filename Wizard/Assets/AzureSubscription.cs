using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizard.Assets
{
    public class AzureSubscription: IAzureComponent
    {
        public Guid Guid { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid TenantId { get; set; }

        public AzureComponentType Type => AzureComponentType.Subscription;
        public IList<Type> Dependencies => new List<Type>();
    }
}