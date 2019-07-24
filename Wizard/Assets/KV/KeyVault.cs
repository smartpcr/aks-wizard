using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Wizard.Assets.KV
{
    [ObjectPath("kv")]
    public class KeyVault : IAsset, IUniqueValidator
    {
        [MaxLength(25), MinLength(3)]
        public string Name { get; set; }

        public string Key { get; }

        public AssetType Type => AssetType.KeyVault;

        public IList<Dependency> Dependencies => new List<Dependency>()
        {
            new Dependency(AssetType.Subscription),
            new Dependency(AssetType.ResourceGroup)
        };

        public int SortOrder { get; }
        public void WriteYaml(StreamWriter writer, int indent = 0)
        {
            throw new NotImplementedException();
        }

        public KeyVault()
        {
            Key = Guid.NewGuid().ToString();
        }

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