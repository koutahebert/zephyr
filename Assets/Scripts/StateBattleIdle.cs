using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class StateBattleIdle : State
{

    public override void Execute(EnemyController character)
    {
        if (character.IsDead)
        {
            character.ChangeState(new StateDead());
        }
        else if (character.IsHit)
        {
            character.ChangeState(new StateHit());
        }
        else if (character.IsCasting)
        {
            character.ChangeState(new StateAttack());
        }
        else
        {
            character.BeBattleIdle();
        }
    }
}

