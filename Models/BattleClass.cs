using System.Text.Json.Serialization;

namespace fightAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BattleClass
    {
        Tank = 1,
        Tactical = 2,
        Healer = 3,
        Mage = 4
    }
}