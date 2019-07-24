using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Wizard.Assets
{
    public class AssetManager
    {
        private readonly ILogger<AssetManager> _logger;
        private readonly Dictionary<string, IAsset> _components = new Dictionary<string, IAsset>();

        public AssetManager(ILogger<AssetManager> logger)
        {
            _logger = logger;
        }

        public void Add(IAsset component)
        {
            if (_components.ContainsKey(component.Key))
            {
                throw new Exception("Component already added");
            }

            _components.Add(component.Key, component);
        }

        public IList<IAsset> GetFulfilledComponents()
        {
            var fulfilledComponents = _components.Values.Where(c => c.Dependencies.All(d => d.Key != null))
                .OrderBy(c => c.SortOrder).ToList();
            return fulfilledComponents;
        }

        public IList<IAsset> EvaluateUnfulfilledComponents(IAsset component)
        {
            var unfulfilledComponents = _components.Values.Where(c =>
                    c.Dependencies.Any(d => string.IsNullOrEmpty(d.Key) && d.Type == component.Type))
                .OrderBy(c => c.SortOrder).ToList();

            foreach (var unfulfilledComponent in unfulfilledComponents)
            {
                var dependency = unfulfilledComponent.Dependencies.FirstOrDefault(d => d.Type == component.Type);
                if (dependency != null && (!dependency.CanHaveMany && !dependency.IsOptional))
                {
                    dependency.Key = component.Key;
                }
            }

            var notFulfilledComponents = unfulfilledComponents
                .Where(c => c.Dependencies.Any(d => string.IsNullOrEmpty(d.Key) && d.Type == component.Type))
                .OrderBy(c => c.SortOrder).ToList();
            return notFulfilledComponents;
        }

        public IAsset FindResolved(Dependency dependency)
        {
            var asset = GetFulfilledComponents().FirstOrDefault(c => c.Key == dependency.Key);

            if (asset == null)
            {
                _logger.LogError($"Invalid manifest: unable to find {dependency.Type} by key {dependency.Key}");
            }

            return asset;
        }

        public IList<IAsset> GetAllAssetsWithObjPath()
        {
            var assetsTypeWithObjPaths = AssetReader.AssetsWithObjPath;
            var sortedComponents = _components.Where(c =>
                    assetsTypeWithObjPaths.ContainsKey(c.GetType()))
                .Select(c => c.Value)
                .OrderBy(c => c.SortOrder)
                .ToList();
            return sortedComponents;
        }
    }
}