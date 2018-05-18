using BeanCopier.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BeanCopier.Emit
{
    /// <summary>
    /// 基于Emit的BeanCopier生成器，核心是动态代码生成，因为生成需要时间，这儿对生成的类型动用缓存
    /// </summary>
    public class EmitBeanCopierGenerator : BeanCopierGenerator
    {
        private readonly ConcurrentDictionary<String, Type> emitTypes = new ConcurrentDictionary<string, Type>();

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

            // 判断类型兼容性
            ValidType(sourceType, desType);

            // 获取可以兼容的属性
            var commonPropertys = CommonProperty<T, V>.GetCommonPropertyPairs(sourceType, desType);

            // 构造类型并缓存
            var dynamicType = emitTypes.GetOrAdd(copierTypeName,
                p =>
                {
                    return typeCreator.CreateBeanCopierType<T, V>(EmitTypeCreateContext.Instance.ModuleBuilder,
                        commonPropertys);
                });

            // 反射生成对象
            return (BeanCopier<T,V>)Activator.CreateInstance(dynamicType);
        }

        /// <summary>
        /// 判断类型兼容性
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="desType"></param>
        /// <returns></returns>
        private void ValidType(Type sourceType, Type desType)
        {
            // 只有一个兼容条件： 两个类都是public
            if (!sourceType.IsPublic)
            {
                throw new ArgumentException("source type must be public");
            }

            if (!desType.IsPublic)
            {
                throw new ArgumentException("destination type must be public");
            }
        }
    }
}
