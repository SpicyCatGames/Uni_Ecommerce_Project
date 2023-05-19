using FoodMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMarket.ViewModels
{
    public class IndexViewModel
    {
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public List<Item> Items { get; set; }
        public String Category { get; set; }
        public IEnumerable<int> Pages { get; set; }
    }
}
