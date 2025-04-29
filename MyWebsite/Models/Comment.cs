using System.ComponentModel.DataAnnotations;

namespace MyWebsite.Models
{
    public class Comment
    {
        //Comment id
        public int Id { get; set; }

        //Id of the user to handle authification
        public string UserId { get; set; }

        //Name of the user
        [DataType(DataType.Text)]
        public string Name { get; set; }

        //Time of posting the comment
        [DataType(DataType.DateTime)]
        public DateTime Posted { get; set; }

        //Text content of the comment
        [DataType(DataType.Text)]
        public string CommentText { get; set; }
    }
}
