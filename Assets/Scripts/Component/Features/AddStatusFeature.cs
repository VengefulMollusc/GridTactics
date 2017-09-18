using System.Collections;
using UnityEngine;

public abstract class AddStatusFeature<T> : Feature where T : StatusEffect
{
    private StatusCondition statusCondition;

    protected override void OnApply()
    {
        Status status = GetComponentInParent<Status>();
        statusCondition = status.Add<T, StatusCondition>();
    }

    protected override void OnRemove()
    {
        if (statusCondition != null)
            statusCondition.Remove();
    }
}
