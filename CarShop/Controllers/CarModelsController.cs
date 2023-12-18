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
using Microsoft.AspNetCore.Identity;
using CarShop.Models.Accounts;
using CarShop.Cloud;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CarShop.Controllers;

[Authorize(Roles = CustomRoles.AdminOrDealer)]
public class CarModelsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly FileService _fileService;

    public CarModelsController
        (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            FileService fileService
        )
    {
        _fileService = fileService;
        _context = context;
        _userManager = userManager;
    }

    // GET: CarModels
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.CarModels.Include(c => c.CarBrand);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: CarModels/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carModel = await _context.CarModels
            .Include(c => c.CarBrand)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (carModel == null)
        {
            return NotFound();
        }

        return View(carModel);
    }

    // GET: CarModels/Create
    public async Task<IActionResult> Create()
    {
        ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name");
        return View();
    }

    // POST: CarModels/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,CarBrandId,ImgFile")] CarModel carModel)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        carModel.AppUser = currentUser;
        carModel.AppUserId = currentUser.Id;

        if (ModelState.IsValid)
        {
            var carBrand =
                await _context.CarBrands.FirstOrDefaultAsync(cb => cb.Id == carModel.CarBrandId);

            carModel.CarBrand = carBrand;

            var uploadResult =
                await _fileService.UploadAsync(carModel.ImgFile, carModel.GeneratePrefix());

            carModel.ImgFileName = uploadResult.Blob.Name;
            carModel.ImgUrl = uploadResult.Blob.Uri;

            _context.Add(carModel);
            await _context.SaveChangesAsync();
            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
        ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
        return View(carModel);
    }

    // GET: CarModels/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carModel = await _context.CarModels.FindAsync(id);
        if (carModel == null)
        {
            return NotFound();
        }
        ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
        return View(carModel);
    }

    // POST: CarModels/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CarBrandId,ImgFile,ImgFileName,ImgUrl,AppUserId")] CarModel carModel)
    {
        if (id != carModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (carModel.ImgFile != null)
            {
                if (carModel.ImgFileName != null)
                {
                    await _fileService.DeleteAsync(carModel.ImgFileName);
                }

                var uploadResult =
                    await _fileService.UploadAsync(carModel.ImgFile, carModel.GeneratePrefix());
                carModel.ImgFileName = uploadResult.Blob.Name;
                carModel.ImgUrl = uploadResult.Blob.Uri;
            }


            try
            {
                _context.Update(carModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarModelExists(carModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectPermanent(Request.Headers.Referer.ToString());
        }
        ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
        return View(carModel);
    }

    // GET: CarModels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carModel = await _context.CarModels
            .Include(c => c.CarBrand)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (carModel == null)
        {
            return NotFound();
        }

        return View(carModel);
    }

    // POST: CarModels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var carModel = await _context.CarModels.FindAsync(id);
        if (carModel != null)
        {
            var res = await _fileService.GetBlobAsync(carModel.ImgFileName);
            if (res != null)
                await _fileService.DeleteAsync(carModel.ImgFileName);
            _context.CarModels.Remove(carModel);
        }

        await _context.SaveChangesAsync();
        //return RedirectToAction(nameof(Index));

        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);

        var isAdmin = roles.Any(r => r == CustomRoles.Admin);

        if (isAdmin)
            return RedirectToAction(nameof(Index));
        else
            return RedirectPermanent(Request.Headers.Referer.ToString());
    }

    private bool CarModelExists(int id)
    {
        return _context.CarModels.Any(e => e.Id == id);
    }
}
