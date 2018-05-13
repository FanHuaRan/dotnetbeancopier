using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Reflection
{
    /// <summary>
    /// 兼容属性实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal class CommonPropertyPair<T, V>
           where T : class
         where V : class
    {
        private readonly PropertyInfo sourcePropeyty;

        private readonly PropertyInfo destinationProperty;

        public Object ReadSourceValue(T source)
        {
            if (source == null || !sourcePropeyty.CanRead)
            {
                return null;
            }
            else
            {
                return sourcePropeyty.GetValue(source);
            }
        }

        public void SetDestinationValue(V destination, Object value)
        {
            if (destination == null || value == null || !destinationProperty.CanWrite)
            {
                return;
            }

            destinationProperty.SetValue(destination, value);
        }

        /// utilities ///
        public static Boolean IsCommonProperty(PropertyInfo sourceProperty, PropertyInfo desProperty)
        {
            if (sourceProperty == null || desProperty == null)
            {
                throw new ArgumentException("property must not be null");
            }

            var typeEquals = sourceProperty.PropertyType.Equals(desProperty.PropertyType);
            var nameEquals = sourceProperty.Name.Equals(desProperty.Name);
            return typeEquals && nameEquals;
        }

        public static CommonPropertyPair<T, V>[] GetCommonPropertyPairs(Type sourceType, Type desType)
        {
            checkTypeParam<T, V>(sourceType, desType);

            IList<CommonPropertyPair<T, V>> commonPropertyPairs = new List<CommonPropertyPair<T, V>> ();

            var sourcePropertys = sourceType.GetProperties();
            foreach (var sourceProperty in sourcePropertys)
            {
                PropertyInfo propertyInfo = desType.GetProperty(sourceProperty.Name);
                if (propertyInfo != null && IsCommonProperty(sourceProperty, propertyInfo))
                {
                    commonPropertyPairs.Add(new CommonPropertyPair<T, V>(sourceProperty, propertyInfo));
                }
            }

            return commonPropertyPairs.ToArray();
        }

        private static void checkTypeParam<T, V>(Type source, Type destination)
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

        internal CommonPropertyPair(PropertyInfo sourcePropeyty, PropertyInfo destinationProperty)
        {
            this.sourcePropeyty = sourcePropeyty;
            this.destinationProperty = destinationProperty;
        }

    }
}
