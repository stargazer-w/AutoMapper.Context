using Xunit;
using Stagazer.Extensions.AutoMapper.Context.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System.Linq;

namespace Stagazer.Extensions.AutoMapper.Context.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        public class Context
        {
            public string Bar { get; set; }
        }

        public class FooDto
        {
            public string Bar { get; set; }
        }

        public class Foo
        {

        }

        public class FooProfile : ContextProfile<Context>
        {
            public override void Configure(Context context)
            {
                CreateMap<Foo, FooDto>().ForMember(dest => dest.Bar, opt => opt.MapFrom(src => context.Bar));
            }
        }

        [Fact()]
        public void AddMapperContextTest()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(GetType());
            services.AddSingleton(new Context { Bar = "abc" });
            services.AddAutoMapperContext<Context>();
            var sp = services.BuildServiceProvider();

            var mapper = sp.GetRequiredService<IMapper>();
            var context = sp.GetRequiredService<Context>();
            var foos = new[] { new Foo() }.AsQueryable();

            var fooDto = mapper.ProjectTo<FooDto>(foos).First();
            Assert.Equal("abc", fooDto.Bar);

            context.Bar = "dfe";
            fooDto = mapper.ProjectTo<FooDto>(foos).First();
            Assert.Equal("dfe", fooDto.Bar);

            context.Bar = "ghi";
            fooDto = mapper.ProjectTo<FooDto>(foos).First();
            Assert.Equal("ghi", fooDto.Bar);
        }
    }
}