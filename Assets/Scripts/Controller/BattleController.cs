using System.Collections;
using System.Collections.Generic;
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
    public Tile currentTile { get { return board.GetTile(pos); } }

    public AbilityMenuPanelController abilityMenuPanelController;
    public StatPanelController statPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
