using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

/// <summary>
/// 动态 String
/// </summary>
public class DynString : DynVariable
{
    private LocalBuilder _var;
    private ILGenerator _il;

    /// <summary>
    /// 通过计算栈顶构造本地 String
    /// </summary>
    /// <param name="il"></param>
    public DynString(ILGenerator il)
    {
        _il = il;
        _var = _il.DeclareLocal(typeof(string));
        _il.Emit(OpCodes.Stloc,_var);
    }
    
    /// <summary>
    /// 通过特定值构造本地 String
    /// </summary>
    /// <param name="il"></param>
    /// <param name="value"></param>
    public DynString(ILGenerator il,string value)
    {
        _il = il;
        _il.Emit(OpCodes.Ldstr, value);
        _var = _il.DeclareLocal(typeof(string));
        _il.Emit(OpCodes.Stloc,_var);
    }

    public override void PushValue() => _il.Emit(OpCodes.Ldloc, _var);
}