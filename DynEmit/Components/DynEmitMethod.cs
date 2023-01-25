using System.Reflection;
using System.Reflection.Emit;
using DynEmit.Model;

namespace DynEmit.Components;

/// <summary>
/// 动态方法
/// </summary>
public class DynEmitMethod
{
    private DynamicMethod _dynEmitMethod;

    public ILGenerator IlGenerator => _il;
    
    private ILGenerator _il;
    
    /// <summary>
    /// 构建动态方法
    /// </summary>
    /// <param name="name">方法名称</param>
    /// <param name="returnType">返回类型</param>
    /// <param name="types">参数列表</param>
    public DynEmitMethod(string name,Type? returnType,List<Type>? types)
    {
        _dynEmitMethod = new DynamicMethod(name, returnType, types?.ToArray());
        _il = _dynEmitMethod.GetILGenerator();
    }
    
    /// <summary>
    /// 创建本地 String 变量
    /// </summary>
    /// <param name="var">值</param>
    /// <returns></returns>
    public DynString CreateLocalVariable(string var)
        => new DynString(_il, var);

    /// <summary>
    /// 执行方法，并将结果（如果有）推送到栈顶
    /// </summary>
    /// <param name="methodInfo">调用的方法信息</param>
    /// <param name="paras">参数</param>
    /// <param name="hasReturn">是否有返回值</param>
    /// <returns></returns>
    public DynObject? ActionStaticMethod(MethodInfo methodInfo,List<DynVariable> paras)
    {
        paras.ForEach( sp => sp.PushValue());
        _il.Emit(OpCodes.Call,methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return new DynObject(_il);
        return null;
    }

    public Delegate GetDelegate(Type type)
    {
        _il.Emit(OpCodes.Ret);
        return _dynEmitMethod.CreateDelegate(type);
    }

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetInvoke<T>(Type type,params object[] objects)
    {
        return (T)this.GetDelegate(type).DynamicInvoke(objects)!;
    }

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public object? Invoke(Type type,params object[] objects)
    {
        Delegate @delegate = this.GetDelegate(type);
        if (@delegate.Method.ReturnType != typeof(void))
            return @delegate.DynamicInvoke(objects);
        return null;
    }
    
    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU">创建的委托类型</typeparam>
    /// <returns></returns>
    public T GetInvoke<T,TU>(Type type,params object[] objects)
    {
        return (T)this.GetDelegate(typeof(TU)).DynamicInvoke(objects)!;
    }
 
    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public object? Invoke<T>(Type type,params object[] objects)
    {
        Delegate @delegate = this.GetDelegate(typeof(T));
        if (@delegate.Method.ReturnType != typeof(void))
            return @delegate.DynamicInvoke(objects);
        return null;
    }
}