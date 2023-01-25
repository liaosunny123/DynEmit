using System.Reflection.Emit;

namespace DynEmit.Model;

/// <summary>
/// 动态 String
/// </summary>
public class DynString
{
    private LocalBuilder _var;
    private ILGenerator _il;
    public DynString(ILGenerator il,string value)
    {
        _il = il;
        _il.Emit(OpCodes.Ldstr, value);
        _var = _il.DeclareLocal(typeof(string));
        _il.Emit(OpCodes.Stloc,_var);
    }

    public void PushValue() => _il.Emit(OpCodes.Ldstr, _var);
}