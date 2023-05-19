using FoodMarket.Helpers;
using FoodMarket.Models;
using FoodMarket.Models.Comments;
using FoodMarket.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void AddItem(Item post)
        {
            _ctx.Items.Add(post);
        }

        public async Task<List<Item>> GetAllIItems()
        {
            return await _ctx.Items.ToListAsync();
        }

        //public async Task<IndexViewModel> GetAllPosts(int pageNumber)
        //{
        //    int pageSize = 5;
        //    int skipAmount = pageSize * (pageNumber - 1);

        //    return new IndexViewModel()
        //    {
        //        PageNumber = pageNumber,
        //        NextPage = _ctx.Posts.Count() > skipAmount + pageSize,
        //        Posts = await _ctx.Posts
        //            .Skip(skipAmount)
        //            .Take(pageSize)
        //            .ToListAsync()
        //    };
        //}

        public async Task<IndexViewModel> GetAllItems(int pageNumber, string category)
        {
            int pageSize = 2;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Items.AsQueryable();

            if (!String.IsNullOrEmpty(category))
                query = query.Where(x => x.Category.Equals(category));

            int itemCount = query.Count();
            int pageCount = (int)Math.Ceiling((double)itemCount / pageSize);

            return new IndexViewModel()
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                Category = category,
                NextPage = itemCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(), // not adding tolist will make the IEnumberable method calculate again
                Items = await query
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync()
            };
        }

        //public async Task<IndexViewModel> GetAllPosts(int pageNumber, string category)
        //{
        //    int pageSize = 5;
        //    int skipAmount = pageSize * (pageNumber - 1);

        //    return new IndexViewModel()
        //    {
        //        PageNumber = pageNumber,
        //        Category = category,
        //        NextPage = _ctx.Posts.Where(p => p.Category.Equals(category)).Count() > skipAmount + pageSize,
        //        Posts = await _ctx.Posts
        //            .Where(p => p.Category.Equals(category))
        //            .Skip(skipAmount)
        //            .Take(pageSize)
        //            .ToListAsync()
        //    };
        //}

        //public async Task<List<Post>> GetAllPosts(string category)
        //{
        //    return await _ctx.Posts
        //        .Where(post => post.Category.Equals(category))
        //        .ToListAsync();
        //    // This words but is case insensitive

        //    // asenumerable or await linq.tolistasync or expression<func<>> for client side eval
        //    // TODO why does expression still give translation error and not allow client side eval

        //    //Expression<Func<Post, bool>> InCategoryExpr = post => post.Category.Equals(category);
        //    //return await _ctx.Posts
        //    //    .Where(InCategoryExpr)
        //    //    .ToListAsync();
        //    // This works but is case insensitive
        //}

        public Item GetItem(int id)
        {
            return _ctx.Items
                .Include(p => p.MainComments)
                .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        public void RemoveItem(int id)
        {
            _ctx.Items.Remove(GetItem(id));
        }

        public void UpdateItem(Item post)
        {
            _ctx.Items.Update(post);
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}