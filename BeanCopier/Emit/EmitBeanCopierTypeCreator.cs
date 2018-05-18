using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BeanCopier.Core;

namespace BeanCopier.Emit
{
    /// <summary>
    /// 基于Emit的BeanCopier实现类生成，核心是动态代码生成
    /// </summary>
    internal class EmitBeanCopierTypeCreator
    {
        public Type CreateBeanCopierType<T, V>(ModuleBuilder moduleBuilder,
            CommonProperty<T, V>[] commonPropertys)
            where T : class
            where V : class
        {
            var sourceType = typeof(T);
            var desType = typeof(V);
            var sourceTypeName = sourceType.Name;
            var desTypeName = desType.Name;

            var copierTypeName = String.Format(EmitBeanCopierConstant.EMIT_BEAN_COPIER_NAMEFORMAT,sourceTypeName,desTypeName);

            var typeBuilder = moduleBuilder.DefineType(copierTypeName, TypeAttributes.Public);
            /// 实现BeanCopier<T,V>接口 
            typeBuilder.AddInterfaceImplementation(typeof(BeanCopier<T,V>));
            // 实现copy(T,V)方法
            var methodBuilder = BuildAssemblyCopyMethod<T, V>(commonPropertys, typeBuilder);
            // 实现Copy(T,V,BeanConverter)方法
            var methodWithConverterBuilder = BuildAssemblyCopyMethodWithConverter<T,V>(commonPropertys, typeBuilder);

            // 建立方法对接口的重载关系（这儿最好说成是实现）
            MethodInfo copyMethodInfoInfo = typeof(BeanCopier<T,V>).GetMethod("Copy",new Type[]{
            typeof(T),typeof(V)});
            typeBuilder.DefineMethodOverride(methodBuilder, copyMethodInfoInfo);

            MethodInfo copyWithConverterMethodInfoInfo = typeof(BeanCopier<T, V>).GetMethod("Copy", new Type[]{
                typeof(T),typeof(V),typeof(BeanConverter<T,V>)});
            typeBuilder.DefineMethodOverride(methodWithConverterBuilder, copyWithConverterMethodInfoInfo);

           // EmitTypeCreateContext.Instance.AssemblyBuilder.Save(EmitBeanCopierConstant.EMIT_BEAN_COPIER_ASSEMBLY_FILE);

            return typeBuilder.CreateType();
        }

        private static MethodBuilder BuildAssemblyCopyMethod<T, V>(CommonProperty<T, V>[] commonPropertys, TypeBuilder typeBuilder)
            where T : class where V : class
        {
            var returnType = typeof(void);
            Type[] paramTypes = {typeof(T), typeof(V)};

            // 定义void Copy(T,V)
            var methodBuilder = typeBuilder.DefineMethod("Copy", MethodAttributes.Public | MethodAttributes.Virtual, returnType,
                paramTypes);

            // 使用ILGenerator为方法进行实现
            var ilGenerator = methodBuilder.GetILGenerator();

            // 遍历赋值
            foreach (var commonProperty in commonPropertys)
            {
                // 如果修改操作码，则填补空间
                ilGenerator.Emit(OpCodes.Nop);
                // 第二个参数入栈顶
                ilGenerator.Emit(OpCodes.Ldarg_2);
                // 第一个参数入栈顶
                ilGenerator.Emit(OpCodes.Ldarg_1);
                // 调用第一个参数的属性获取方法
                ilGenerator.Emit(OpCodes.Callvirt, commonProperty.SourcePropeyty.GetGetMethod());
                // 调用第二个参数的属性设置方法
                ilGenerator.Emit(OpCodes.Callvirt, commonProperty.DestinationProperty.GetSetMethod());
            }

            // 如果修改操作码，则填补空间
            ilGenerator.Emit(OpCodes.Nop);
            // 返回
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        private static MethodBuilder BuildAssemblyCopyMethodWithConverter<T, V>(CommonProperty<T, V>[] commonPropertys, TypeBuilder typeBuilder)
            where T : class 
            where V : class
        {
            var returnType = typeof(void);
            var converterType = typeof(BeanConverter<T, V>);
            Type[] paramTypes = { typeof(T), typeof(V), converterType };

            // 定义void Copy(T,V,BeanConverter)
            var methodBuilder = typeBuilder.DefineMethod("Copy", MethodAttributes.Public | MethodAttributes.Virtual, returnType,
                paramTypes);

            // 使用ILGenerator为方法进行实现
            var ilGenerator = methodBuilder.GetILGenerator();

            // 遍历赋值
            foreach (var commonProperty in commonPropertys)
            {
                // 如果修改操作码，则填补空间
                ilGenerator.Emit(OpCodes.Nop);
                // 第二个参数入栈顶
                ilGenerator.Emit(OpCodes.Ldarg_2);
                // 第一个参数入栈顶
                ilGenerator.Emit(OpCodes.Ldarg_1);
                // 调用第一个参数的属性获取方法
                ilGenerator.Emit(OpCodes.Callvirt, commonProperty.SourcePropeyty.GetGetMethod());
                // 调用第二个参数的属性设置方法
                ilGenerator.Emit(OpCodes.Callvirt, commonProperty.DestinationProperty.GetSetMethod());
            }
            // 如果修改操作码，则填补空间
            ilGenerator.Emit(OpCodes.Nop);
            // 第二个参数入栈顶
            ilGenerator.Emit(OpCodes.Ldarg_2);
            // 第一个参数入栈顶
            ilGenerator.Emit(OpCodes.Ldarg_1);
            // 第三个参数入栈
            ilGenerator.Emit(OpCodes.Ldarg_3);
            // 调用converter的convert方法
            ilGenerator.Emit(OpCodes.Callvirt,converterType.GetMethod("Convert", new Type[] { typeof(T), typeof(V) }));

            // 如果修改操作码，则填补空间
            ilGenerator.Emit(OpCodes.Nop);
            // 返回
            ilGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}
