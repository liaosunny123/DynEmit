using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

/// <summary>
/// 动态 Long
/// </summary>
public class DynLong : DynVariable
{
    private LocalBuilder _var;
    private ILGenerator _il;

    /// <summary>
    /// 通过计算栈顶构造本地 Long
    /// </summary>
    /// <param name="il"></param>
    public DynLong(ILGenerator il)
    {
        _il = il;
        _var = _il.DeclareLocal(typeof(long));
        _il.Emit(OpCodes.Stloc, _var);
    }

    /// <summary>
    /// 通过特定值构造本地 Long
    /// </summary>
    /// <param name="il"></param>
    /// <param name="value"></param>
    public DynLong(ILGenerator il, long value)
    {
        _il = il;
        _il.Emit(OpCodes.Ldc_I8, value);
        _var = _il.DeclareLocal(typeof(long));
        _il.Emit(OpCodes.Stloc, _var);
    }

    public override void PushValue() => _il.Emit(OpCodes.Ldloc, _var);
    
}