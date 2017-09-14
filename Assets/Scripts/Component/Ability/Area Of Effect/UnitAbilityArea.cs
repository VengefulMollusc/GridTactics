using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityArea : AbilityArea {

    /* Our first concrete subclass is called UnitAbilityArea 
     * and it simply returns whatever Tile exists at the indicated 
     * position. This could be used for implementing the 
     * attack ability of an archer.*/

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        List<Tile> retValue = new List<Tile>();
        Tile tile = board.GetTile(pos);
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}
