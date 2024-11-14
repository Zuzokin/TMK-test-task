namespace PipeManager.DataAccess.Entites
{
    public class PackageEntity
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        // Связь с PipeEntity
        public ICollection<PipeEntity> Pipes { get; set; }  // Коллекция труб, связанных с этим пакетом
    }
}