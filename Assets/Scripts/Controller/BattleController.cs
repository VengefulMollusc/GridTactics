using System.Collections;
using UnityEngine;

public class BattleController : StateMachine {

    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    // This is prototype code which I expect not to keep,
    // so I wont bother wrapping it in the BaseBattleState. 
    // A more complete implementation for spawning our characters 
    // would load the correct models through a Resources.Load call.
    public GameObject heroPrefab;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
