using System.Collections;
using UnityEngine;

public class EndFacingState : BattleState {

    /* This state is used to wrap up the end of a turn, instead of 
     * simply choosing “Wait” from the ability menu. It allows you a 
     * chance to decide which direction you want a unit to face before 
     * giving control to the next unit.
     * In the future we will add some UI here of arrows over the 
     * active units head which indicate what you are supposed to 
     * be doing.*/

    private Directions startDir;

    public override void Enter()
    {
        base.Enter();
        startDir = turn.actor.dir;
        SelectTile(turn.actor.tile.pos);
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        turn.actor.dir = e.info.GetDirection();
        turn.actor.Match();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        switch (e.info)
        {
            case 0:
                owner.ChangeState<SelectUnitState>();
                break;
            case 1:
                turn.actor.dir = startDir;
                turn.actor.Match();
                owner.ChangeState<CommandSelectionState>();
                break;
        }
    }
}
