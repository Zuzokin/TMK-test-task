using System;

namespace PipeManager.Core.Models
{
    public class SteelGrade
    {
        public const int MAX_NAME_LENGTH = 100;
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public SteelGrade() { }
        private SteelGrade(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Result<SteelGrade> Create(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                return Result<SteelGrade>.Failure("Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<SteelGrade>.Failure("Name cannot be empty.");
            }

            if (name.Length > MAX_NAME_LENGTH)
            {
                return Result<SteelGrade>.Failure($"Name cannot exceed {MAX_NAME_LENGTH} characters.");
            }

            var steelGrade = new SteelGrade(id, name);
            return Result<SteelGrade>.Success(steelGrade);
        }
    }
}