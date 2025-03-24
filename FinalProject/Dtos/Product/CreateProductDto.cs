using System.ComponentModel.DataAnnotations;

namespace FinalProject.Dtos.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün adı boş olamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ürün açıklaması boş olamaz")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Ürün fiyatı boş olamaz")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Ürün fotoğrafı boş olamaz")]
        public IFormFile Image { get; set; }
        //public string CategoryId { get; set; }

    }
}
