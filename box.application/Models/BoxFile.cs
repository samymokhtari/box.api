namespace box.application.Models
{
    public class BoxFile : IEntity
    {
        public int Id { get; set; }

        public string Filename { get; set; } = null!;

        public int ProjectId { get; set; }

        public byte? IsActive { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public BoxFile(string filename, int projectId)
        {
            Filename = filename;
            ProjectId = projectId;
            IsActive = 1;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }
}