using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeanCopier.Core;
namespace BeanCopier.Reflection
{
    /// <summary>
    /// 基于反射的属性复制器实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class ReflectionBeanCopier<T, V> : BeanCopier<T, V>
         where T : class
         where V : class
    {

        public PropertyInfo[] SourcePropertyInfos { get; internal set; }

        public PropertyInfo[] DesPropertyInfos { get; internal set; }

        internal CommonPropertyPair<T,V>[] CommonPropertyInfos { get;  set; }

        internal ReflectionBeanCopier()
        {

        }

        public void Copy(T source, V destination)
        {
            if(CommonPropertyInfos == null)
            {
                return;
            }

            foreach(var commonProperty in CommonPropertyInfos)
            {
                var sourceValue = commonProperty.ReadSourceValue(source);
                commonProperty.SetDestinationValue(destination, sourceValue);
            }
        }

        public void Copy(T source, V destination, BeanConverter<T, V> beanConverter)
        {
            if(beanConverter == null)
            {
                throw new ArgumentException("converter must not be null");
            }

            Copy(source, destination);

            beanConverter.Convert(source, destination);
        }
    }


}
