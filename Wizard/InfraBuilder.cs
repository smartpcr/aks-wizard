using Wizard.Assets;

namespace Wizard
{
    public class InfraBuilder
    {
        private readonly AssetManager _assetManager;

        public InfraBuilder(AssetManager assetManager)
        {
            _assetManager = assetManager;
        }
    }
}