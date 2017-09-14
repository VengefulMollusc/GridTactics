using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifyAbilityArea : AbilityArea
{
    /* When you want to target an area of tiles around the cursor’s position, 
     * you will use our next subclass, SpecifyAbilityArea. For example, this 
     * would be used to implement a black mage’s fire spell which can also 
     * hit tiles adjacent to the targeted location.*/

    public int horizontal;
    public int vertical;
    private Tile tile;

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        tile = board.GetTile(pos);
        return board.Search(tile, ExpandSearch);
    }

    bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= horizontal && Mathf.Abs(to.height - tile.height) <= vertical;
    }
}
