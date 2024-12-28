using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Metrics;

namespace AirportDictionaryApp_v1.Model
{
    // Airport - класс, описывающий сущность аэропорта
    [Index(nameof(Code), IsUnique = true)]
    public class Airport
    {
        // поя
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int OpeningYear { get; set; }
        public int RunwayCount { get; set; }
        public long AnnualPassengerTraffic { get; set; }
        public string Location { get; set; } = string.Empty;

        // связи
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
        public HashSet<Company>? Companies { get; set; }

        public Airport() { }
    }
}
