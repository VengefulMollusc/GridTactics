using UnityEngine;
using System.Collections;

public class StopStatusEffect : StatusEffect
{
    Stats myStats;
    void OnEnable()
    {
        this.AddObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);

        myStats = GetComponentInParent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        exc.FlipToggle();
    }

    void OnAutomaticHitCheck(object sender, object args)
    {
        Unit owner = GetComponentInParent<Unit>();
        MatchException exc = args as MatchException;
        if (owner == exc.target)
            exc.FlipToggle();
    }
}