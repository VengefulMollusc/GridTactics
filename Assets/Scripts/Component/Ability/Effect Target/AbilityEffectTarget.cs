using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffectTarget : MonoBehaviour
{
    /* an ability can have more than one effect. For example, 
     * a Cure spell which normally restores hit points to 
     * units might have a secondary effect of damaging the 
     * undead. So in this example, the first effect of the 
     * ability would only target living units, and the 
     * second effect would only target undead units. Even 
     * if the ability only has a single effect, it still 
     * may require special targeting. The ability to 
     * determine what is and is not a valid target will 
     * be another component.*/

    public abstract bool IsTarget(Tile tile);

    /* The only thing this component does is to determine 
     * whether or not the effect applies to whatever may 
     * or may not be located at the specified board tile.*/
}
