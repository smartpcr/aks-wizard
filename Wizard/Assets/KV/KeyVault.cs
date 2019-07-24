using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizard.Assets.KV
{
    public class KeyVault : IAzureComponent, IUniqueValidator
    {
        public string ResourceGroup { get; set; }
        public string Location { get; set; }
        [MaxLength(25), MinLength(3)]
        public string Name { get; set; }

        public AzureComponentType Type => AzureComponentType.KeyVault;
        public IList<Type> Dependencies => new List<Type>()
        {
            typeof(AzureSubscription),
            typeof(ResourceGroup)
        };

        /// <summary>
        /// make sure vault name is unique globally
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Validate()
        {
            // TODO: implement using rest api
            return true;
        }
    }
}