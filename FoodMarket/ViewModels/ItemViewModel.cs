using Microsoft.AspNetCore.Http;

namespace FoodMarket.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public int Price { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string CurrentImage { get; set; } = "";
        public IFormFile Image { get; set; } = null;
    }
}