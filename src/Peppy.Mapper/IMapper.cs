using System;

namespace Peppy.Mapper
{
    public interface IMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);
    }
}