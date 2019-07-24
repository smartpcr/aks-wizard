using System;

namespace Wizard.Assets
{
    public class Dependency
    {
        public AssetType Type { get; }
        public string Id { get; set; }
        public bool IsOptional { get; set; } = false;
        public bool CanHaveMany { get; set; } = false;

        public Dependency(AssetType type, string id = null)
        {
            Type = type;
            Id = id;
        }
    }
}