using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeanCopier.Core;
namespace BeanCopier.Reflection
{
    /// <summary>
    /// 基于反射的属性复制器的工厂，使用单例
    /// </summary>
    public class ReflectionBeanCopierFactory : BeanCopierFactory
    {

        // if we use ConcurrentDictionary and the getOrUpdate atomic method , it's ThreadSafe.
        private readonly ConcurrentDictionary<String, object> beanCopiers = new ConcurrentDictionary<string, object>();

        private readonly ReflectionBeanCopierGenerator reflectionBeanCopierGenerator = new ReflectionBeanCopierGenerator();

        public BeanCopier<T, V> Create<T, V>()
            where T : class
            where V : class
        {
            var source = typeof(T);
            var destination = typeof(V);

            var key = buildTheCacheKey(source, destination);

            return (BeanCopier<T, V>)beanCopiers.GetOrAdd(key, p => reflectionBeanCopierGenerator.Generate<T, V>());
        }

        private String buildTheCacheKey(Type source, Type destination)
        {
            return String.Format("{0}.{1}-{2}.{3}", source.Namespace, source.Name, destination.Namespace, destination.Name);
        }


        #region Singleton

        protected static ReflectionBeanCopierFactory instance = null;

        protected static readonly Object instanceLocker = new object();

        public static ReflectionBeanCopierFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLocker)
                    {
                        if (instance == null)
                        {
                            instance = new ReflectionBeanCopierFactory();
                        }
                    }
                }

                return instance;
            }
        }

        private ReflectionBeanCopierFactory()
        {

        }
        #endregion
    }

}