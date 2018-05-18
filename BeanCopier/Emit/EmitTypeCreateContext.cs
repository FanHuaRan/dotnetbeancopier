using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BeanCopier.Emit
{
    /**
     * Emit代理类的上下文
     */
    internal class EmitTypeCreateContext
    {
        private readonly  AssemblyBuilder assemblyBuilder;

        private readonly  ModuleBuilder moduleBuilder;

        public  AssemblyBuilder AssemblyBuilder
        {
            get { return this.assemblyBuilder; }
        }

        public  ModuleBuilder ModuleBuilder
        {
            get { return this.moduleBuilder; }
        }

        protected EmitTypeCreateContext(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
        {
            this.assemblyBuilder = assemblyBuilder;
            this.moduleBuilder = moduleBuilder;
        }
        #region Singleton
        private static EmitTypeCreateContext instance;

        public static EmitTypeCreateContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (INITIAL_LOCKER)
                    {
                        if (instance == null)
                        {
                            // 获取当前的应用程序域
                            var appDomain = AppDomain.CurrentDomain;

                            // 动态在当前的AppDomain当中创建一个程序集 
                            var assemblyName = new AssemblyName(EmitBeanCopierConstant.EMIT_BEAN_COPIER_ASSEMBLY_NAME);
                            // 该程序集的访问模式设置为run,意味着该程序集只驻留在内存，不会持久化
                            // var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                            // 该程序集的访问模式设置为runAndSave,意味着该程序集既可驻留内存,又可以持久化
                            var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

                            // 动态在程序集内创建一个模块
                            // var moduleBuilder = assemblyBuilder.DefineDynamicModule("MyDynamicModule");
                            // 动态在程序集内创建一个模块，RunAndSave模式下，后面需要指定保存的文件名
                            var moduleBuilder = assemblyBuilder.DefineDynamicModule(EmitBeanCopierConstant.EMIT_BEAN_COPIER_MODULE_NAME, EmitBeanCopierConstant.EMIT_BEAN_COPIER_ASSEMBLY_FILE);

                            instance = new EmitTypeCreateContext(assemblyBuilder,moduleBuilder);
                        }
                    }
                }

                return instance;
            }
        }

        private static readonly Object INITIAL_LOCKER = new Object();
        #endregion
    }
}
