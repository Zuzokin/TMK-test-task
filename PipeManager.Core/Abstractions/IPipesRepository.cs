using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions
{
    public interface IPipesRepository
    {
        Task<Guid> Create(Pipe pipe);
        Task<Guid> Delete(Guid id);
        Task<List<Pipe>> Get();
        Task<Pipe> GetById(Guid id);
        Task<Guid> Update(Guid id, string label, bool isGood, decimal diameter, decimal length, decimal weight, Guid? steelGradeId, Guid? packageId);
        Task<bool> SteelGradeExists(Guid steelGradeId);
        #region Возможные методы для тз
/// <summary>
        /// Возвращает общие статистические данные по трубам, включая общее количество,
        /// количество годных и с браком, а также общий вес.
        /// </summary>
        Task<PipeStatistics> GetStatistics();

        /// <summary>
        /// Фильтрует трубы по заданным критериям, таким как качество, марка стали, диапазоны размеров и веса.
        /// </summary>
        /// <param name="isGood">Фильтр по качеству (годная или брак).</param>
        /// <param name="steelGradeId">Фильтр по идентификатору марки стали.</param>
        /// <param name="minDiameter">Минимальное значение диаметра трубы.</param>
        /// <param name="maxDiameter">Максимальное значение диаметра трубы.</param>
        /// <param name="minWeight">Минимальное значение веса трубы.</param>
        /// <param name="maxWeight">Максимальное значение веса трубы.</param>
        Task<List<Pipe>> FilterPipes(bool? isGood = null, Guid? steelGradeId = null, decimal? minDiameter = null, decimal? maxDiameter = null, decimal? minWeight = null, decimal? maxWeight = null);

        /// <summary>
        /// Проверяет, привязана ли труба к пакету, для ограничения возможности её редактирования или удаления.
        /// </summary>
        Task<bool> IsPipeInPackage(Guid pipeId);

        /// <summary>
        /// Добавляет трубу в указанный пакет.
        /// </summary>
        Task AddPipeToPackage(Guid pipeId, Guid packageId);

        /// <summary>
        /// Удаляет трубу из пакета, оставляя трубу, но убирая её привязку к пакету.
        /// </summary>
        Task RemovePipeFromPackage(Guid pipeId);

        /// <summary>
        /// Возвращает список труб, которые находятся в указанном пакете.
        /// </summary>
        Task<List<Pipe>> GetPipesInPackage(Guid packageId);

        /// <summary>
        /// Возвращает количество труб определённого качества (годные или брак).
        /// </summary>
        /// <param name="isGood">Фильтр по качеству (true для годных, false для брака).</param>
        Task<int> CountPipesByQuality(bool isGood);
        
        #endregion
    }
    
}