namespace PipeManager.Core.Models;

public class PipeStatistics
{
    public Guid Id { get; set; }
    public int TotalCount { get; set; }
    public int GoodCount { get; set; }
    public int DefectiveCount { get; set; }
    public decimal TotalWeight { get; set; }

    public PipeStatistics(int totalCount, int goodCount, int defectiveCount, decimal totalWeight)
    {
        Id = Guid.NewGuid();
        TotalCount = totalCount;
        GoodCount = goodCount;
        DefectiveCount = defectiveCount;
        TotalWeight = totalWeight;
    }

    public PipeStatistics() { }
}