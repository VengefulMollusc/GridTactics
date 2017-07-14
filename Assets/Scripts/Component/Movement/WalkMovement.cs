using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement {

    protected override bool ExpandSearch(Tile from, Tile to)
    {
        // skip if distance in height is greater than jump height
        if (Mathf.Abs(from.height - to.height) > jumpHeight)
            return false;

        // skip if tile is occupied by other content
        if (to.content != null)
            return false;

        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);

        // build a list of way points from unit's starting tile to destination tile
        List<Tile> targets = new List<Tile>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
        }

        // move to each way point in succession
        for (int i = 1; i < targets.Count; ++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];

            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
                yield return StartCoroutine(Turn(dir));

            if (from.height == to.height)
                yield return StartCoroutine(Walk(to));
            else
                yield return StartCoroutine(Jump(to));
        }
    }
}
