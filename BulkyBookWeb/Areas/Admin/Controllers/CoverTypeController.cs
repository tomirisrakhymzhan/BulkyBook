using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BulkyBook.Models;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: CoverType
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.CoverType.GetAllAsync());
        }

        // GET: CoverType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = await _unitOfWork.CoverType
                .GetFirstOrDefaultAsync(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // GET: CoverType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoverType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "CoverType created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(coverType);
        }

        // GET: CoverType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = await _unitOfWork.CoverType.GetFirstOrDefaultAsync(m => m.Id == id); ;
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        // POST: CoverType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CoverType.Update(coverType);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "CoverType updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoverTypeExists(coverType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: CoverType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = await _unitOfWork.CoverType
                .GetFirstOrDefaultAsync(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coverType = await _unitOfWork.CoverType
                .GetFirstOrDefaultAsync(m => m.Id == id);
            _unitOfWork.CoverType.Remove(coverType);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CoverTypeExists(int id)
        {
            return _unitOfWork.CoverType.Any(e => e.Id == id);
        }
    }
}
