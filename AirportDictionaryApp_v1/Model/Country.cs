using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryApp_v1.Model
{
    // Country - класс, описывающий сущность страны
    [Index(nameof(Code), IsUnique = true)]
    public class Country
    {
        // поля
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;    // alpha3

        // связи
        public HashSet<Airport>? Airports { get; set; }

        public Country() { }

    }
}
