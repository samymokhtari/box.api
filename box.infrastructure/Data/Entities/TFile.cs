﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace box.infrastructure.Data.Entities;

public partial class TFile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Filename { get; set; } = null!;

    public int ProjectId { get; set; }

    public byte IsActive { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public virtual TProject Project { get; set; } = null!;
}