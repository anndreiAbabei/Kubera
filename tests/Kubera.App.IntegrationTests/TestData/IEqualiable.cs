namespace Kubera.App.IntegrationTests.TestData
{
    public interface IEqualiable<in T1, in T2>
    {
        bool AreEqual(T1 t1, T2 t2);
    }
}
