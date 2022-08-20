using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DotNetCrud_5.Models
{
    [Table("Catogray")]
    public class Genre
    {
        //Make A cloumn Auto Incremnt When You Use Byte Type ==> 
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required,MaxLength(100,ErrorMessage ="Max Len 100 Char")]
        public string Name { get; set; }
    }
}
