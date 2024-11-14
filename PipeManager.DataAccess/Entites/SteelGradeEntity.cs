namespace PipeManager.DataAccess.Entites
{
    public class SteelGradeEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Связь с PipeEntity
        public ICollection<PipeEntity> Pipes { get; set; }  // Коллекция труб, связанных с этой маркой стали
    }
}