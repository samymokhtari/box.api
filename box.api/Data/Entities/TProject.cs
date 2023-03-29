using System;
using System.Collections.Generic;

namespace box.api.Data.Entities;

public partial class TProject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TFile> TFiles { get; } = new List<TFile>();
}
