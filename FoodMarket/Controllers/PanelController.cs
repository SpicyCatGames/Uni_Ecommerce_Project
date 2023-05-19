using FoodMarket.Data.FileManager;
using FoodMarket.Data.Repository;
using FoodMarket.Models;
using FoodMarket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllIItems());
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new ItemViewModel());
            else
            {
                var item = _repo.GetItem((int)id);
                return View(new ItemViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                    Stock = item.Stock,
                    CurrentImage = item.Image,
                    Category = item.Category,
                    Description = item.Description
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemViewModel vm)
        {
            Item item = new Item
            {
                Id = vm.Id,
                Title = vm.Title,
                Price = vm.Price,
                Stock = vm.Stock,
                Category = vm.Category,
                Description = vm.Description
            };

            if (vm.Image == null)
                item.Image = vm.CurrentImage;
            else
            {
                if (!String.IsNullOrEmpty(vm.CurrentImage))
                {
                    _fileManager.RemoveImage(vm.CurrentImage);
                }
                item.Image = await _fileManager.SaveImage(vm.Image);
            }
                

            if (item.Id > 0)
                _repo.UpdateItem(item);
            else
                // new post
                _repo.AddItem(item);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else
            {
                return View(item);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemoveItem(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}