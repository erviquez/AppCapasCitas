using System.Data;
using AppCapasCitas.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Infrastructure.Repositories;
using AppCapasCitas.Infrastructure.Data;

namespace AppCapasCitas.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InfrastructureDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(InfrastructureDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesWithResultAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No hay ninguna transacción activa para confirmar");
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No hay ninguna transacción activa para revertir");
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null!;
                }
            }
        }

        public async Task ExecuteInTransactionAsync(
            Func<Task> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transaction = await BeginTransactionAsync(isolationLevel, cancellationToken))
            {
                try
                {
                    await action();
                    await CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }
        }

        public IAsyncRepository<TEntity> GetRepository<TEntity>() where TEntity : EntidadBaseAuditoria
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new RepositoryBase<TEntity>(_context);
            }

            return (IAsyncRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currentTransaction?.Dispose();
                _context.Dispose();
            }
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                }
                await _context.DisposeAsync();
            }
        }
    }
}