using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

public class DynArray<T> : DynVariable
{

    private ILGenerator _il;
    private LocalBuilder _var;
    
    public DynArray(ILGenerator il)
    {
        this._il = il;
        this._var = il.DeclareLocal(typeof(T[]));
        il.Emit(OpCodes.Stloc,_var);
    }
    
    public override void PushValue()
    {
        _il.Emit(OpCodes.Ldloc,_var);
    }

    public void PushMemeberValue(int index)
    {
        _il.Emit(OpCodes.Ldarg,_var);
        _il.Emit(OpCodes.Ldc_I4,index);
        _il.Emit(OpCodes.Ldelem_Ref);
    }
}