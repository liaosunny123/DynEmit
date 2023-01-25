using System.Reflection;

namespace NuGetUnitTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void TestCommonMethodInvoke()
    {
        MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) })!;

        DynEmitMethod method = new ("Say",null,null);
        DynString dynString = method.CreateLocalVariable("123");
        method.ActionStaticMethod(writeLine, new () { dynString });
        method.GetDelegate(typeof(Action)).DynamicInvoke();
        
        Assert.Pass();
    }
}