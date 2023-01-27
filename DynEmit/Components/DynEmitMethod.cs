using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using DynEmit.Model;

namespace DynEmit.Components;

/// <summary>
/// 动态方法
/// </summary>
public class DynEmitMethod<TDel> where TDel : Delegate
{
    private DynamicMethod _dynEmitMethod;

    public ILGenerator IlGenerator => _il;

    private ILGenerator _il;

    private List<DynVariable> _agrumentList = new List<DynVariable>();


    /// <summary>
    /// 构建动态方法
    /// </summary>
    /// <param name="name">方法名称</param>
    /// <param name="returnType">返回类型</param>
    /// <param name="types">参数列表</param>
    public DynEmitMethod(string name)
    {
        var start = Stopwatch.GetTimestamp();
        var returnType = typeof(TDel).GetMethods()[0].ReturnType;
        var types = typeof(TDel)
            .GetMethods()[0]
            .GetParameters()?
            .Select(sp => sp.ParameterType).ToList();
        _dynEmitMethod = new DynamicMethod(name, returnType, types?.ToArray());
        _il = _dynEmitMethod.GetILGenerator();
        Console.WriteLine(Stopwatch.GetTimestamp() - start);
    }

    /// <summary>
    /// 构建动态方法
    /// </summary>
    /// <param name="name">方法名称</param>
    /// <param name="returnType">返回类型</param>
    /// <param name="types">参数列表</param>
    public DynEmitMethod(string name, Type returnType, List<Type>? types)
    {
        var start = Stopwatch.GetTimestamp();
        _dynEmitMethod = new DynamicMethod(name, returnType, types?.ToArray());
        _il = _dynEmitMethod.GetILGenerator();
        Console.WriteLine(Stopwatch.GetTimestamp() - start);
    }

    /// <summary>
    /// 创建本地 String 变量
    /// </summary>
    /// <param name="var">值</param>
    /// <returns></returns>
    public DynString CreateLocalVariable(string var) => new(_il, var);

    /// <summary>
    /// 创建本地 Int 变量
    /// </summary>
    /// <param name="var"></param>
    /// <returns></returns>
    public DynInt CreateLocalVariable(int var) => new(_il, var);

    /// <summary>
    /// 创建本地 Long 变量
    /// </summary>
    /// <param name="var"></param>
    /// <param name="isPointer"></param>
    /// <returns></returns>
    public DynVariable CreateLocalVariable(long var, bool isPointer = false)
    {
        if (isPointer)
        {
            return new DynPointer(_il, var);
        }

        return new DynLong(_il, var);
    }

    /// <summary>
    /// 创建本地 MethodInfo 变量
    /// </summary>
    /// <param name="var"></param>
    /// <returns></returns>
    public DynMethodInfo CreateLocalVariable(MethodInfo var) => new DynMethodInfo(_il, var);

    /// <summary>
    /// 创建本地 Type 变量
    /// </summary>
    /// <param name="var"></param>
    /// <returns></returns>
    public DynType CreateLocalVariable(Type var) => new DynType(_il, var);

    public DynArgument LoadArgument(int index)
        => new DynArgument(_il, index);

    /// <summary>
    /// 执行方法，并将结果（如果有）推送到栈顶
    /// </summary>
    /// <param name="methodInfo">调用的方法信息</param>
    /// <param name="paras">参数</param>
    /// <param name="castType"></param>
    /// <returns></returns>
    public DynVariable? ActionStaticMethod(MethodInfo methodInfo, List<DynVariable> paras,
        CastType castType = CastType.DynObject)
    {
        paras.ForEach(sp => sp.PushValue());
        _il.Emit(OpCodes.Call, methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return castType switch
            {
                CastType.DynVoid => new DynVoid(),
                CastType.DynObject => new DynObject(_il),
                CastType.DynString => new DynString(_il),
                CastType.DynInt => new DynInt(_il),
                CastType.DynLong => new DynLong(_il),
                CastType.DynType => new DynType(_il),
                CastType.DynPointer => new DynPointer(_il),
                CastType.DynMethodInfo => new DynMethodInfo(_il),
                _ => throw new ArgumentOutOfRangeException(nameof(castType), castType, null)
            };
        return null;
    }

    /// <summary>
    /// 执行非静态方法
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="dynObject"></param>
    /// <param name="paras"></param>
    /// <param name="castType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DynVariable? ActionNonStaticMethod(MethodInfo methodInfo, DynObject dynObject,List<DynVariable> paras,
        CastType castType = CastType.DynObject)
    {
        dynObject.PushValue();
        paras.ForEach(sp => sp.PushValue());
        _il.Emit(OpCodes.Callvirt, methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return castType switch
            {
                CastType.DynVoid => new DynVoid(),
                CastType.DynObject => new DynObject(_il),
                CastType.DynString => new DynString(_il),
                CastType.DynInt => new DynInt(_il),
                CastType.DynLong => new DynLong(_il),
                CastType.DynType => new DynType(_il),
                CastType.DynPointer => new DynPointer(_il),
                CastType.DynMethodInfo => new DynMethodInfo(_il),
                _ => throw new ArgumentOutOfRangeException(nameof(castType), castType, null)
            };
        return null;
    }

    /// <summary>
    /// 执行非静态方法
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="dynObject"></param>
    /// <param name="castType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public DynVariable? ActionNonStaticMethodFromStack(MethodInfo methodInfo, DynObject dynObject,
        CastType castType = CastType.DynObject)
    {
        dynObject.PushValue();
        _il.Emit(OpCodes.Callvirt, methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return castType switch
            {
                CastType.DynVoid => new DynVoid(),
                CastType.DynObject => new DynObject(_il),
                CastType.DynString => new DynString(_il),
                CastType.DynInt => new DynInt(_il),
                CastType.DynLong => new DynLong(_il),
                CastType.DynType => new DynType(_il),
                CastType.DynPointer => new DynPointer(_il),
                CastType.DynMethodInfo => new DynMethodInfo(_il),
                _ => throw new ArgumentOutOfRangeException(nameof(castType), castType, null)
            };
        return null;
    }
    
    /// <summary>
    /// 从堆栈顶直接执行方法
    /// </summary>
    /// <param name="methodInfo">调用的方法信息</param>
    /// <param name="castType"></param>
    /// <returns></returns>
    public DynVariable? ActionStaticMethodFromStack(MethodInfo methodInfo, CastType castType = CastType.DynObject)
    {
        _il.Emit(OpCodes.Call, methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return castType switch
            {
                CastType.DynVoid => new DynVoid(),
                CastType.DynObject => new DynObject(_il),
                CastType.DynString => new DynString(_il),
                CastType.DynInt => new DynInt(_il),
                CastType.DynLong => new DynLong(_il),
                CastType.DynType => new DynType(_il),
                CastType.DynPointer => new DynPointer(_il),
                CastType.DynMethodInfo => new DynMethodInfo(_il),
                _ => throw new ArgumentOutOfRangeException(nameof(castType), castType, null)
            };
        return null;
    }

    public Delegate GetDelegate()
    {
        _il.Emit(OpCodes.Ret);
        return _dynEmitMethod.CreateDelegate(typeof(TDel));
    }

    /// <summary>
    /// 从方法体上装载参数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public DynArgument LoadFromMethodArgument(int index) => new DynArgument(_il, index);

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetInvoke<T>(params object[] objects)
    {
        return (T)this.GetDelegate().DynamicInvoke(objects)!;
    }

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public object? Invoke(params object[] objects)
    {
        Delegate @delegate = this.GetDelegate();
        if (@delegate.Method.ReturnType != typeof(void))
            return @delegate.DynamicInvoke(objects);
        @delegate.DynamicInvoke(objects);
        return null;
    }
}