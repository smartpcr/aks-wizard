using System.ComponentModel.DataAnnotations;

namespace Wizard.Assets.CosmosDB
{
    public class CosmosDbAccount
    {
        [MaxLength(25), MinLength(3)]
        public string Name { get; set; }

    }
}