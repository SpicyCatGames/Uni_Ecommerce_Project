using FoodMarket.Models;
using FoodMarket.Models.Comments;
using FoodMarket.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.Data.Repository
{
    public interface IRepository
    {
        Item GetItem(int id);
        Task<List<Item>> GetAllIItems();
        /// <param name="pageNumber">index starts at 1</param>
        //Task<IndexViewModel> GetAllItems(int pageNumber);
        Task<IndexViewModel> GetAllItems(int pageNumber, string category);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void RemoveItem(int id);

        void AddSubComment(SubComment subComment);

        Task<bool> SaveChangesAsync();
    }
}