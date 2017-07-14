using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    // directions for pathfinding
    private Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);

    public void Load (LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    // Pathfinding scripts

    // returns a list of tiles reachable from the start tile using a given criteria function
    public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            
            // for tiles in each direction from t
            for (int i = 0; i < 4; ++i) // maybe should be i++?
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                // if next tile meets the criteria function
                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }

            // swap next for current if checkNow empty
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    // methods for selecting and deselecting tiles
    public void SelectTiles (List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }

    // returns a given tile if it exists
    public Tile GetTile (Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    // swaps references for checknow and checknext
    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    // clear tile pathfinding fields
    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }
}
