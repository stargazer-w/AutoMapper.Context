using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Internal;

namespace Stagazer.Extensions.AutoMapper.Context
{
    public class ContextMapper<T> : IMapper where T : notnull
    {
        readonly IMapper backingMapper;
        readonly T context;
        public ContextMapper(IMapper backingMapper, T context)
        {
            this.context = context;
            this.backingMapper = backingMapper;
        }

        public IConfigurationProvider ConfigurationProvider => backingMapper.ConfigurationProvider;

        public Func<Type, object> ServiceCtor => backingMapper.ServiceCtor;

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
        {
            return backingMapper.Map(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return backingMapper.Map(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return backingMapper.Map(source, destination, opts);
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            return backingMapper.Map(source, sourceType, destinationType, opts);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            return backingMapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public TDestination Map<TDestination>(object source)
        {
            return backingMapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return backingMapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return backingMapper.Map(source, destination);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return backingMapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return backingMapper.Map(source, destination, sourceType, destinationType);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object? parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return backingMapper.ProjectTo(source, parameters ?? new { context }, membersToExpand);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object>? parameters, params string[] membersToExpand)
        {
            return backingMapper.ProjectTo<TDestination>(source, MergeParameters(parameters), membersToExpand);
        }

        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object>? parameters = null, params string[] membersToExpand)
        {
            return backingMapper.ProjectTo(source, destinationType, MergeParameters(parameters), membersToExpand);
        }

        private IDictionary<string, object> MergeParameters(IDictionary<string, object>? parameters)
        {
            switch(parameters)
            {
                case null:
                    parameters = new Dictionary<string, object>() { ["context"] = context };
                    break;
                case { } when !parameters.ContainsKey("context"):
                    parameters.Add("context", context);
                    break;
                default:
                    break;
            }
            return parameters;
        }
    }
}
