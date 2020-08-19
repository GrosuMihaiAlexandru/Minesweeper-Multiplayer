using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsMine { get; set; }
    public int AdjacentMines { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }

    public void Init(int id, int x, int y)
    {
        this.ID = id;
        this.X = x;
        this.Y = y;
    }

    public event Action<int, int> OnPanelPressed;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPanelPressed(X, Y);
            Debug.Log (X + " " + Y);
        }
    }
}