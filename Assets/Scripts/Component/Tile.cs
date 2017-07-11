using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public const float stepHeight = 0.25f;

    public Point pos;
    public int height;

    // gets the Vector3 location of the tile's center
    public Vector3 center {
        get { return new Vector3(pos.x, height * stepHeight, pos.y); }
    }

    // updates visual position of tile relative to Point coordinates
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    // methods to grow or shrink tile
    public void Grow()
    {
        height++;
        Match();
    }

    public void Shrink()
    {
        height--;
        Match();
    }

    // Load methods
    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }

    public void Load (Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
