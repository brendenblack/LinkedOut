using System.ComponentModel.DataAnnotations;

namespace LinkedOut.BlazorWasm.Models
{
    public class NoteFormModel
    {
        [Required]
        public string Contents { get; set; }
    }
}
