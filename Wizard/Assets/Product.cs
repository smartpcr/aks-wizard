using System.Collections.Generic;
using System.IO;

namespace Wizard.Assets
{
    public class Product : IAsset
    {
        [PropertyPath("global/productName")]
        public string Name { get; set; }

        public string Key { get; }
        public AssetType Type => AssetType.Prodct;
        public IList<Dependency> Dependencies => new List<Dependency>();

        public int SortOrder { get; }

        public void WriteYaml(StreamWriter writer, int indent = 0)
        {
            var spaces = "".PadLeft(indent);
            writer.Write($"{spaces}productName: " + Name);
        }
    }
}