using DynEmit.Components;
using DynEmit.Components.SafeTrack.Exception;

namespace DynEmit.Model;

/// <summary>
/// 动态 Void
/// </summary>
public class DynVoid : DynVariable
{
    public override void PushValue()
    {
        throw new DynVariableNotFitException("Cannot push a DynVariable which is a void Type.");
    }
}