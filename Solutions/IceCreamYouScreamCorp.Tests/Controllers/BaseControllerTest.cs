using MbUnit.Framework;


namespace IceCreamYouScreamCorp.Tests.Controllers
{
    [TestFixture]
    public abstract class BaseControllerTest
    {
        [MbUnit.Framework.SetUp]
        public virtual void Setup()
        {
            ServiceLocatorInitializer.Init();
        }
    }
}
