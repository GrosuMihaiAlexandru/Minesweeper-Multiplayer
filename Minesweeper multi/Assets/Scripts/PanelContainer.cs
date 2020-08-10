using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelContainer : MonoBehaviour
{
    public Panel panel;

    public event Action<int, int> OnPanelPressed;
    public void Init(int id, int x, int y)
    {
        panel = new Panel(id, x, y);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPanelPressed(panel.X, panel.Y);
        }
    }
}
