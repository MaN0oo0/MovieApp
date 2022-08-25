using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DotNetCrud_5.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(250,ErrorMessage ="Max Len 250 Char")]
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        [Required,MaxLength(2500,ErrorMessage ="Max Len 2500")]
        public string StoryLine { get; set; }

        [Required]
        public byte[] Poster { get; set; }

        public byte GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }


}
