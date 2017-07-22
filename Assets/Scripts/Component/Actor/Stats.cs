using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

	public int this[StatTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    int[] _data = new int[ (int)StatTypes.Count ];


    /*One job of the setter will be to post notifications 
     * that we will be changing a stat (in case listeners 
     * want a chance to make some sort of exception) and 
     * another notification will be posted after we did change 
     * a stat (in case listeners want a chance to respond 
     * based on the change). For example, after incrementing 
     * the experience stat, a listener might also decide 
     * modify the level stat to match. After incrementing 
     * the level stat, a variety of other stats might change
     * such as attack or defense. 
     * The notifications for each stat are built dynamically
     * and stored statically by the class. This way both the
     * listeners and the component itself can continually reuse
     * a string instead of constantly needing to recreate it.*/
    static Dictionary<StatTypes, string> _willChangeNotifications = new Dictionary<StatTypes, string>();
    static Dictionary<StatTypes, string> _didChangeNotifications = new Dictionary<StatTypes, string>();

    public static string WillChangeNotification (StatTypes type)
    {
        if (!_willChangeNotifications.ContainsKey(type))
            _willChangeNotifications.Add(type, string.Format("Stats.{0}WillChange", type.ToString()));
        return _willChangeNotifications[type];
    }

    public static string DidChangeNotification(StatTypes type)
    {
        if (!_didChangeNotifications.ContainsKey(type))
            _didChangeNotifications.Add(type, string.Format("Stats.{0}DidChange", type.ToString()));
        return _didChangeNotifications[type];
    }

    /*The first thing our setter checks is whether or not there 
     * are any changes to the value. If not we just exit early. 
     * If exceptions are allowed we will create a 
     * ValueChangeException and post it along with our will 
     * change notification. If the value does change, we assign 
     * the new value in the array and post a notification that 
     * the stat value actually changed.*/
     public void SetValue (StatTypes type, int value, bool allowExceptions)
    {
        int oldValue = this[type];
        if (oldValue == value)
            return;

        if (allowExceptions)
        {
            // Allow exceptions to the rule here
            ValueChangeException exc = new ValueChangeException(oldValue, value);

            // The notification is unique per stat type
            this.PostNotification(WillChangeNotification(type), exc);

            // Did anything modify the value?
            value = Mathf.FloorToInt(exc.GetModifiedValue());

            // Did something nullify the change?
            if (exc.toggle == false || value == oldValue)
                return;
        }

        _data[(int)type] = value;
        this.PostNotification(DidChangeNotification(type), oldValue);
    }
}
