using AutoMapper;
using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Stagazer.Extensions.AutoMapper.Context.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapperContext<T>(this IServiceCollection services) where T : notnull
        {
            services.AddDecorator<IMapper, ContextMapper<T>>();
        }
    }
}
