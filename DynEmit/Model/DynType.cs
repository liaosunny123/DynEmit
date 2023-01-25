using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

/// <summary>
/// 动态 Type
/// </summary>
public class DynType : DynVariable
{
    private LocalBuilder _var;
    private ILGenerator _il;

    /// <summary>
    /// 通过计算栈顶构造本地 Type
    /// </summary>
    /// <param name="il"></param>
    public DynType(ILGenerator il)
    {
        _il = il;
        _var = _il.DeclareLocal(typeof(Type));
        _il.Emit(OpCodes.Stloc, _var);
    }

    /// <summary>
    /// 通过特定值构造本地 Type
    /// </summary>
    /// <param name="il"></param>
    /// <param name="value"></param>
    public DynType(ILGenerator il, Type value)
    {
        _il = il;
        _il.Emit(OpCodes.Ldtoken, value);
        _var = _il.DeclareLocal(typeof(Type));
        _il.Emit(OpCodes.Stloc, _var);
    }

    public override void PushValue() => _il.Emit(OpCodes.Ldloc, _var);
}