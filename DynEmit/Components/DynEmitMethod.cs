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
    /// <summary>
    /// 构建动态方法
    /// </summary>
    /// <param name="name">方法名称</param>
    /// <param name="returnType">返回类型</param>
    /// <param name="types">参数列表</param>
    public DynEmitMethod(string name,Type returnType,List<Type> types)
    {
        _dynEmitMethod = new DynamicMethod(name, returnType, types.ToArray());
        _il = _dynEmitMethod.GetILGenerator();
    }
    
    /// <summary>
    /// 创建本地 String 变量
    /// </summary>
    /// <param name="var"></param>
    /// <returns></returns>
    public DynString CreateLocalVariable(string var)
        => new DynString(_il, var);
    
    /// <summary>
    /// 执行方法，并将结果（如果有）推送到栈顶
    /// </summary>
    public void ActionMethod()
}