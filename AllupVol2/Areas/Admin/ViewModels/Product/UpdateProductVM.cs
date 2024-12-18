using AllupVol2.Models;
using System.ComponentModel.DataAnnotations;

namespace AllupVol2.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        public string Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public bool Availability { get; set; }
        public string Title { get; set; }
        [Required]
        public decimal? Tax { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [Required]
        public double? DisCountPercentage { get; set; }
        [Required(ErrorMessage = "Category daxil et")]
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
