using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeanCopier.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanCopier.Core;
using BeanCopier.Tests;

namespace BeanCopier.Emit.Tests
{
    [TestClass()]
    public class EmitBeanCopierTests
    {

        [TestMethod()]
        public void copyTest()
        {
            BeanCopier<User, UserInfo> beanCopier = beanCopier = EmitBeanCopierFactory.Instance.Create<User, UserInfo>();
            var user = new User()
            {
                Name = "Tom",
                Id = 1,
                Age = 12,
                Password = "****"
            };

            var userInfo = new UserInfo();

            beanCopier.Copy(user, userInfo);

            Assert.AreEqual(user.Name, userInfo.Name);
            Assert.AreEqual(user.Id, userInfo.Id);
            Assert.AreEqual(user.Age, userInfo.Age);
            Assert.AreEqual(null, userInfo.Gender);

        }

        [TestMethod()]
        public void copyTest1()
        {
            BeanCopier<User, UserInfo> beanCopier = beanCopier = EmitBeanCopierFactory.Instance.Create<User, UserInfo>();
            var user = new User()
            {
                Name = "Tom",
                Id = 1,
                Age = 12,
                Password = "****"
            };

            var userInfo = new UserInfo();

            var selfConverter = new SelfConverter();

            beanCopier.Copy(user, userInfo, selfConverter);

            Assert.AreEqual(user.Name, userInfo.Name);
            Assert.AreEqual(user.Id, userInfo.Id);
            Assert.AreEqual(user.Age, userInfo.Age);
            Assert.AreEqual(null, userInfo.Gender);
        }

        [TestMethod()]
        public void pressureTest()
        {
            var beanCopier = EmitBeanCopierFactory.Instance.Create<User, UserInfo>();

            var user = new User()
            {
                Name = "Tom",
                Id = 1,
                Age = 12,
                Password = "****"
            };

            var userInfo = new UserInfo();
            for (int i = 0; i < 10000000; i++)
            {
                beanCopier.Copy(user, userInfo);
            }

            Assert.AreEqual(user.Name, userInfo.Name);
            Assert.AreEqual(user.Id, userInfo.Id);
            Assert.AreEqual(user.Age, userInfo.Age);
            Assert.AreEqual(null, userInfo.Gender);
        }
    }

    public class SelfConverter : BeanConverter<User, UserInfo>
    {
        public UserInfo Convert(User souce, UserInfo target)
        {
            Console.WriteLine("Hello World");
            return target;
        }
    }
}