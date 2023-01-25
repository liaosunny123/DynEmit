using System.Reflection;
using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

/// <summary>
/// 动态 MethodInfo
/// </summary>
public class DynMethodInfo : DynVariable
{
    private LocalBuilder _var;
    private ILGenerator _il;

    /// <summary>
    /// 通过计算栈顶构造本地 MethodInfo
    /// </summary>
    /// <param name="il"></param>
    public DynMethodInfo(ILGenerator il)
    {
        _il = il;
        _var = _il.DeclareLocal(typeof(MethodInfo));
        _il.Emit(OpCodes.Stloc, _var);
    }

    /// <summary>
    /// 通过特定值构造本地 MethodInfo
    /// </summary>
    /// <param name="il"></param>
    /// <param name="value"></param>
    public DynMethodInfo(ILGenerator il, MethodInfo value)
    {
        _il = il;
        _il.Emit(OpCodes.Ldtoken, value);
        _var = _il.DeclareLocal(typeof(MethodInfo));
        _il.Emit(OpCodes.Stloc, _var);
    }

    public override void PushValue() => _il.Emit(OpCodes.Ldloc, _var);
}