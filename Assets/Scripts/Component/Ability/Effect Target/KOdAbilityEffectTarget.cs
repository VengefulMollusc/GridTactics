using System.Collections;
using UnityEngine;

public class KOdAbilityEffectTarget : AbilityEffectTarget {

    /* This time we are looking for an entity with no hit 
     * points. For example, a resurrection skill might 
     * require this target type.*/

    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.content == null)
            return false;

        Stats s = tile.content.GetComponent<Stats>();
        return s != null && s[StatTypes.HP] <= 0;
    }
}
