using FoodMarket.Models.Comments;
using System;
using System.Collections.Generic;

namespace FoodMarket.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Image { get; set; } = "";

        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public int Price { get; set;} = 0;
        public int Stock { get; set;} = 0;
        public DateTime Created { get; set; } = DateTime.Now;

        public List<MainComment> MainComments { get; set; }
    }
}