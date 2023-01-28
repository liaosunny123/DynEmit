namespace NuGetUnitTest;

public class ArguTest
{
    [Test]
    public void ArguStringTest()
    {
        DynEmitMethod<Func<string, string>> method
            = new DynEmitMethod<Func<string, string>>("method");
        var dynArgument = method.LoadArgument(0);
        dynArgument.PushValue();
        var test = (string)method.Invoke("123");
        Assert.AreEqual("123",test);
    }

    [Test]
    public void ArguIntTest()
    {
        DynEmitMethod<Func<int, int>> method
            = new DynEmitMethod<Func<int, int>>("method");
        var dynArgument = method.LoadArgument(0);
        dynArgument.PushValue();
        var test = (int)method.Invoke(123);
        Assert.AreEqual(123,test);
    }
    
    [Test]
    public void ArguArrayTest()
    {
        DynEmitMethod<Func<string[],int, string[]>> method
            = new ("method",typeof(string[]),new (){typeof(string[]),typeof(int)});
        var argu = method.LoadArgument(0);
        var array = argu.CastToLocalVariable(typeof(string[]));
        ((DynArray<object>)array).PushMemeberValue(1);
        var test = (string)method.GetDelegate().DynamicInvoke(new string[]{ "123","1234"},123);
        Assert.AreEqual("1234",test);
    }
}