using System.Reflection;

namespace NuGetUnitTest;

public class EmitMethod
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void StringCommonMethodInvoke()
    {
        MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) })!;
        DynEmitMethod<Action> method = new ("Say",null,null);
        DynString dynString = method.CreateLocalVariable("123");
        method.ActionStaticMethod(writeLine, new () { dynString });
        method.Invoke();
        Assert.Pass();
    }
    
    [Test]
    public void IntCommonMethodInvoke()
    {
        MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new[] { typeof(int) })!;
        DynEmitMethod<Action> method = new ("Say",null,null);
        var dynVar = method.CreateLocalVariable(2);
        method.ActionStaticMethod(writeLine, new () { dynVar });
        method.Invoke();
        Assert.Pass();
    }
    
    
}