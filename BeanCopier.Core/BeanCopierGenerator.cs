using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Core
{
    /// <summary>
    /// BeanCopier创建器接口，分离创建的的策略和创建的定义
    /// </summary>
    public interface BeanCopierGenerator
    {
        BeanCopier<T, V> Generate<T, V>(Type source, Type destination)
            where T : class
            where V : class;
    }
}
