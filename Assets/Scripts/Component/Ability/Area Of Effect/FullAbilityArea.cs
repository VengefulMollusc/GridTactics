using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAbilityArea : AbilityArea {

    /* Our next concrete subclass is called FullAbilityArea, 
     * and as the name implies, every tile that is highlighted 
     * by an ability’s range is also a potential target for 
     * this area of effect. This could be used for a 
     * dragoon’s fire breath attack.*/

    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        AbilityRange ar = GetComponent<AbilityRange>();
        return ar.GetTilesInRange(board);
    }
}
