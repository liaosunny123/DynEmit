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

    private List<DynVariable> _agrumentList = new List<DynVariable>();
    /// <summary>
    /// 构建动态方法
    /// </summary>
    /// <param name="name">方法名称</param>
    /// <param name="returnType">返回类型</param>
    /// <param name="types">参数列表</param>
    public DynEmitMethod(string name, Type? returnType, List<Type>? types)
    {
        _dynEmitMethod = new DynamicMethod(name, returnType, types?.ToArray());
        _il = _dynEmitMethod.GetILGenerator();
        if (types != null)
        {
            int count = 0;
            types.ForEach(sp =>
            {
                _il.Emit(OpCodes.Ldarg,count++);
                if (sp == typeof(int))
                {
                    _agrumentList.Add(new DynInt(_il));
                }else if (sp == typeof(long))
                {
                    _agrumentList.Add(new DynLong(_il));
                }else if (sp == typeof(MethodInfo))
                {
                    _agrumentList.Add(new DynMethodInfo(_il));
                }else if (sp == typeof(object))
                {
                    _agrumentList.Add(new DynObject(_il));
                }else if (sp == typeof(string))
                {
                    _agrumentList.Add(new DynString(_il));
                }else if (sp == typeof(Type))
                {
                    _agrumentList.Add(new DynType(_il));
                }
            });
        }
    }
    
    /// <summary>
    /// 创建本地 String 变量
    /// </summary>
    /// <param name="var">值</param>
    /// <returns></returns>
    public DynString CreateLocalVariable(string var) => new DynString(_il, var);

    /// <summary>
    /// 创建本地 Int 变量
    /// </summary>
    /// <param name="var"></param>
    /// <returns></returns>
    public DynInt CreateLocalVariable(int var) => new DynInt(_il, var);

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
    
    /// <summary>
    /// 执行方法，并将结果（如果有）推送到栈顶
    /// </summary>
    /// <param name="methodInfo">调用的方法信息</param>
    /// <param name="paras">参数</param>
    /// <param name="hasReturn">是否有返回值</param>
    /// <returns></returns>
    public DynVariable? ActionStaticMethod(MethodInfo methodInfo, List<DynVariable> paras,
        CastType castType = CastType.DynObject)
    {
        paras.ForEach(sp => sp.PushValue());
        _il.Emit(OpCodes.Call, methodInfo);
        if (methodInfo.ReturnType != typeof(void))
            return castType switch
            {
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
    public T GetInvoke<T>(Type type, params object[] objects)
    {
        return (T)this.GetDelegate(type).DynamicInvoke(objects)!;
    }

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public object? Invoke(Type type, params object[] objects)
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
    public T GetInvoke<T, TU>(Type type, params object[] objects)
    {
        return (T)this.GetDelegate(typeof(TU)).DynamicInvoke(objects)!;
    }

    /// <summary>
    /// 直接调用返回的方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public object? Invoke<T>(Type type, params object[] objects)
    {
        Delegate @delegate = this.GetDelegate(typeof(T));
        if (@delegate.Method.ReturnType != typeof(void))
            return @delegate.DynamicInvoke(objects);
        return null;
    }
}