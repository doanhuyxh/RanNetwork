using System.ComponentModel.DataAnnotations;

namespace RanNetwork.Models
{
    public class WebConfig
    {
        [Key]
        public string KeyName { get; set; }
        public string value { get; set; }
    }
}
