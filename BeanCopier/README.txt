﻿仿照java的BeanCopier的.net实现
解决问题是复制两个对象的属性:
复制的限制条件：
源类和目标类均为public
源类的属性需要具备public类型的get访问器
目标类的属性需要具备public类型的set访问器
工程目录：
Core:接口定义
Emit:基于Emit的动态代码实现
Reflection:基于反射的实现
Emit性能是Reflection性能的100倍