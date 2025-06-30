using System.ComponentModel.DataAnnotations;

namespace FinalProject.Dtos.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Ürün adı boş olamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ürün açıklaması boş olamaz")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Ürün fiyatı boş olamaz")]
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public Guid CategoryId { get; set; }
}
}
