namespace Pronia.Models
{
    public class Slide
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string SubTitle { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string ButtonText { get; set; } = null!;
        public int Order { get; set; }

    }
}
