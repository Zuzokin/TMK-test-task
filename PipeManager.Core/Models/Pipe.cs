using System.Reflection;

namespace PipeManager.Core.Models;

public class Pipe
{
    public const int MAX_NUMBER_LENGTH  = 100;
    
    public Guid Id { get; set; }
    public string Label { get; set; }
    public bool IsGood { get; set; }
    public Guid SteelGradeId { get; set; }
    public SteelGrade? SteelGrade { get; set; }
    public decimal Diameter { get; set; }
    public decimal Length { get; set; }
    public decimal Weight { get; set; }
    public Guid? PackageId { get; set; }
    public Package? Package { get; set; }

    private Pipe(Guid id, string label, bool isGood, Guid steelGradeId, decimal diameter, decimal length, decimal weight)
    {
        Id = id;
        Label = label;
        IsGood = isGood;
        SteelGradeId = steelGradeId;
        Diameter = diameter;
        Length = length;
        Weight = weight;
    }
    
    public Pipe() { }
    public static Result<Pipe> Create(Guid id, string label, bool isGood, Guid steelGradeId, decimal diameter, decimal length, decimal weight)
    {
        if (string.IsNullOrEmpty(label) || label.Length > Pipe.MAX_NUMBER_LENGTH)
        {
            return Result<Pipe>.Failure($"Label can't be empty or longer than {Pipe.MAX_NUMBER_LENGTH} symbols");
        }

        var pipe = new Pipe(id, label, isGood, steelGradeId, diameter, length, weight);
        return Result<Pipe>.Success(pipe);
    }

}