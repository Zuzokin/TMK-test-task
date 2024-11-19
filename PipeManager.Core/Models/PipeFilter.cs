namespace PipeManager.Core.Models;

public class PipeFilter
{
    public Guid? SteelGradeId { get; set; }
    public bool? IsGood { get; set; }
    public decimal? MinWeight { get; set; }
    public decimal? MaxWeight { get; set; }
    public Guid? PackageId { get; set; }
}
