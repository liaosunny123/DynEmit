using System.Reflection;
using System.Reflection.Emit;
using DynEmit.Components;

namespace DynEmit.Model;

/// <summary>
/// 动态 Object
/// </summary>
public class DynObject : DynVariable
{
    private LocalBuilder _var;
    private ILGenerator _il;
    
    /// <summary>
    /// 通过计算栈顶构造本地 Object
    /// </summary>
    /// <param name="il"></param>
    public DynObject(ILGenerator il)
    {
        _il = il;
        _var = _il.DeclareLocal(typeof(object));
        _il.Emit(OpCodes.Stloc,_var);
    }

    public DynObject(ILGenerator il, ConstructorInfo constructorInfo)
    {
        _il = il;
        _il.Emit(OpCodes.Newobj, constructorInfo);
        _var = _il.DeclareLocal(typeof(object));
        _il.Emit(OpCodes.Stloc,_var);
    }
    
    public DynObject(ILGenerator il, ConstructorInfo constructorInfo,Type type)
    {
        _il = il;
        _il.Emit(OpCodes.Newobj, constructorInfo);
        _var = _il.DeclareLocal(type);
        _il.Emit(OpCodes.Stloc,_var);
    }
    
    public override void PushValue() => _il.Emit(OpCodes.Ldloc, _var);
}