using System.ComponentModel.DataAnnotations;

namespace Wizard.Assets
{
    public class Global
    {
        [Required, MinLength(2), MaxLength(20)]
        public string ProductName { get; set; }
        [Required]
        public string SubscriptionName { get; set; }
        [Required, RegularExpression("westus|westus2")]
        public string Location { get; set; }
        [Required]
        public string ResourceGroup { get; set; }
        [RegularExpression("dev|int|prod")]
        public string EnvName { get; set; }
        public string SpaceName { get; set; }
    }
}