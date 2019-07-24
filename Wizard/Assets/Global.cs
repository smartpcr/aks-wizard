using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizard.Assets
{
    public class Global : IAsset
    {
        [RegularExpression("dev|int|prod"), PropertyPath("global/envName")]
        public string EnvName { get; set; }

        [PropertyPath("global/spaceName")]
        public string SpaceName { get; set; }

        public string Key { get; }

        public AssetType Type => AssetType.Global;

        public IList<Dependency> Dependencies => new List<Dependency>()
        {
            new Dependency(AssetType.Subscription),
            new Dependency(AssetType.ResourceGroup),
            new Dependency(AssetType.Prodct)
        };

        public int SortOrder { get; }
    }
}