namespace PipeManager.Core.Contracts.Responses;

public record PipeResponse
{
    public Guid Id { get; init; }
    public string Label { get; init; }
    public bool IsGood { get; init; }
    public decimal Diameter { get; init; }
    public decimal Length { get; init; }
    public decimal Weight { get; init; }
    public string SteelGradeName { get; init; } // Для удобства, если имя марки стали нужно на фронте
    public string PackageNumber { get; init; } // Можно добавить номер пакета, если он нужен на фронте
}