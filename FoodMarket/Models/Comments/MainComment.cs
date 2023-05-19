using System.Collections.Generic;

namespace FoodMarket.Models.Comments
{
    public class MainComment : Comment
    {
        public List<SubComment> SubComments { get; set; }
    }
}