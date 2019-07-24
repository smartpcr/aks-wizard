using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Wizard.Assets;

namespace Wizard
{
    public class InfraBuilder
    {
        private readonly AssetManager _assetManager;
        private readonly ILogger<InfraBuilder> _logger;

        public InfraBuilder(AssetManager assetManager, ILogger<InfraBuilder> logger)
        {
            _assetManager = assetManager;
            _logger = logger;
        }

        public void Build(string manifestFile, string outputFolder)
        {
            IList<IAsset> unresolvedAssets = null;
            var assets = AssetReader.Read(manifestFile);
            if (assets?.Any() == true)
            {
                foreach (var asset in assets)
                {
                    _assetManager.Add(asset);
                    unresolvedAssets = _assetManager.EvaluateUnfulfilledComponents(asset);
                }
            }

            if (unresolvedAssets?.Any() == true)
            {
                foreach (var asset in unresolvedAssets)
                {
                    _logger.LogWarning($"Unable to resolve {asset.Type}");
                    var missingDependencyTypes = asset.Dependencies.Where(d =>
                            string.IsNullOrEmpty(d.Key))
                        .Select(d => d.Type).ToList();
                    _logger.LogWarning($"Unresolved components: {string.Join(",", missingDependencyTypes)}");
                }
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }


        }
    }
}