using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shoper.BusinessLogic.Interface;
using Shoper.Entities;
using Shoper.Management.Models.ViewModels;

namespace Shoper.Management.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductImageService _productImageService;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductDiscountService _productDiscountService;
        public ProductController(IProductService productService, ICategoryService categoryService, IProductImageService productImageService, IProductPriceService productPriceService, IProductDiscountService productDiscountService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._productImageService = productImageService;
            this._productPriceService = productPriceService;
            this._productDiscountService = productDiscountService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetAll());
        }
        public IActionResult Create()
        {
            ViewData["categories"] = _categoryService.GetAll();
            // view data hem controller da hem de view da erişilir 
            // amacı view ile data arasında getir götür yapmaktadır. 
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product model, IEnumerable<IFormFile> img, decimal price, UnitPrice unitPrice) // asp-for olmayanları burada yazmak gerekir. 
                                                                                                                   // çünki model dışı olur yani producta ait değil bunlar farklı classlar 
        {
            if (model != null)
            {
                var InsertedModel = _productService.Add(model);

                if (img.Count() > 0)
                {
                    byte[] data;
                    // her resmi buna parçala 
                    foreach (var item in img)
                    {
                        ProductImage imageModel = new ProductImage();
                        imageModel.ProductId = InsertedModel.ProductId;
                        using (var br = new BinaryReader(item.OpenReadStream()))
                        {
                            // binaryreader resmi bytlara bölüp kaydediyor. 
                            data = br.ReadBytes((int)item.OpenReadStream().Length);
                            imageModel.Image = data;
                        }
                        _productImageService.Add(imageModel);
                    }
                }
                ProductPrice priceModel = new ProductPrice()
                {
                    isValidFlag = true,
                    // her bir ürün yeni bir fiyat 
                    // ikinci bir fiyat ilk oluştururken giremezsin  
                    Price = price,
                    UnitPrice = unitPrice,
                    ProductId = InsertedModel.ProductId
                };
                _productPriceService.Add(priceModel);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            Product_Price_Img_VM model = new Product_Price_Img_VM();
            model.Product = _productService.Get(id);
            model.Price = _productPriceService.GetExp(x => x.ProductId == id).FirstOrDefault(x=>x.isValidFlag==true); // güncel fiyat gelecek 
            model.Images = _productImageService.GetExp(x => x.ProductId == id).ToList();
            
            ViewBag.Categories = new SelectList(_categoryService.GetAll(),"CategoryId","CategoryName",model.Product.CategoryId);
            // ilgili ürünün kategori idsini aldık 
            //yeni bir select list oluşturup tüm kategorileri getiriyor 
            // bu listenin içinden seçilen categoryId yi gönder 
            // sonda ki parametre "categoryId" bunun için 
            return View(model);

        }
        [HttpPost]
        public IActionResult Edit(Product_Price_Img_VM model, IEnumerable<IFormFile> img, decimal price, UnitPrice unitPrice) // asp-for olmayanları burada yazmak gerekir. 
                                                                                                                              // çünki model dışı olur yani producta ait değil bunlar farklı classlar 
        {
            if (model != null)
            {
                var UpdatedModel = _productService.Update(model.Product);

                if (img.Count() > 0)
                {
                    byte[] data;
                    // her resmi buna parçala 
                    foreach (var item in img)
                    {
                        ProductImage imageModel = new ProductImage();
                        imageModel.ProductId = UpdatedModel.ProductId;
                        using (var br = new BinaryReader(item.OpenReadStream()))
                        {
                            // binaryreader resmi bytlara bölüp kaydediyor. 
                            data = br.ReadBytes((int)item.OpenReadStream().Length);
                            imageModel.Image = data;
                        }
                        _productImageService.Add(imageModel);
                    }
                }
               // model.Price.isValidFlag = true;
               // model.Price.ProductId = UpdatedModel.ProductId;
                ProductPrice priceModel = new ProductPrice()
                {
                   PriceId= model.Price.PriceId,
                    isValidFlag = true,
                    // değişen fiyat da güncel fiyat olduğu için true olaacak
                    // true güncelliği gösterir 
                    Price = model.Price.Price,
                    UnitPrice = unitPrice,
                    ProductId = UpdatedModel.ProductId
                };
                _productPriceService.Update(priceModel);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult PriceHistory(int id)
        {
            return View(_productPriceService.GetExp(x => x.ProductId == id));// bir den fazla gidiyor bir liste onun için where yapısı olması lazım
        }

        public IActionResult CreateNewPrice(int productId) // hangi ürüne ait yeni bir fiyat oluşturulacak 
        {
            ProductPrice model = new ProductPrice();
            model.ProductId= productId; // belli olan bir ürüne yeni fiyat ekleme formunda yazıyoruz 
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateNewPrice(ProductPrice model) 
        {
            if (model!= null)
            {
                model.isValidFlag = false; // geçerli olmasın 
                _productPriceService.Add(model);
                return RedirectToAction("PriceHistory", new {id=model.ProductId});
                //price history id istiyor onun için bizim productıd vermemiz gerekir. 
            }
            return View(model);
        }
        //[NonAction] // metot yazıcaz 
        public bool ImageDelete(int ImageId)
        {
            var result=_productImageService.Delete(_productImageService.Get(ImageId));
            return result!=null;
            // servisler de get komutu id ye göre bulup getiriyorud 
            // resut nullsa false değilse ture gönderir 
            // yani silme işlemi yapılmadıysa false 
        }

        public bool SetValidFlag(int PriceId)
        {
            ProductPrice priceModel= _productPriceService.Get(PriceId);

            if (priceModel!=null)
            {
                foreach (var item in _productPriceService.GetExp(x=>x.ProductId==priceModel.ProductId))
                {
                    // id ye göre ürünün tüm fiyatlarını getiriyoruz ve hepsini geçersiz yapıyor.
                    // yeniden bir fiyatı aktif yapabilmek için önceden hepsini sıfırlıyoruz. 
                    item.isValidFlag = false;
                    _productPriceService.Update(item);
                }
                priceModel.isValidFlag = true; // seçtiğmiz fiyatın değerini geçerli yap 
                _productPriceService.Update(priceModel);
                return true;
            }
            return false;
        }

        public IActionResult Discounts(int id)
        {
            return View(_productDiscountService.GetExp(d=>d.ProductId==id));
            // producta ait indirim listesi gidecek 
        }
       
    }
}
