namespace PipeManager.Core.Contracts.Requests;

public record PackageRequest
{
    public string Number { get; init; } // Номер пакета
    public DateTime Date { get; init; } // Дата создания или регистрации пакета
}