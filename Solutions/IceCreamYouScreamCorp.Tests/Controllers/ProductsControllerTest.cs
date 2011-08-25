using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IceCreamYouScreamCorp.Domain;
using IceCreamYouScreamCorp.Web.Mvc.Controllers;
using MbUnit.Framework;
using Rhino.Mocks;
using SharpArch.NHibernate.Contracts.Repositories;
using SharpArch.Testing;

namespace IceCreamYouScreamCorp.Tests.Controllers
{
    public class ProductsControllerTest : BaseControllerTest
    {
        private INHibernateRepository<Product> _repository;
        private ProductsController _controller;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _repository = MockRepository.GenerateMock<INHibernateRepository<Product>>();
            _controller = new ProductsController(_repository);
        }

        [Test]
        public void IndexForwardsToIndexWithProducts()
        {
            //Arrange
            _repository.Expect(x => x.GetAll()).Return(new List<Product>{new Product()});

            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.IsNull(result.View);
            Assert.IsInstanceOfType<IList<Product>>(result.Model);
            Assert.AreEqual(1, (result.Model as IList<Product>).Count);
        }

        [Test]
        public void CreateForwardsToCreateWithNewProduct()
        {
            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.IsNull(result.View);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(0, (result.Model as Product).Id);
        }

        [Test]
        public void CreateValidatesBadModelAndForwardsToCreate()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();

            //Act
            var result = _controller.Create(new Product()) as ViewResult;

            //Assert
            Assert.IsNull(result.View);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(0, (result.Model as Product).Id);
        }

        [Test]
        public void CreateValidatesGoodModelAndRedirectsToIndex()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _repository.Expect(x => x.SaveOrUpdate(Arg<Product>.Is.Anything)).Return(new Product());

            //Act
            var result = _controller.Create(new Product {Name = "blah"}) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void UpdateWithoutIDRedirectsToIndex()
        {
            //Act
            var result = _controller.Update((int?)null) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [Test]
        public void UpdateWithIdForwardsToViewWithProduct()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _repository.Expect(x => x.Get(3)).Return(product);

            //Act
            var result = _controller.Update(3) as ViewResult;

            //Assert
            Assert.IsNull(result.View);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(3, (result.Model as Product).Id);
        }

        [Test]
        public void UpdateWithValidModelSavesAndRedirectsToIndex()
        {
            //Arrange
            _repository.Expect(x => x.SaveOrUpdate(Arg<Product>.Is.Anything));

            //Act
            var result = _controller.Update(new Product {Name = "blah"}) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void UpdateWithInvalidModelForwardsBackToUpdate()
        {
            //Act
            var result = _controller.Update(new Product()) as ViewResult;

            //Assert
            Assert.IsNull(result.View);
            Assert.IsNull((result.Model as Product).Name);
        }

        [Test]
        public void DeleteWithoutIdRedirectsToIndex()
        {
            //Act
            var result = _controller.Delete(null) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void DeleteWithIdDeletesProductAndRedirectsToIndex()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _repository.Expect(x => x.Get(3)).Return(product);
            _repository.Expect(x => x.Delete(Arg<Product>.Matches(y => y.Id == 3)));

            //Act
            var result = _controller.Delete(3) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _repository.VerifyAllExpectations();
        }
    }
}
