using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState {

    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits(); // temp for demo
        yield return null;
        //owner.ChangeState<MoveTargetState>();
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();

        owner.ChangeState<CutSceneState>(); // SelectUnitState
    }

    void SpawnTestUnits()
    {
        string[] recipes = new string[]
        {
            "Craig",
            "Wallace",
            "Berthel",
            "Enemy Rogue",
            "Enemy Worrier",
            "Enemy Lizard"
        };

        List<Tile> locations = new List<Tile>(board.tiles.Values);
        for (int i = 0; i < recipes.Length; ++i)
        {
            int level = UnityEngine.Random.Range(9, 12);
            GameObject instance = UnitFactory.Create(recipes[i], level);

            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            unit.dir = (Directions) UnityEngine.Random.Range(0, 4);
            unit.Match();

            units.Add(unit);
        }

        SelectTile(units[0].tile.pos);
    }
}
