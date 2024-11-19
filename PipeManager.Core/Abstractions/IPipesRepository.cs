using PipeManager.Core.Models;

namespace PipeManager.Core.Abstractions
{
    public interface IPipesRepository
    {
        /// <summary>
        /// Создаёт новую трубу.
        /// </summary>
        /// <param name="pipe">Объект трубы.</param>
        /// <returns>Идентификатор созданной трубы.</returns>
        Task<Guid> Create(Pipe pipe);

        /// <summary>
        /// Удаляет трубу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор трубы.</param>
        /// <returns>Идентификатор удалённой трубы.</returns>
        Task<Guid> Delete(Guid id);

        /// <summary>
        /// Получает список всех труб.
        /// </summary>
        /// <returns>Список всех труб.</returns>
        Task<List<Pipe>> Get();

        /// <summary>
        /// Получает трубу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор трубы.</param>
        /// <returns>Объект трубы.</returns>
        Task<Pipe> GetById(Guid id);

        /// <summary>
        /// Обновляет свойства трубы.
        /// </summary>
        /// <param name="id">Идентификатор трубы.</param>
        /// <param name="label">Метка трубы.</param>
        /// <param name="isGood">Качество трубы.</param>
        /// <param name="diameter">Диаметр трубы.</param>
        /// <param name="length">Длина трубы.</param>
        /// <param name="weight">Вес трубы.</param>
        /// <param name="steelGradeId">Идентификатор марки стали.</param>
        /// <param name="packageId">Идентификатор пакета.</param>
        /// <returns>Идентификатор обновлённой трубы.</returns>
        Task<Guid> Update(Guid id, string label, bool isGood, decimal diameter, decimal length, decimal weight, Guid? steelGradeId, Guid? packageId);

        /// <summary>
        /// Проверяет существование марки стали по идентификатору.
        /// </summary>
        /// <param name="steelGradeId">Идентификатор марки стали.</param>
        /// <returns>True, если марка стали существует; иначе false.</returns>
        Task<bool> SteelGradeExists(Guid steelGradeId);

        /// <summary>
        /// Получает отфильтрованный список труб.
        /// </summary>
        /// <param name="filter">Фильтры для труб.</param>
        /// <returns>Список труб, соответствующих фильтру.</returns>
        Task<List<Pipe>> GetFilteredPipes(PipeFilter filter);

        /// <summary>
        /// Получает статистику по трубам.
        /// </summary>
        /// <returns>Статистика по трубам.</returns>
        Task<PipeStatistics> GetStatistics();

        /// <summary>
        /// Проверяет, находится ли труба в пакете.
        /// </summary>
        /// <param name="pipeId">Идентификатор трубы.</param>
        /// <returns>True, если труба находится в пакете; иначе false.</returns>
        Task<bool> IsPipeInPackage(Guid pipeId);

        Task<List<Pipe>> GetPipesInPackage(Guid packageId);
        
        Task<bool> PackageExists(Guid packageId);
    }
}
