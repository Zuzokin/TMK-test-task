namespace PipeManager.Core.Models
{
    public class Package
    {
        public const int MAX_NUMBER_LENGTH = 50;
        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public DateTime Date { get; private set; }

        private Package(Guid id, string number, DateTime date)
        {
            Id = id;
            Number = number;
            Date = date.ToUniversalTime(); // Преобразование даты в формат UTC
        }
        
        public Package() {}

        public static Result<Package> Create(Guid id, string number, DateTime date)
        {
            if (id == Guid.Empty)
            {
                return Result<Package>.Failure("Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(number))
            {
                return Result<Package>.Failure("Number cannot be empty.");
            }

            if (number.Length > MAX_NUMBER_LENGTH)
            {
                return Result<Package>.Failure($"Number cannot exceed {MAX_NUMBER_LENGTH} characters.");
            }

            if (date == default)
            {
                return Result<Package>.Failure("Date must be a valid date.");
            }
            
            var package = new Package(id, number, date);
            return Result<Package>.Success(package);
        }
    }
}