using System.Collections;
using UnityEngine;

public class DefaultAbilityEffectTarget : AbilityEffectTarget {

    /* Let’s create our first concrete subclass called DefaultAbilityEffectTarget. 
     * Most ability effects will probably use this – it simply requires that 
     * there be something on the tile, and that the something which is there 
     * has hit points. Note that it doesn’t necessarily have to be a normal
     * unit – you may or may not wish to include that requirement.*/

    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.content == null)
            return false;

        Stats s = tile.content.GetComponent<Stats>();
        return s != null && s[StatTypes.HP] > 0;
    }
}
