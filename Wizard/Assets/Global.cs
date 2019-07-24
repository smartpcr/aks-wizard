using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Wizard.Assets
{
    public class Global : IAsset
    {
        private readonly AssetManager _assetManager;
        private readonly ILogger<Global> _logger;

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

        public Global(AssetManager assetManager, ILogger<Global> logger)
        {
            _assetManager = assetManager;
            _logger = logger;
        }

        public void WriteYaml(StreamWriter writer, int indent = 0)
        {
            var spaces = "".PadLeft(indent);
            writer.Write($"{spaces}global:");
            foreach (var dependency in Dependencies)
            {
                if (string.IsNullOrEmpty(dependency.Key))
                {
                    _logger.LogError($"Missing dependent definition: {dependency.Type}");
                }
                else
                {
                    var dependentAsset = _assetManager.FindResolved(dependency);
                    dependentAsset?.WriteYaml(writer, indent + 2);
                }
            }

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