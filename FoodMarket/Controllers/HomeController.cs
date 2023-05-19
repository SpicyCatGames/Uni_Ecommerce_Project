using FoodMarket.Data;
using FoodMarket.Data.FileManager;
using FoodMarket.Data.Repository;
using FoodMarket.Models;
using FoodMarket.Models.Comments;
using FoodMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index(int pageNumber, string category)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new {pageNumber = 1, category});

            //var posts = String.IsNullOrEmpty(category) ? 
            //    _repo.GetAllPosts(pageNumber) : 
            //    _repo.GetAllPosts(category);

            //var vm = (String.IsNullOrEmpty(category)) ?
            //    await _repo.GetAllPosts(pageNumber)
            //    : await _repo.GetAllPosts(pageNumber, category);

            var vm = await _repo.GetAllItems(pageNumber, category);

            return View(vm);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetItem(id);

            return View(post);
        }

        //[HttpGet("[controller]/Image/{image}")]
        [HttpGet("/Image/{image}")]
        [ResponseCache(Duration = 300)]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId});

            var post = _repo.GetItem(vm.PostId);
            if(vm.MainCommentId == 0) // maincomment
            {
                post.MainComments = post.MainComments ?? new List<Models.Comments.MainComment>();
                post.MainComments.Add(new MainComment()
                {
                    Created = DateTime.Now,
                    Message = vm.Message
                });
                _repo.UpdateItem(post);
            }
            else // subcomment
            {
                var comment = new SubComment()
                {
                    MainCommentId = vm.MainCommentId,
                    Created = DateTime.Now,
                    Message = vm.Message
                };
                _repo.AddSubComment(comment);
            }
            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }
    }
}