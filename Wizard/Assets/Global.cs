using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Wizard.Assets
{
    public class Global : BaseAsset
    {
        [RegularExpression("dev|int|prod"), PropertyPath("global/envName")]
        public string EnvName { get; set; }

        [PropertyPath("global/spaceName")]
        public string SpaceName { get; set; }


        public override AssetType Type => AssetType.Global;

        public override IList<Dependency> Dependencies => new List<Dependency>()
        {
            new Dependency(AssetType.Subscription),
            new Dependency(AssetType.ResourceGroup),
            new Dependency(AssetType.Prodct)
        };

        public override int SortOrder { get; } = 1;

        public override void WriteYaml(StreamWriter writer, AssetManager assetManager, ILoggerFactory loggerFactory, int indent = 0)
        {
            var spaces = "".PadLeft(indent);
            writer.Write($"{spaces}global:");
            WriteDependencies(writer, assetManager, loggerFactory, indent + 2);

            spaces = "".PadLeft(indent+2);
            if (!string.IsNullOrWhiteSpace(EnvName))
            {
                writer.Write($"{spaces}envName: {EnvName}");
            }

            if (!string.IsNullOrWhiteSpace(SpaceName))
            {
                writer.Write($"{spaces}spaceName: {SpaceName}");
            }
        }
    }
}