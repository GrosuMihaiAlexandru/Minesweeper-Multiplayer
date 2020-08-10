using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private int mineCount;
    [SerializeField]
    private List<PanelContainer> panels;

    [SerializeField]
    private PanelContainer panelPrefab;

    private bool isFirstMove;

    // Start is called before the first frame update
    void Start()
    {
        isFirstMove = true;

        panels = new List<PanelContainer>();

        int id = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                PanelContainer panelContainer = Instantiate(panelPrefab, new Vector2(j, i), Quaternion.identity);
                panelContainer.Init(id, j, i);
                panelContainer.transform.parent = transform;
                panelContainer.OnPanelPressed += OnPanelPress;
                panels.Add(panelContainer);
                id++;
            }
        }
    }

    public void OnPanelPress(int x, int y)
    {
        if (isFirstMove)
        {
            FirstMove(x, y);
        }
    }

    public void FirstMove(int x, int y)
    {
        float depth = 0.125f * width;
        List<PanelContainer> neighbors = GetNeighbors(x, y, (int)depth);
        neighbors.Add(GetPanel(x, y));

        IEnumerable<PanelContainer> mineList = panels.Except(neighbors).OrderBy(user => Random.Range(0, 100000));
        var mineSlots = mineList.Take(mineCount).ToList().Select(z => new { z.panel.X, z.panel.Y });

        // Place the mines
        foreach (var mineCoord in mineSlots)
        {
            panels.Single(panel => panel.panel.X == mineCoord.X && panel.panel.Y == mineCoord.Y).panel.IsMine = true;
        }

        // Set adjacent mines for every panel that is not a mine
        foreach (PanelContainer openPanel in panels.Where(panel => !panel.panel.IsMine))
        {
            List<PanelContainer> nearbyPanels = GetNeighbors(openPanel.panel.X, openPanel.panel.Y);
            openPanel.panel.AdjacentMines = nearbyPanels.Count(z => z.panel.IsMine);
        }
    }    

    public PanelContainer GetPanel(int x, int y)
    {
        return panels.Where(panel => panel.panel.X == x && panel.panel.Y == y).ToList()[0];
    }

    public List<PanelContainer> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, 1);
    }

    public List<PanelContainer> GetNeighbors(int x, int y, int depth)
    {
        var nearbyPanels = panels.Where(panel => panel.panel.X >= (x - depth) && panel.panel.X <= (x + depth) && 
                                        panel.panel.Y >= (y - depth) && panel.panel.Y <= (y + depth));
        var currentPanel = panels.Where(panel => panel.panel.X == x && panel.panel.Y == y);

        return nearbyPanels.Except(currentPanel).ToList();
    }

    public void RevealPanel(int x, int y)
    {
        PanelContainer selectedPanel = panels.First(panel => panel.panel.X == x && panel.panel.Y == y);
        selectedPanel.panel.IsRevealed = true;
        selectedPanel.panel.IsFlagged = false;
    }
}
