using System.Data;
using AppCapasCitas.Domain.Models.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppCapasCitas.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Guarda los cambios en la base de datos sin confirmar la transacción
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>Número de entidades afectadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Inicia una nueva transacción con el nivel de aislamiento especificado
    /// </summary>
    /// <param name="isolationLevel">Nivel de aislamiento de la transacción</param>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>Transacción iniciada</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Revierte la transacción actual
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    // Repositorios
    /// <summary>
    /// Obtiene el repositorio para la entidad especificada
    /// </summary>
    /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
    /// <returns>Repositorio para la entidad</returns>
    IAsyncRepository<TEntity> GetRepository<TEntity>() where TEntity : EntidadBaseAuditoria;


    /// <summary>
    /// Guarda los cambios en la base de datos y devuelve un valor booleano indicando si se realizaron cambios
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>True si se realizaron cambios, de lo contrario false</returns>
    /// 
    Task<bool> SaveChangesWithResultAsync(CancellationToken cancellationToken = default);
    Task ExecuteInTransactionAsync(Func<Task> action, 
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);
        
    // Propiedades para ver estado
    bool HasActiveTransaction { get; }
}