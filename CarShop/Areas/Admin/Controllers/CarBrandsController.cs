using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop.Data;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Authorization;
using CarShop.Common;
using CarShop.Cloud;

namespace CarShop.Controllers;

[Authorize(Roles = CustomRoles.Admin)]
[Area("Admin")]
public class CarBrandsController : Controller
{
    private readonly FileService _fileService;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CarBrandsController
        (
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment,
            FileService fileService
        )
    {
        _hostEnvironment = hostEnvironment;
        _context = context;
        _fileService = fileService;
    }

    // GET: CarBrands
    public async Task<IActionResult> Index()
    {
        return View(await _context.CarBrands.ToListAsync());
    }

    // GET: CarBrands/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carBrand = await _context.CarBrands
            .FirstOrDefaultAsync(m => m.Id == id);
        if (carBrand == null)
        {
            return NotFound();
        }

        return View(carBrand);
    }

    // GET: CarBrands/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CarBrands/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ImgFile")] CarBrand carBrand)
    {
        ModelState.Remove("ImgFileName");
        ModelState.Remove("CarModels");
        if (ModelState.IsValid)
        {

            var result = await _fileService.UploadAsync(carBrand.ImgFile,carBrand.GeneratePrefix());

            carBrand.ImgFileName = result.Blob.Name;
            carBrand.ImgUrl = result.Blob.Uri;

            _context.Add(carBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(carBrand);
    }

    // GET: CarBrands/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carBrand = await _context.CarBrands.FindAsync(id);
        if (carBrand == null)
        {
            return NotFound();
        }
        return View(carBrand);
    }

    // POST: CarBrands/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImgFile,ImgFileName,ImgUrl")] CarBrand carBrand)
    {
        if (id != carBrand.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if(carBrand.ImgFile != null)
            {
                var currentCarBrand = await _context.CarBrands.AsNoTracking().FirstAsync(cb => cb.Id == id);
                var blob = await _fileService.GetBlobAsync(currentCarBrand.ImgFileName);

                if (blob != null)
                {
                    var removeResult = await _fileService.DeleteAsync(blob.Name);
                }

                var uploadResult = await _fileService.UploadAsync(carBrand.ImgFile, carBrand.GeneratePrefix());

                carBrand.ImgFileName = uploadResult.Blob.Name;
                carBrand.ImgUrl = uploadResult.Blob.Uri;
            }
            
            try
            {
                _context.Update(carBrand);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarBrandExists(carBrand.Id))
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
        return View(carBrand);
    }

    // GET: CarBrands/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carBrand = await _context.CarBrands
            .FirstOrDefaultAsync(m => m.Id == id);
        if (carBrand == null)
        {
            return NotFound();
        }

        return View(carBrand);
    }

    // POST: CarBrands/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var carBrand = await _context.CarBrands.FindAsync(id);
        if (carBrand != null)
        {
            if(carBrand.ImgFileName != null)
            {
                await _fileService.DeleteAsync(carBrand.ImgFileName);
            }
            _context.CarBrands.Remove(carBrand);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CarBrandExists(int id)
    {
        return _context.CarBrands.Any(e => e.Id == id);
    }
}
