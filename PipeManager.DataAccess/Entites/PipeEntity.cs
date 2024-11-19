namespace PipeManager.DataAccess.Entites
{
    public class PipeEntity
    {
        public Guid Id { get; set; }

        public string Label { get; set; }

        public bool IsGood { get; set; }

        public Guid SteelGradeId { get; set; }
        public SteelGradeEntity SteelGrade { get; set; }  // Навигационное свойство для связи с SteelGrade

        public decimal Diameter { get; set; }

        public decimal Length { get; set; }

        public decimal Weight { get; set; }

        public Guid? PackageId { get; set; }
        public PackageEntity? Package { get; set; }  // Навигационное свойство для связи с Package
    }
}