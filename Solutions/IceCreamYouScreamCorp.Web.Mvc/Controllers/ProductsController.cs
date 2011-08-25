using System.Web.Mvc;
using IceCreamYouScreamCorp.Domain;
using SharpArch.NHibernate.Contracts.Repositories;
using SharpArch.NHibernate.Web.Mvc;
using MvcContrib;

namespace IceCreamYouScreamCorp.Web.Mvc.Controllers
{
    public class ProductsController : Controller
    {
        private INHibernateRepository<Product> _productRepository;

        public ProductsController(INHibernateRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public ActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if(!TryUpdateModel(product))
            {
                ViewBag.updateError = "Create Failure";
                return View(product);
            }
            _productRepository.SaveOrUpdate(product);
            return this.RedirectToAction(x => x.Index());
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            if (!id.HasValue)
                return this.RedirectToAction(x => x.Index());

            var product = _productRepository.Get(id.Value);
            return View(product);
        }

        [Transaction]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(Product product)
        {
            if (ModelState.IsValid && product.IsValid())
            {
                _productRepository.SaveOrUpdate(product);
                return this.RedirectToAction(x => x.Index());
            }

            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return this.RedirectToAction(x => x.Index());

            var product = _productRepository.Get(id.Value);
            _productRepository.Delete(product);
            return this.RedirectToAction(x => x.Index());
        }
    }
}