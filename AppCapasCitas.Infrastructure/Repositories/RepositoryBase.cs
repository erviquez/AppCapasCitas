using System;
using System.Linq.Expressions;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications;
using AppCapasCitas.Infrastructure.Data;
using AppCapasCitas.Infrastructure.Specificaction;
using Microsoft.EntityFrameworkCore;

namespace AppCapasCitas.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : class
{
    protected readonly InfrastructureDbContext _context;
    public RepositoryBase(InfrastructureDbContext context)
    {
        _context = context;
    }
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    //Solo inserta el record en memoria temporal
    public void AddEntity(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
    }
    public void AddRange(List<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().AddRange(entities);
    }
    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    public void DeleteEntity(T entity, CancellationToken cancellationToken = default)
    {
        //Solo en memoria
        _context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IReadOnlyList<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().RemoveRange(entities);
    }
    public async Task<IReadOnlyList<T>> GetAllAsync( CancellationToken cancellationToken)
    {
        //Devuelve todos los records de una entidad
        return await _context.Set<T>().ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAsync(
                                            Expression<Func<T, bool>>? predicate,
                                            Func<IQueryable<T>,
                                            IOrderedQueryable<T>>? orderBy,
                                            string? includeString,
                                            bool disableTracking = true,
                                            CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (!string.IsNullOrEmpty(includeString)) query = query.Include(includeString);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();

    }
    public async Task<IReadOnlyList<T>> GetAsync(
                                        Expression<Func<T, bool>>? predicate,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        CancellationToken cancellationToken = default
                                        )
    {
        //Realizar consulta a multiples entidades
        IQueryable<T> query = _context.Set<T>();
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAsync(
                                        Expression<Func<T, bool>>? predicate,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        List<Expression<Func<T, object>>>? includes = null,
                                        bool disableTracking = true,
                                        CancellationToken cancellationToken = default)
    {
        //Realizar consulta a multiples entidades
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }

    
    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {        
        return (await _context.Set<T>().FindAsync(id))!;
    }
    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {        
        return (await _context.Set<T>().FindAsync(id))!;
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> GetEntityAsync(
                                    Expression<Func<T, bool>>? predicate,
                                    List<Expression<Func<T, object>>>? includes = null,
                                    bool disableTracking = true,
                                    CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
        if (predicate != null) query = query.Where(predicate);
        return (await query.FirstOrDefaultAsync())!;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public void UpdateEntity(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    //
    public async Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> qry = _context.Set<T>();
        if (filter is not null)
        {
            qry = qry.Where(filter);
        }
        return await qry.AnyAsync();
    }

    //Specification
    public IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificactionEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(),spec);
    }
    public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
    {
        var result  = await ApplySpecification(spec).FirstOrDefaultAsync();
        return result!;
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsyncWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

}

