using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuPanelController : MonoBehaviour
{
    private const string ShowKey = "Show";
    private const string HideKey = "Hide";
    private const string EntryPoolKey = "AbilityMenuPanel.Entry";
    private const int MenuCount = 4;

    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Text titleLabel;
    [SerializeField] private Panel panel;
    [SerializeField] private GameObject canvas;
    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    public int selection { get; private set; }

    // configure pool manager to generate menu entries
    void Awake()
    {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    // get menu entry from pool manager
    AbilityMenuEntry Dequeue()
    {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);
        entry.Reset();
        return entry;
    }

    // return entry to pool manager
    void Enqueue(AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    // when clearing the menu, make sure to enqueue each entry
    void Clear()
    {
        for (int i = menuEntries.Count - 1; i >= 0; --i)
            Enqueue(menuEntries[i]);
        menuEntries.Clear();
    }

    // make sure to hide the panel and disable the canvas on start
    void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false);
    }

    // method for specifying consistent menu easing/tweening duration
    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    /*The menu itself will always highlight a single entry as Selected. 
     * Since entries can be locked, we will need to know whether or not 
     * we are allowed to select any given entry. If we can’t select an 
     * entry, then we will need to try to select something else instead.*/
    bool SetSelection(int value)
    {
        if (menuEntries[value].IsLocked)
            return false;

        // deselect previously selected entry
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = false;

        selection = value;

        // select the new entry
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = true;

        return true;
    }

    /*Menu cycling public methods
     * step through selection options to take next entry that isnt locked
     * Loop on reaching end of menu*/
    public void Next()
    {
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    public void Previous()
    {
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    /*To initially load and display the menu, I’ve exposed a method 
     * called Show where you pass along the title to display in the 
     * header, as well as a list of string which are the text to 
     * show for each entry in the menu.*/
    public void Show(string title, List<string> options)
    {
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for (int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }
        SetSelection(0);
        TogglePos(ShowKey);
    }

    // lock/unlock a given menu option
    public void SetLocked(int index, bool value)
    {
        if (index < 0 || index >= menuEntries.Count)
            return;

        menuEntries[index].IsLocked = value;
        if (value && selection == index)
            Next();
    }

    /*When the user confirms a menu selection 
     * (using the Fire1 input) then we can dismiss the panel.*/
    public void Hide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate(object sender, System.EventArgs e)
        {
            if (panel.CurrentPosition == panel[HideKey])
            {
                Clear();
                canvas.SetActive(false);
            }
        };
    }

}
