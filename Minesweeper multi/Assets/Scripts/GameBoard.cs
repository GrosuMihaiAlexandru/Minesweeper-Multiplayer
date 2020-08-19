using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    private Panel panelPrefab;

    private List<Panel> panels = new List<Panel>();

    private bool isFirstMove = true;

    void Start()
    {
        int id = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Panel panel = Instantiate(panelPrefab, new Vector3(j, 0, i), Quaternion.identity, transform);
                panel.Init(id, j, i);
                panel.OnPanelPressed += OnPanelPress;
                panels.Add(panel);
                id++;
            }
        }
    }

    public void OnPanelPress(int x, int y)
    {
        if (isFirstMove)
        {
            FirstMove(x, y);
        } else
        {
            
        }
    }

    public void FirstMove(int x, int y)
    {
        float depth = 0.125f * width;
        List<Panel> neighbors = GetNeighbors(x, y, (int)depth);
        neighbors.Add(GetPanel(x, y));

        IEnumerable<Panel> mineList = panels.Except(neighbors).OrderBy(user => Random.Range(0, 100000));
        var mineSlots = mineList.Take(mineCount).ToList().Select(z => new { z.X, z.Y });

        // Place the mines
        foreach (var mineCoord in mineSlots)
        {
            panels.Single(panel => panel.X == mineCoord.X && panel.Y == mineCoord.Y).IsMine = true;
        }

        // Set adjacent mines for every panel that is not a mine
        foreach (Panel openPanel in panels.Where(panel => !panel.IsMine))
        {
            List<Panel> nearbyPanels = GetNeighbors(openPanel.X, openPanel.Y);
            openPanel.AdjacentMines = nearbyPanels.Count(z => z.IsMine);
        }
    }    

    public Panel GetPanel(int x, int y)
    {
        return panels.Where(panel => panel.X == x && panel.Y == y).ToList()[0];
    }

    public List<Panel> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, 1);
    }

    public List<Panel> GetNeighbors(int x, int y, int depth)
    {
        IEnumerable<Panel> nearbyPanels = panels.Where(panel => panel.X >= (x - depth) && panel.X <= (x + depth) && 
                                        panel.Y >= (y - depth) && panel.Y <= (y + depth));
        IEnumerable<Panel> currentPanel = panels.Where(panel => panel.X == x && panel.Y == y);

        return nearbyPanels.Except(currentPanel).ToList();
    }

    public void RevealPanel(int x, int y)
    {
        Panel selectedPanel = panels.First(panel => panel.X == x && panel.Y == y);
        selectedPanel.IsRevealed = true;
        selectedPanel.IsFlagged = false;

        if (!selectedPanel.IsMine && selectedPanel.AdjacentMines == 0)
        {
            RevealZeros(x, y);

        }

        if (!selectedPanel.IsMine)
        {
            CompletionCheck();
        }
    }

    public void RevealZeros(int x, int y)
    {
        IEnumerable<Panel> neighborPanels = GetNeighbors(x, y).Where(panel => !panel.IsRevealed);
        foreach (var neighbor in neighborPanels)
        {
            neighbor.IsRevealed = true;
            if (neighbor.AdjacentMines == 0)
            {
                RevealZeros(neighbor.X, neighbor.Y);
            }
        }
    }

    private void CompletionCheck()
    {
        IEnumerable<int> hiddenPanels = panels.Where(x => !x.IsRevealed).Select(x => x.ID);
        IEnumerable<int> minePanels = panels.Where(x => x.IsMine).Select(x => x.ID);
        if (!hiddenPanels.Except(minePanels).Any())
        {
        
        }
    }

    public void FlagPanel(int x, int y)
    {
        Panel panel = panels.Where(z => z.X == x && z.Y == y).First();
        if (!panel.IsRevealed)
        {
            panel.IsFlagged = true;
        }
    }
}
