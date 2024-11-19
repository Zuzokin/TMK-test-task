using System;
using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record PackageRequest(
        [Required(ErrorMessage = "Number is required.")]
        [StringLength(100, ErrorMessage = "Number cannot exceed 100 characters.")]
        string Number, // Номер пакета

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        DateTime Date // Дата создания или регистрации пакета
    );
}