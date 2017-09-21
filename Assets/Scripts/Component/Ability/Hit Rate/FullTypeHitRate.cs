using System.Collections;
using UnityEngine;

public class FullTypeHitRate : HitRate {
    public override int Calculate(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();
        if (AutomaticMiss(defender))
            return Final(100);

        return Final(0);
    }
}
