using System.Collections;
using UnityEngine;

public abstract class Feature : MonoBehaviour {

	protected GameObject _target { get; private set; }

    /*A feature can be activated for a time and then be deactivated. 
     * This would be the case with equipment – equip a sword for an 
     * attack boost but when you un-equip the sword your attack stat 
     * drops back down. I have exposed the methods Activate and 
     * Deactivate for this purpose.
     * A feature can have a one-shot (permanent) application. 
     * This would be the case when you consume a health potion – 
     * you get a boost to your hit point stat which doesn’t need 
     * to be un-done by the item. I have exposed the 
     * method Apply for this purpose.*/
    public void Activate (GameObject target)
    {
        if (_target == null)
        {
            _target = target;
            OnApply();
        }
    }

    public void Deactivate()
    {
        if (_target != null)
        {
            OnRemove();
            _target = null;
        }
    }

    public void Apply (GameObject target)
    {
        _target = target;
        OnApply();
        _target = null;
    }

    protected abstract void OnApply();
    protected virtual void OnRemove() { }
}
