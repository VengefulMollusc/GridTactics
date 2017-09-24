using System.Collections;
using UnityEngine;

public class AbilityMagicCost : MonoBehaviour
{

    public int amount;
    private Ability owner;

    #region Monobehaviour

    void Awake()
    {
        owner = GetComponent<Ability>();
    }

    void OnEnable()
    {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    #endregion

    #region Event Handlers

    void OnCanPerformCheck(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatTypes.MP] < amount)
        {
            BaseException exc = (BaseException) args;
            exc.FlipToggle();
        }
    }

    void OnDidPerformNotification(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.MP] -= amount;
    }

    #endregion
}
