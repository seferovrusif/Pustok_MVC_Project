using Pustok_project.Contexts;
using Pustok_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_project.ViewModels.SliderVM;
using Pustok_project.ViewModels.ProductVM;
using System.Xml.Linq;
using Pustok_project.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok_project.ViewModels.Common;
using Pustok_project.ViewModels.TagVM;
using Pustok_project.Controllers;

namespace Pustok_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }


        public ProductController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            int take = 4;

            var items = _db.Product.Take(take).Select(s => new ProductListItemVM
            {

                Id = s.Id,
                Name = s.Name,
                ProductCode = s.ProductCode,
                About = s.About,
                Description = s.Description,
                SellPrice = s.SellPrice,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                Quantity = s.Quantity,
                CategoryId = s.CategoryId,
                ProductMainImg = s.ProductMainImg,
                IsDeleted = s.IsDeleted,
                Tag = s.TagProducts.Select(a => a.Tag).ToList(),

            });
            int count = await _db.Product.CountAsync();
            PaginationVM<IEnumerable<ProductListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / take), items);

            return View(pag);

        }
        public IActionResult Create()
        {
            ViewBag.Tag = _db.Tag;
            ViewBag.Category = _db.Category;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            if (vm.ProductMainImg != null)
            {
                if (!vm.ProductMainImg.IsCorrectType())
                {
                    ModelState.AddModelError("ProductMainImg", "Wrong file type");
                }
                if (!vm.ProductMainImg.IsValidSize())
                {
                    ModelState.AddModelError("ProductMainImg", "Files length must be less than kb");
                }
            }
            if (vm.ProductImages != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.ProductImages)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize())
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
                //if (!string.IsNullOrEmpty(message))
                //{
                //    ModelState.AddModelError("Images", message);
                //}
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Category = _db.Category;
                ViewBag.Tag = new SelectList(_db.Tag, "Id", "Name");
                return View(vm);
            }
            //if (!await _db.Category.AnyAsync(c => c.Id == vm.categ))
            //{
            //    ModelState.AddModelError("CategoryId", "Category doesnt exist");
            //    ViewBag.Categories = _db.Categories;
            //    ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
            //    return View(vm);
            //}
            //var a = await (from c in _db.Colors
            //               where vm.ColorIds.Contains(c.Id)
            //               select c.Id).CountAsync();
            if (await _db.Tag.Where(c => vm.TagId.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagId.Count())
            {
                ModelState.AddModelError("TagId", "Tag doesnt exist");
                ViewBag.Category = _db.Category;
                ViewBag.Tag = _db.Tag;
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                ProductCode = vm.ProductCode,
                About = vm.About,
                Description = vm.Description,
                SellPrice = vm.SellPrice,
                CostPrice = vm.CostPrice,
                Discount = vm.Discount,
                CategoryId = vm.CategoryId,
                Quantity = vm.Quantity,
                IsDeleted = vm.IsDeleted,
                ProductMainImg = vm.ProductMainImg.SaveAsync(PathConstants.Productimages).Result,
                TagProducts = vm.TagId.Select(id => new TagProduct
                {
                    TagId = id,
                }).ToList(),
                ProductImages = vm.ProductImages.Select(i => new ProductImage
                {
                    ImagePath = i.SaveAsync(PathConstants.Productimages).Result
                }).ToList()
            };
            await _db.Product.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Category = _db.Category;
            ViewBag.Tag = _db.Tag;
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Product
              .Include(p => p.ProductImages)
              .Include(p => p.TagProducts)
              .SingleOrDefaultAsync(p => p.Id == id);
            //var data = await _db.Product.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateProductVM
            {
                Name = data.Name,
                ProductCode = data.ProductCode,
                About = data.About,
                Description = data.Description,
                SellPrice = data.SellPrice,
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                CategoryId = data.CategoryId,
                Quantity = data.Quantity,
                IsDeleted = data.IsDeleted,
                ProductMainImgstr = data.ProductMainImg,
                ImageUrls = data.ProductImages?.Select(pi => new ProductImageVM
                {
                    Id = pi.Id,
                    Url = pi.ImagePath
                })
            });
        } 
        [HttpPost]
        public async Task<IActionResult> Update (int id , UpdateProductVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            

            if (!vm.TagId.Any())
            {
                ModelState.AddModelError("TagId", "You must add at least 1 Tag");
            }
            var data = await _db.Product
                              .Include(p => p.ProductImages)

                      .Include(p => p.TagProducts)
                      .SingleOrDefaultAsync(p => p.Id == id);
            //var data = await _db.Product.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
                data.ProductCode = vm.ProductCode;
                data.About = vm.About;
                data.Description = vm.Description;
                data.SellPrice = vm.SellPrice;
                data.CostPrice = vm.CostPrice;
                data.Discount = vm.Discount;
                data.CategoryId = vm.CategoryId;
                data.Quantity = vm.Quantity;
                data.IsDeleted = vm.IsDeleted;
            if (vm.TagId != null)
            {
                data.TagProducts = vm.TagId.Select(id => new TagProduct
                {
                    TagId = id,
                }).ToList();
            }
            if (vm.Images != null)
            {
                var imgs = vm.Images.Select(i => new ProductImage
                {
                    ImagePath = i.SaveAsync(PathConstants.Productimages).Result,
                    ProductId = data.Id
                });
                
               data.ProductImages.AddRange(imgs);
            }
            if (vm.ProductMainImg != null)
            {
                data.ProductMainImg = await vm.ProductMainImg.SaveAsync(PathConstants.Productimages);
            }

            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //TempData["Response"] = false;
            if (id == null) return BadRequest();

             var data =await _db.Product.FindAsync(id);
            if (data == null) return NotFound();
            _db.Product.Remove(data);
            await _db.SaveChangesAsync();
            //TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
       
       
        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var items = _db.Product.Skip((page - 1) * count).Take(count).Select(s => new ProductListItemVM
            {

                Id = s.Id,
                Name = s.Name,
                ProductCode = s.ProductCode,
                About = s.About,
                Description = s.Description,
                SellPrice = s.SellPrice,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                Quantity = s.Quantity,
                CategoryId = s.CategoryId,
                ProductMainImg = s.ProductMainImg,
                IsDeleted = s.IsDeleted,
                TagId = s.TagProducts.Select(a => a.TagId).ToList(),

            });
            int totalCount = await _db.Product.CountAsync();
            PaginationVM<IEnumerable<ProductListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);

            return PartialView("_ProductPaginationPartial", pag);
        }
    }
    }





