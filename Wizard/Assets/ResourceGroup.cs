using System;
using System.Collections.Generic;

namespace Wizard.Assets
{
    public class ResourceGroup: IAzureComponent, IUniqueValidator
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public AzureComponentType Type => AzureComponentType.ResourceGroup;
        public IList<Type> Dependencies => new List<Type>(){typeof(AzureSubscription)};

        public bool Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}