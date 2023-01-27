namespace NuGetUnitTest;

public class DynRetTest
{
    [Test]
    public void StringRet()
    {
        var method = new DynEmitMethod<Func<string>>("BackString",typeof(string),null);
        var dynVar = method.CreateLocalVariable("hello");
        dynVar.PushValue();
        string test = method.GetInvoke<string>();
        Assert.AreEqual("hello",test);
    }
    
    [Test]
    public void IntRet()
    {
        var method = new DynEmitMethod<Func<int>>("BackInt",typeof(int),null);
        var dynVar = method.CreateLocalVariable(123);
        dynVar.PushValue();
        int test = method.GetInvoke<int>();
        Assert.AreEqual(123,test);
    }
}