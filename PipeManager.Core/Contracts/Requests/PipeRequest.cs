using System;
using System.ComponentModel.DataAnnotations;

namespace PipeManager.Core.Contracts.Requests
{
    public record PipeRequest(
        [Required(ErrorMessage = "Label is required.")]
        [StringLength(100, ErrorMessage = "Label cannot exceed 100 characters.")]
        string Label, // Название или номер трубы

        [Required(ErrorMessage = "Quality (IsGood) is required.")]
        bool IsGood, // Качество (годная или брак)

        [Range(1, double.MaxValue, ErrorMessage = "Diameter must be greater than 0.")]
        decimal Diameter, // Диаметр трубы

        [Range(1, double.MaxValue, ErrorMessage = "Length must be greater than 0.")]
        decimal Length, // Длина трубы

        [Range(1, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        decimal Weight, // Вес трубы

        [Required(ErrorMessage = "SteelGradeId is required.")]
        Guid SteelGradeId, // Идентификатор марки стали

        Guid? PackageId // Идентификатор пакета (необязательно, если труба не в пакете)
    );
}