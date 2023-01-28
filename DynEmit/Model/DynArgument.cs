using System.Reflection;
using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

public class DynArgument : DynVariable
{
    private ILGenerator _il;
    private int index;

    public DynArgument(ILGenerator il, int index)
    {
        this._il = il;
        this.index = index;
    }

    public override void PushValue()
    {
        _il.Emit(OpCodes.Ldarg, index);
    }

    public DynVariable CastToLocalVariable(Type? type)
    {
        if (type == typeof(string))
        {
            this.PushValue();
            return new DynString(_il);
        }

        if (type == typeof(int))
        {
            this.PushValue();
            return new DynInt(_il);
        }

        if (type == typeof(long))
        {
            this.PushValue();
            return new DynLong(_il);
        }
        
        if (type == typeof(MethodInfo))
        {
            this.PushValue();
            return new DynMethodInfo(_il);
        }
        
        if (type == typeof(Type))
        {
            this.PushValue();
            return new DynType(_il);
        }
        
        if (type == null)
        {
            this.PushValue();
            return new DynVoid();
        }

        if (type.IsArray)
        {
            this.PushValue();
            return new DynArray<object>(_il);
        }
        
        this.PushValue();
        return new DynObject(_il);
    }

    public DynArray<T> CastToTargetArray<T>()
    {
        this.PushValue();
        return new DynArray<T>(_il);
    }
}