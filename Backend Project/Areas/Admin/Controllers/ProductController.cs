﻿using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Backend_Project.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        public ProductController(AppDbContext context,
                                IWebHostEnvironment env,
                                IProductService productService,
                                ICategoryService categoryService,
                                IColorService colorService,
                                ISizeService sizeService,
                                ITagService tagService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sizeService = sizeService;
            _tagService = tagService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5,int? cateId = null)
        {
            List<Product> products = await _productService.GetPaginateDatas(page, take,cateId);
            List<ProductListVM> mappedDatas = GetMappedDatas(products);
            int pageCount = await GetPageCountAsync(take);
            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);
            ViewBag.take = take;
            return View(paginatedDatas);
        }

        private async Task<SelectList> GetCategoryAsync()
        {
            IEnumerable<Category> categories = await _categoryService.GetCategories();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetColorAsync()
        {
            IEnumerable<Color> colors = await _colorService.GetAllColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetSizeAsync()
        {
            IEnumerable<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetTagAsync()
        {
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductListVM productVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    MainImage = product.MainImage,
                    SKU = product.SKU
                };

                mappedDatas.Add(productVM);

            }

            return mappedDatas;
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Product product = await _productService.GettFullDataById((int)id);
            if (product is null) return NotFound();

            ProductDetailVM model = new()
            {
                Name = product.Name,
                SaleCount = product.SaleCount,
                StockCount = product.StockCount,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                ProductCategories = product.ProductCategories,
                ProductImages = product.Images,
                ProductSizes = product.ProductSizes,
                ProductTags = product.ProductTags,
                HoverImage = product.HoverImage,
                MainImage = product.MainImage,
                ColorName = product.Color.Name,
                Rate = product.Rate
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.categories = await GetCategoryAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.sizes = await GetSizeAsync();
            ViewBag.colors = await GetColorAsync();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            try
            {
                ViewBag.categories = await GetCategoryAsync();
                ViewBag.tags = await GetTagAsync();
                ViewBag.sizes = await GetSizeAsync();
                ViewBag.colors = await GetColorAsync();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                Product newProduct = new();
                List<ProductImage> productImages = new();
                List<ProductCategory> productCategories = new();
                List<ProductTag> productTags = new();
                List<ProductSize> productSizes = new();


                //main image
                if (!model.MainImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.MainImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string mainImagefileName = Guid.NewGuid().ToString() + "_" + model.MainImage.FileName;
                string mainImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", mainImagefileName);
                await FileHelper.SaveFileAsync(mainImagepath, model.MainImage);
                newProduct.MainImage = mainImagefileName;


                //hover image
                if (!model.HoverImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.HoverImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string hoverImagefileName = Guid.NewGuid().ToString() + "_" + model.HoverImage.FileName;
                string hoverImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", hoverImagefileName);
                await FileHelper.SaveFileAsync(hoverImagepath, model.HoverImage);
                newProduct.HoverImage = hoverImagefileName;

                //all images
                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }

                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                }


                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(path, photo);

                    ProductImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);
                }
                newProduct.Images = productImages;



                if (model.CategoryIds.Count > 0)
                {
                    foreach (var cateId in model.CategoryIds)
                    {
                        ProductCategory productCategory = new()
                        {
                            CategoryId = cateId
                        };

                        productCategories.Add(productCategory);
                    }
                    newProduct.ProductCategories = productCategories;
                }
                else
                {
                    ModelState.AddModelError("CategoryIds", "Don't be empty");
                    return View();
                }



                if (model.TagIds.Count > 0)
                {
                    foreach (var tagId in model.TagIds)
                    {
                        ProductTag productTag = new()
                        {
                            TagId = tagId
                        };

                        productTags.Add(productTag);
                    }
                    newProduct.ProductTags = productTags;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }



                if (model.SizeIds.Count > 0)
                {
                    foreach (var sizeId in model.SizeIds)
                    {
                        ProductSize productSize = new()
                        {
                            SizeId = sizeId
                        };

                        productSizes.Add(productSize);
                    }
                    newProduct.ProductSizes = productSizes;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }

                var convertPrice = decimal.Parse(model.Price);
                Random random = new();

                newProduct.Name = model.Name;
                newProduct.Description = model.Description;
                newProduct.Price = convertPrice;
                newProduct.StockCount = model.StockCount;
                newProduct.SaleCount = model.SaleCount;
                newProduct.Rate = model.Rate;
                newProduct.ColorId = model.ColorId;
                newProduct.SKU = model.Name.Substring(0, 3) + "_" + random.Next();

                //await _context.ProductImages.AddRangeAsync(productImages);
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == null) return BadRequest();

                Product dbProduct = await _productService.GetFullDataById(id);

                if (dbProduct is null) return NotFound();

                foreach (var item in dbProduct.Images)
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", item.Image);
                    FileHelper.DeleteFile(path);
                }

                string mainPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbProduct.MainImage);
                FileHelper.DeleteFile(mainPath);

                string hoverPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbProduct.HoverImage);
                FileHelper.DeleteFile(hoverPath);

                _context.Products.Remove(dbProduct);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.categories = await GetCategoryAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.sizes = await GetSizeAsync();
            ViewBag.colors = await GetColorAsync();


            if (id == null) return BadRequest();
            Product dbProduct = await _productService.GettFullDataById(id);
            if (dbProduct is null) return NotFound();

            ProductUpdateVM model = new()
            {
                Id = dbProduct.Id,
                Images = dbProduct.Images.ToList(),
                Name = dbProduct.Name,
                Description = dbProduct.Description,
                MainImage = dbProduct.MainImage,
                HoverImage = dbProduct.HoverImage,
                Price = dbProduct.Price,
                SaleCount = dbProduct.SaleCount,
                StockCount = dbProduct.StockCount,
                SKU = dbProduct.SKU,
                TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                CategoryIds = dbProduct.ProductCategories.Select(m => m.Category.Id).ToList(),
                SizeIds = dbProduct.ProductSizes.Select(m=>m.Size.Id).ToList(),
                ColorId = dbProduct.Color.Id
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductUpdateVM model)
        {
            ViewBag.categories = await GetCategoryAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.sizes = await GetSizeAsync();
            ViewBag.colors = await GetColorAsync();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id is null) return BadRequest();
            Product product = await _productService.GettFullDataById((int)id);
            if (product is null) return NotFound();

            List<ProductImage> productImages = new();
            List<ProductCategory> productCategories = new();
            List<ProductTag> productTags = new();
            List<ProductSize> productSizes = new();

            //main image

            if (model.MainPhoto is not null)
            {
                if (!model.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.MainPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string mainImagefileName = Guid.NewGuid().ToString() + "_" + model.MainPhoto.FileName;
                string mainImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", mainImagefileName);
                await FileHelper.SaveFileAsync(mainImagepath, model.MainPhoto);
                product.MainImage = mainImagefileName;
            }
            else
            {
                model.MainImage = product.MainImage;
            }



            //hover image
            if (model.HoverPhoto is not null)
            {
                if (!model.HoverPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.HoverPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string hoverImagefileName = Guid.NewGuid().ToString() + "_" + model.HoverPhoto.FileName;
                string hoverImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", hoverImagefileName);
                await FileHelper.SaveFileAsync(hoverImagepath, model.HoverPhoto);
                product.HoverImage = hoverImagefileName;
            }
            else
            {
                model.HoverImage = product.HoverImage;
            }
           


            //all images
            if (model.Photos is not null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }

                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                }

                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);
                    await FileHelper.SaveFileAsync(path, photo);

                    ProductImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);
                }
                product.Images = productImages;
            }
            else
            {
                model.Images = product.Images.ToList();
            }

            if (model.CategoryIds.Count > 0)
            {
                foreach (var cateId in model.CategoryIds)
                {
                    ProductCategory productCategory = new()
                    {
                        CategoryId = cateId
                    };

                    productCategories.Add(productCategory);
                }
                product.ProductCategories = productCategories;
            }
            else
            {
                ModelState.AddModelError("CategoryIds", "Don't be empty");
                return View();
            }



            if (model.TagIds.Count > 0)
            {
                foreach (var tagId in model.TagIds)
                {
                    ProductTag productTag = new()
                    {
                        TagId = tagId
                    };

                    productTags.Add(productTag);
                }
                product.ProductTags = productTags;
            }
            else
            {
                ModelState.AddModelError("TagIds", "Don't be empty");
                return View();
            }



            if (model.SizeIds.Count > 0)
            {
                foreach (var sizeId in model.SizeIds)
                {
                    ProductSize productSize = new()
                    {
                        SizeId = sizeId
                    };

                    productSizes.Add(productSize);
                }
                product.ProductSizes = productSizes;
            }
            else
            {
                ModelState.AddModelError("TagIds", "Don't be empty");
                return View();
            }

            product.Id = model.Id;
            product.Name = model.Name;
            product.Price = model.Price;
            product.SaleCount = model.SaleCount;
            product.StockCount = model.StockCount;
            product.SKU = model.SKU;
            product.Description = model.Description;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }

    }
}
