using BeanCopier.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Reflection
{
    /// <summary>
    /// 基于反射策略的BeanCopier生成器实现
    /// </summary>
    public class ReflectionBeanCopierGenerator : BeanCopierGenerator

    {
        public BeanCopier<T, V> Generate<T, V>()
             where T : class
            where V : class
        {
            var source = typeof(T);
            var destination = typeof(V);

            // build the beancopier base reflection
            var commonPropertyPairs = CommonPropertyPair<T,V>.GetCommonPropertyPairs(source, destination);

            var beanCopier = new ReflectionBeanCopier<T, V>();
            beanCopier.CommonPropertyInfos = commonPropertyPairs;
            beanCopier.SourcePropertyInfos = source.GetProperties();
            beanCopier.DesPropertyInfos = destination.GetProperties();

            return beanCopier;
        }

        private void checkTypeParam<T, V>(Type source, Type destination)
            where T : class
           where V : class
        {
            var typeT = typeof(T);
            var typeV = typeof(V);
            if (!typeT.Equals(source))
            {
                throw new ArgumentException("source type is wrong.");
            }
            if (!typeV.Equals(destination))
            {
                throw new ArgumentException("source type is wrong.");
            }
        }

    }
}
