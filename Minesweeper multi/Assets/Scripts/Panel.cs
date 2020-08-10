using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel
{
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsMine { get; set; }
    public int AdjacentMines { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    
    public Panel(int id, int x, int y)
    {
        this.ID = id;
        this.X = x;
        this.Y = y;
    }
}
