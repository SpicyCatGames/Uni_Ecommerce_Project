namespace FoodMarket.Models.Comments
{
    public class SubComment : Comment
    {
        // not necessary, as long as Maincomment has the list of subcomments, ef will auto create this id property
        public int MainCommentId { get; set; } 
    }
}
