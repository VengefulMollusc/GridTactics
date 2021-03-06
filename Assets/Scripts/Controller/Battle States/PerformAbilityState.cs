﻿using System.Collections;
using UnityEngine;

public class PerformAbilityState : BattleState {

    /*After having selected an ability and a target to apply the ability 
     * to, it is time to actually take action. There are a ton of ways 
     * this could be implemented, though Mechanim would probably be used 
     * since we are focusing on Unity. Ultimately we need some way to 
     * have events tied to animation so that we can do something like 
     * swing a sword, and then at a specific point in the animation, play 
     * a sound and apply the effect of the ability – which in that case 
     * would be to reduce the target’s hit points.
     * This state is sort of a placeholder – I left comments showing 
     * potential places for logic to appear. I also added a TemporaryAttackExample 
     * method. As the name hopefully implies, this is placeholder code. 
     * In a more complete project, I would not directly do the work of 
     * an Ability’s Effect in this state. Instead, there would be another 
     * class per effect, very much like we did with the Feature component 
     * of an item. The real implementation would probably loop through 
     * the effects and targets and attempt to apply the effect on each 
     * target.
     * When the animation and application of the ability are complete, 
     * we continue onto the next relevant state.*/

    public override void Enter()
    {
        base.Enter();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        // TODO play animations etc
        yield return null;
        // TODO apply ability effect etc
        ApplyAbility();

        if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else 
            owner.ChangeState<CommandSelectionState>();
    }

    private void ApplyAbility()
    {
        BaseAbilityEffect[] effects = turn.ability.GetComponentsInChildren<BaseAbilityEffect>();
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            Tile target = turn.targets[i];
            for (int j = 0; j < effects.Length; ++j)
            {
                BaseAbilityEffect effect = effects[j];
                AbilityEffectTarget targeter = effect.GetComponent<AbilityEffectTarget>();
                if (targeter.IsTarget(target))
                {
                    HitRate rate = effect.GetComponent<HitRate>();
                    int chance = rate.Calculate(target);
                    if (UnityEngine.Random.Range(0, 101) > chance)
                    {
                        // A Miss!
                        continue;
                    }
                    effect.Apply(target);
                }
            }
        }
    }
}
