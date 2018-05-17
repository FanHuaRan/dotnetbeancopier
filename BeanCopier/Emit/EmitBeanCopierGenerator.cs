using BeanCopier.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Emit
{
    public class EmitBeanCopierGenerator : BeanCopierGenerator
    {
        private readonly ConcurrentDictionary<String,Type> emitTypes = new ConcurrentDictionary<string, Type>();

        private readonly EmitBeanCopierTypeCreator typeCreator = new EmitBeanCopierTypeCreator();

        public BeanCopier<T, V> Generate<T, V>()
            where T : class
            where V : class
        {
            var sourceType = typeof(T);
            var desType = typeof(V);
            var sourceTypeName = sourceType.Name;
            var desTypeName = desType.Name;

            var copierTypeName = String.Format(EmitBeanCopierConstant.EMIT_BEAN_COPIER_NAMEFORMAT, sourceTypeName,
                desTypeName);

            // build the beancopier base reflection
            var commonPropertys = CommonProperty<T, V>.GetCommonPropertyPairs(sourceType, desType);
            emitTypes.GetOrAdd(copierTypeName,
                p =>
                {
                    return typeCreator.CreateBeanCopierType<T, V>(EmitTypeCreateContext.Instance.ModuleBuilder,
                        commonPropertys);
                });

            var type = emitTypes[copierTypeName];

            return (BeanCopier<T,V>)Activator.CreateInstance(type);
        }
    }
}
