using System;
using System.Collections.Generic;
using System.Text;

namespace PipAndIvory.Domain.Entities;

public class Player : BaseAuditableEntity
{
    public string? Name { get; set; }

    public int BlockGamesPlayed { get; set; }

    public int BlockGamesWon { get; set; }

    public int HighestBlockScore { get; set; }

    public int DrawGamesPlayed { get; set; }

    public int DrawGamesWon { get; set; }

    public int HighestDrawScore { get; set; }
}
