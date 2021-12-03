using System.Text.Json.Serialization;

namespace dotnet_rpg.Models
{
    //Enum For Creating Character Role
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        knight,
        Mage,
        Cleric
    }
}
