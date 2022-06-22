using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Cloudinary _cloudinary;

        public ProductController(IUnitOfWork unitOfWork,
                                IWebHostEnvironment hostEnvironment,
                                Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _cloudinary = cloudinary;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Product.GetAllAsync());
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                        i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        })
            };

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                // update product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }

            //return View(productVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                //string wwwRootPath = _hostEnvironment.WebRootPath;
                //if (file != null)
                //{
                //    string fileName = Guid.NewGuid().ToString();
                //    var uploads = Path.Combine(wwwRootPath, @"images/products");
                //    var extension = Path.GetExtension(file.FileName);

                //    if (obj.Product.ImageUrl != null)
                //    {
                //        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                //        if (System.IO.File.Exists(oldImagePath))
                //        {
                //            System.IO.File.Delete(oldImagePath);
                //        }
                //    }

                //    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                //    {
                //        file.CopyTo(fileStreams);
                //    }
                //    obj.Product.ImageUrl = @"/images/products/" + fileName + extension;

                //}
                if (file != null)
                {
                    await using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        //Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error != null)
                    {
                        throw new Exception(uploadResult.Error.Message);
                    }
                    var photo = new CoverPhoto
                    {
                        Url = uploadResult.Url.ToString(),
                        PublicId = uploadResult.PublicId
                    };
                    obj.Product.CoverPhoto = photo;
                    _unitOfWork.CoverPhoto.Add(photo);
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Product updated successfully";
                }

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}
            var deleteParams = new DeletionParams(obj.CoverPhoto.PublicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.CoverPhoto.Remove(obj.CoverPhoto);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Product delete was successful" });

        }
        #endregion
    }
}

