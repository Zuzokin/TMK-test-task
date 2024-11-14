namespace PipeManager.Core.Contracts.Responses;

public record PackageResponse
{
    public Guid Id { get; init; } // Идентификатор пакета
    public string Number { get; init; } // Номер пакета
    public DateTime Date { get; init; } // Дата создания или регистрации пакета
    // public int PipeCount { get; init; } // Количество труб в пакете (если необходимо)
}