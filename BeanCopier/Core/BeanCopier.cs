using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Core
{
    /// <summary>
    /// BeanCopier接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface BeanCopier<T,V>
        where T:class
        where V:class
    {

        void Copy(T source, V destination);

        void Copy(T source, V destination, BeanConverter<T,V> beanConverter);
    }
}
