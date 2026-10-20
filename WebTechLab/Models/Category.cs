namespace WebTechLab.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Навігаційна властивість
        public List<Product> Products { get; set; } = new();
    }
}