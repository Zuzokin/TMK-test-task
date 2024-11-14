namespace PipeManager.Core.Contracts.Requests;

public record PipeRequest
{
    public string Label { get; init; } // Название или номер трубы
    public bool IsGood { get; init; } // Качество (годная или брак)
    public decimal Diameter { get; init; } // Диаметр трубы
    public decimal Length { get; init; } // Длина трубы
    public decimal Weight { get; init; } // Вес трубы

    // Связанные идентификаторы
    public Guid SteelGradeId { get; init; } // Идентификатор марки стали
    public Guid? PackageId { get; init; } // Идентификатор пакета (необязательно, если труба не в пакете)
}