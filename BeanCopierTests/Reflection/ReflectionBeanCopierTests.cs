using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeanCopier.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanCopier.Core;
using BeanCopier.Tests;

namespace BeanCopier.Reflection.Tests
{
    [TestClass()]
    public class ReflectionBeanCopierTests
    {

        private static readonly BeanCopier<User, UserInfo> beanCopier = ReflectionBeanCopierFactory.Instance.Create<User, UserInfo>();

        [TestMethod()]
        public void copyTest()
        {
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
           // TODO
        }

        [TestMethod()]
        public void pressureTest()
        {
            var user = new User()
            {
                Name = "Tom",
                Id = 1,
                Age = 12,
                Password = "****"
            };

            var userInfo = new UserInfo();
            for(int i = 0; i < 10000000; i++)
            {
                beanCopier.Copy(user, userInfo);
            }

            Assert.AreEqual(user.Name, userInfo.Name);
            Assert.AreEqual(user.Id, userInfo.Id);
            Assert.AreEqual(user.Age, userInfo.Age);
            Assert.AreEqual(null, userInfo.Gender);
        }

        void test(User user,UserInfo userInfo)
        {
            userInfo.Age = user.Age;
            userInfo.Id = user.Id;
            userInfo.Name = user.Name;
        }
    }
}