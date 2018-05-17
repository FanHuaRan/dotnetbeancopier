using BeanCopier.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeanCopier;
using BeanCopier.Core;

namespace BeanCopierTests
{
    class UserUserInfoBeanCopier : BeanCopier<User, UserInfo>
    {
        public void Copy(User source, UserInfo destination)
        {
            throw new NotImplementedException();
        }

        public void Copy(User source, UserInfo destination, BeanConverter<User, UserInfo> beanConverter)
        {
            throw new NotImplementedException();
        }
    }
}
