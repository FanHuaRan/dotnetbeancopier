using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Core
{
    /// <summary>
    /// 自定义转换器接口，客户端使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface BeanConverter<T, V>
        where T : class
        where V : class
    {
        V Convert(T souce, V target);
    }
}
