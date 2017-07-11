using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour {

    [SerializeField]
    private GameObject tileViewPrefab;

    [SerializeField]
    private GameObject tileSelectionIndicatorPrefab;

    // Selection indicator instantiator (Lazy Loading)
    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;

    // Point/Tile Dictionary
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 8;

    // Game board modification position
    [SerializeField] Point pos;

    // Allows loading previously saved levels
    [SerializeField]
    LevelData levelData;

    // Methods for randomising levels
    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    // Creates a random rectangle defined by the w/d/h constraints
    Rect RandomRect()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, depth);
        int w = Random.Range(1, width - x + 1);
        int h = Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    // Methods for growing and shrinking an area of the game board
    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    // grow tile at given point - create if non existant
    void GrowSingle (Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
        {
            t.Grow();
        }
    }

    // shrink tile if exists at point - destroy if shrinking reduces height to 0
    void ShrinkSingle (Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    // get or create tiles to grow/shrink
    Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    Tile GetOrCreate (Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    // Methods to grow or shrink tile defined by Point pos field
    public void Grow()
    {
        GrowSingle(pos);
    }

    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    // update position of tile selection indicator
    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    // Clears board and resets state
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    // methods for saving levels
    // stores each tiles position and height in V3 form
    public void Save ()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);

        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    void CreateSaveDirectory ()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");

        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    // Load a previously saved editor
    public void Load()
    {
        Clear();
        if (levelData == null)
            return;

        foreach(Vector3 v in levelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }
}
