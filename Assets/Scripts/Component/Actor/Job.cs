using System.Collections;
using UnityEngine;

public class Job : MonoBehaviour {

	public static readonly StatTypes[] statOrder = new StatTypes[]
	{
	    StatTypes.MHP, 
        StatTypes.MMP,
        StatTypes.ATK, 
        StatTypes.DEF, 
        StatTypes.MAT, 
        StatTypes.MDF, 
        StatTypes.SPD
	};

    public int[] baseStats = new int[statOrder.Length];
    public float[] growStats = new float[statOrder.Length];
    private Stats stats;

    void OnDestroy()
    {
        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL));
    }

    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
        this.AddObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL), stats);

        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Activate(gameObject);
    }

    public void UnEmploy()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Deactivate();

        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StatTypes.LVL), stats);
        stats = null;
    }

    public void LoadDefaultStats()
    {
        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            stats.SetValue(type, baseStats[i], false);
        }

        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
    }

    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int) args;
        int newValue = stats[StatTypes.LVL];

        for (int i = oldValue; i < newValue; ++i)
            LevelUp();
    }

    void LevelUp()
    {
        for (int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            int whole = Mathf.FloorToInt(growStats[i]);
            float fraction = growStats[i] - whole;

            int value = stats[type];
            value += whole;
            if (UnityEngine.Random.value > (1f - fraction))
                value++;

            stats.SetValue(type, value, false);
        }

        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
    }

    /*First, I declared an array of StatTypes called statOrder – this will serve as a convenience 
     * array to help me parse data from the spreadsheets we created earlier. It is static because 
     * it wont change from job to job and this way they can all share.
     * Next I defined two instance arrays, one for holding the starting stat values, and one for 
     * holding the grow stat values. I was able to init them with a length equal to the length of 
     * the statOrder array from earlier. I might have decided to implement these as a Dictionary, 
     * but because Unity doesn’t serialize Dictionaries I decided to keep it as an Array.
     * There are three public methods. First is Employ which should be called after instantiating 
     * a job and attaching it to an actor’s hierarchy. In this method, we get a reference to the 
     * actor’s Stats component so that we can listen to targeted level up notifications as well 
     * as apply growth to the other stats in response. In addition, this method will allow any 
     * job-based feature to become active.
     * If you want to switch jobs, you should first UnEmploy any currently active Job. This 
     * gives the script a chance to deactivate its features and unregister from level up 
     * notifications etc.
     * When creating a unit for the first time, call LoadDefaultstats so that its stats 
     * will be initiated to playable values.*/
}
