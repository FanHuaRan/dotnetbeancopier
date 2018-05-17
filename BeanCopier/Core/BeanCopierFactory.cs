using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Core
{
    /// <summary>
    /// BeanCopier工厂
    /// </summary>    
    public interface BeanCopierFactory
    {
        BeanCopier<T,V> Create<T, V>()
            where T : class
            where V : class;
    }
}
