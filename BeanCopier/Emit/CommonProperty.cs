using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Emit
{
    /// <summary>
    /// 兼容属性实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal class CommonProperty<T, V>
           where T : class
         where V : class
    {
        private readonly PropertyInfo sourcePropeyty;

        private readonly PropertyInfo destinationProperty;

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

        public static CommonProperty<T, V>[] GetCommonPropertyPairs(Type sourceType, Type desType)
        {
            checkTypeParam<T, V>(sourceType, desType);

            IList<CommonProperty<T, V>> commonPropertyPairs = new List<CommonProperty<T, V>> ();

            var sourcePropertys = sourceType.GetProperties();
            foreach (var sourceProperty in sourcePropertys)
            {
                PropertyInfo propertyInfo = desType.GetProperty(sourceProperty.Name);
                if (propertyInfo != null && IsCommonProperty(sourceProperty, propertyInfo))
                {
                    commonPropertyPairs.Add(new CommonProperty<T, V>(sourceProperty, propertyInfo));
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

        internal CommonProperty(PropertyInfo sourcePropeyty, PropertyInfo destinationProperty)
        {
            this.sourcePropeyty = sourcePropeyty;
            this.destinationProperty = destinationProperty;
        }

        public  PropertyInfo SourcePropeyty
        {
            get { return this.sourcePropeyty; }
        }

        public PropertyInfo DestinationProperty
        {
            get { return this.destinationProperty; }
        }
    }
}
