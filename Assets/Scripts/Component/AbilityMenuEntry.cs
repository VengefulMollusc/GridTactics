using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuEntry : MonoBehaviour {

    [SerializeField] Image bullet;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite disabledSprite;
    [SerializeField] Text label;
    Outline outline;

    private void Awake()
    {
        outline = label.GetComponent<Outline>();
    }

    public string Title
    {
        get { return label.text; }
        set { label.text = value; }
    }

    // Flags the state of the menu item
    [System.Flags]
    enum States
    {
        None = 0,
        Selected = 1 << 0,
        Locked = 1 << 1
    }

    public bool IsLocked
    {
        get { return (State & States.Locked) != States.None; }
        set
        {
            if (value)
                State |= States.Selected;
            else
                State &= ~States.Selected;
        }
    }

}
