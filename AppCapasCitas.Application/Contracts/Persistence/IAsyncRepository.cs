using System;
using System.Linq.Expressions;
using AppCapasCitas.Application.Specifications;

namespace AppCapasCitas.Application.Contracts.Persistence;


public interface IAsyncRepository<T> where T : class
{


    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
                                     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy 
                                     , CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                   Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
                                   string? includeString,//Enviar una consición que involucre a otra entidad
                                   bool disableTracking = true//Si se quiere almacenar la información en memoria
                                    , CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                   Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy = null,
                                   List<Expression<Func<T, object>>>? includes = null,//Enviar una lista de entidades involucradas en el query
                                   bool disableTracking = true
                                   , CancellationToken cancellationToken = default);

    //paginacion
    // Task<IReadOnlyList<T>> GetPaginationAsync(Expression<Func<T, bool>> filter = null,
    //                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    //                             List<Expression<Func<T, object>>> includes = null,
    //                             int? pageSize = null,
    //                             int? skip = null);

    //Devuelve un solo objeto
    Task<T> GetEntityAsync(Expression<Func<T, bool>>? predicate,
                          List<Expression<Func<T, object>>>? includes = null,//Lista de relacion con otras entidades 
                          bool disableTracking = true
                          , CancellationToken cancellationToken = default);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);


    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);


    void AddEntity(T entity, CancellationToken cancellationToken = default);

    void UpdateEntity(T entity, CancellationToken cancellationToken = default);

    void DeleteEntity(T entity, CancellationToken cancellationToken = default);

    //Agregar un conjunto de records
    void AddRange(List<T> entities, CancellationToken cancellationToken = default);
    //Eliminar un conjunto de records 
    void DeleteRange(IReadOnlyList<T> entities, CancellationToken cancellationToken = default);


    Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);



    //Representan en Specification de paginación
    //ISpecification: Es un artefacto complejo que contiene la lógica para poder ordenar, para paginación, para hacer filtro, etc 
      Task<T> GetByIdWithSpec(ISpecification<T> spec);
      Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
      Task<int> CountAsyncWithSpec(ISpecification<T> spec);

}