namespace box.infrastructure.Data.Entities;

public partial class TProject : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TFile> TFiles { get; } = new List<TFile>();
}