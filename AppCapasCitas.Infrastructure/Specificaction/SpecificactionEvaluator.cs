using System;
using AppCapasCitas.Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace AppCapasCitas.Infrastructure.Specificaction;


public class SpecificactionEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T>  inputQuery ,ISpecification<T> spec) //Spec contiene las especificaciones de cliteria e include
    {
        //
        if (spec.Criteria is not null)
        {
            inputQuery = inputQuery.Where(spec.Criteria);
        }
        if (spec.OrderBy  is not null)
        {
             inputQuery = inputQuery.OrderBy(spec.OrderBy);
        }
        
        if (spec.OrderByDescending  is not null)
        {
             inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        }

        if(spec.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
        }

        // aplicar varios .Include() a una consulta IQueryable.
        inputQuery = spec.Includes!.Aggregate(inputQuery,(currrent, include) => currrent.Include(include));
        return inputQuery;
    }

    //

 }