using AutoMapper;

namespace Stagazer.Extensions.AutoMapper.Context
{
    public abstract class ContextProfile<T> : Profile
    {
        public ContextProfile()
        {
            Configure(default!);
        }

        public abstract void Configure(T context);
    }
}
