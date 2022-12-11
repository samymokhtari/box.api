namespace box.application.Models
{
    public class BoxFile
    {
        public int Id { get; set; }

        public string Filename { get; set; } = null!;

        public int ProjectId { get; set; }

        public byte? IsActive { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}