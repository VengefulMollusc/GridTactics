using System.Collections;
using UnityEngine;

public class SelectUnitState : BattleState {

    // old implementation - allows selection via cursor

    //protected override void OnMove(object sender, InfoEventArgs<Point> e)
    //{
    //    SelectTile(e.info + pos);
    //}

    //protected override void OnFire(object sender, InfoEventArgs<int> e)
    //{
    //    GameObject content = owner.currentTile.content;
    //    if (content != null)
    //    {
    //        owner.currentUnit = content.GetComponent<Unit>();
    //        owner.ChangeState<MoveTargetState>();
    //    }
    //}

    int index = -1;

    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }

    IEnumerator ChangeCurrentUnit()
    {
        index = (index + 1) % units.Count;
        turn.Change(units[index]);
        RefreshPrimaryStatPanel(pos);
        yield return null;
        owner.ChangeState<CommandSelectionState>();
    }

    public override void Exit()
    {
        base.Exit();
        statPanelController.HidePrimary();
    }
}
