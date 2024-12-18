using AllupVol2.Models;

namespace AllupVol2.ViewModels
{
    public class GetProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
        public string CategoryName { get; set; }
        public double DisCountPercentage { get; set; }
        public string Description { get; set; } 
        public decimal DisCountPrice { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
