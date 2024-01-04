using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TryAutoZone.Models
{

    public enum CarMake
    {
        Toyota
    }

    public enum GearboxType
    {
        Automatyczna, Manualna
    }

    public enum EngineType
    {
        Benzyna, Diesel, Elektryczny, Hybryda, Wodorowy
    }

    public class Car
    {
        public int Id { get; set; }
        public CarMake Make { get; set; }
        public string Model { get; set; }

        [Range(1980, 2024, ErrorMessage = "Rok musi być większy niż 1980 i nie większy niż 2024.")]
        public int Year { get; set; }

        [Range(0, 2000, ErrorMessage = "Moc musi być w zakresie od 0 do 2000 KM.")]
        public int HorsePower { get; set; } 

        [Range(0, 10000, ErrorMessage = "Pojemność silnika musi być w zakresie od 0 do 10000 cm³.")]
        public int EngineCapacity { get; set; } 

        public EngineType EngineType { get; set; }

        public GearboxType Gearbox { get; set; }

        [Range(0, 1000, ErrorMessage = "Emisja CO2 musi być w zakresie od 0 do 1000 g/km.")]
        public int CO2Emission { get; set; } 

        [NotMapped]
        public string FuelConsumptionString { get; set; } 

        [Range(0, 100, ErrorMessage = "Zużycie paliwa musi być w zakresie od 0 do 100 l/100km.")]
        public double FuelConsumption { get; set; }
        public bool IsReserved { get; set; }
        public string? Description { get; set; } 
        public string? ImageUrl { get; set; } 
        public string? VideoUrl { get; set; } 
    }
}
