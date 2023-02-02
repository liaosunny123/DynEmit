# DynEmit
[![NuGet](https://img.shields.io/nuget/vpre/DynEmit?style=flat&label=NuGet&color=9866ca)](https://www.nuget.org/packages/DynEmit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DynEmit?style=flat&label=Downloads&color=42a5f5)](https://www.nuget.org/packages/DynEmit/)  
A NuGet for simply generate ASM through EMIT in .NET with optimizing performance.  
使用IL动态生成字节码的NuGet包，可以大幅优化程序部分场景性能和创造更多可能。
# Language  
[简体中文](https://github.com/liaosunny123/DynEmit/blob/master/README.md)  
[English](https://github.com/liaosunny123/DynEmit/blob/master/README-EN.md)
# 特点  
- 低成本学习  
- API友好，对底层封装完善，几乎零接触OpCodes等低代码  
- 几乎无损耗操作，通过在编译期解决个性化需求来优化程序性能，且大幅优化反射性能  
- 更为友好的编写逻辑，及时抛出报错。  
# 示例  
更多示例请在Wiki界面查看：  
例如，调用Console的WriteLine方法打印`123`：  
```csharp
MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) })!;
DynEmitMethod method = new ("Say",null,null);
DynString dynString = method.CreateLocalVariable("123");
method.ActionStaticMethod(writeLine, new () { dynString });
method.Invoke(typeof(Action));
```  
