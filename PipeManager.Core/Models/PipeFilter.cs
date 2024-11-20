namespace PipeManager.Core.Models;

public class PipeFilter
{
    public Guid? SteelGradeId { get; set; }
    public bool? IsGood { get; set; }
    public decimal? MinWeight { get; set; }
    public decimal? MaxWeight { get; set; }
    public decimal? MinLength { get; set; }
    public decimal? MaxLength { get; set; }
    public decimal? MinDiameter { get; set; }
    public decimal? MaxDiameter { get; set; }
    public bool? NotInPackage { get; set; } // Исключает трубы из пакетов
}


