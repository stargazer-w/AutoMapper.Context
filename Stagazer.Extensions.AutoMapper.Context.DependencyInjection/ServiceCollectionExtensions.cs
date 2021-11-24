using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Stagazer.Extensions.AutoMapper.Context.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapperContext<T>(this IServiceCollection services) where T : notnull
        {
            ReplaceMapper<T>(services);
        }

        private static void ReplaceMapper<T>(IServiceCollection services) where T : notnull
        {
            var sd = services.First(x => x.ServiceType == typeof(IMapper));

            switch(sd)
            {
                case { ImplementationType: { } t }:
                    {
                        services.Add(new ServiceDescriptor(t, t, sd.Lifetime));
                        services.Replace(ServiceDescriptor.Scoped(typeof(IMapper), sp =>
                        {
                            var bkMapper = sp.GetRequiredService(t);
                            var context = sp.GetRequiredService<T>();
                            return new ContextMapper<T>((IMapper)bkMapper, context);
                        }));
                    }
                    break;
                case { ImplementationInstance: { } o }:
                    {
                        var t = o.GetType();
                        services.Add(new ServiceDescriptor(t, o));
                        services.Replace(ServiceDescriptor.Scoped(typeof(IMapper), sp =>
                        {
                            var bkMapper = sp.GetRequiredService(t);
                            var context = sp.GetRequiredService<T>();
                            return new ContextMapper<T>((IMapper)bkMapper, context);
                        }));
                    }
                    break;
                case { ImplementationFactory: { } f }:
                    {
                        services.Replace(ServiceDescriptor.Scoped(typeof(IMapper), sp =>
                        {
                            var bkMapper = f(sp);
                            var context = sp.GetRequiredService<T>();
                            return new ContextMapper<T>((IMapper)bkMapper, context);
                        }));
                    }
                    break;
            }
        }
    }
}
