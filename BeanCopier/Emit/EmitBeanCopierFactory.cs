using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeanCopier.Core;
using BeanCopier.Emit;

namespace BeanCopier.Emit
{
    /// <summary>
    /// 基于Emit的属性复制器的工厂，使用单例
    /// </summary>
    public class EmitBeanCopierFactory : BeanCopierFactory
    {

        // if we use ConcurrentDictionary and the getOrUpdate atomic method , it's ThreadSafe.
        private readonly ConcurrentDictionary<String, object> beanCopiers = new ConcurrentDictionary<string, object>();

        private readonly EmitBeanCopierGenerator emitBeanCopierGenerator = new EmitBeanCopierGenerator();

        public BeanCopier<T, V> Create<T, V>()
            where T : class
            where V : class
        {
            var source = typeof(T);
            var destination = typeof(V);

            var key = buildTheCacheKey(source, destination);
            var copier = beanCopiers.GetOrAdd(key, p => emitBeanCopierGenerator.Generate<T, V>());

            if (copier == null)
            {
                return (BeanCopier<T, V>)beanCopiers[key];
            }
            else
            {
                return (BeanCopier<T, V>)copier;
            }

        }

        private String buildTheCacheKey(Type source, Type destination)
        {
            return String.Format("{0}.{1}-{2}.{3}", source.Namespace, source.Name, destination.Namespace, destination.Name);
        }


        #region Singleton

        protected static EmitBeanCopierFactory instance = null;

        protected static readonly Object instanceLocker = new object();

        public static EmitBeanCopierFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLocker)
                    {
                        if (instance == null)
                        {
                            instance = new EmitBeanCopierFactory();
                        }
                    }
                }

                return instance;
            }
        }

        private EmitBeanCopierFactory()
        {

        }
        #endregion
    }

}