using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Wizard.Assets
{
    public class ResourceGroup: IAsset, IUniqueValidator
    {
        [Required, PropertyPath("global/resourceGroup")]
        public string Name { get; set; }

        [Required, PropertyPath("global/location")]
        public string Location { get; set; }

        public string Key { get; }
        public AssetType Type => AssetType.ResourceGroup;
        public IList<Dependency> Dependencies => new List<Dependency>()
        {
            new Dependency(AssetType.Subscription)
        };

        public int SortOrder { get; }

        public ResourceGroup()
        {
            Key = Guid.NewGuid().ToString();
        }

        public bool Validate()
        {
            return true;
        }

        public void WriteYaml(StreamWriter writer, int indent = 0)
        {
            var spaces = "".PadLeft(indent);
            writer.Write($"{spaces}resourceGroup: {Name}");
            writer.Write($"{spaces}location: {Location}");
        }
    }
}