using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class StateHit : State
{

    public override void Execute(EnemyController character)
    {
        if (!character.IsHit)
        {
            character.ChangeState(new StateBattleIdle());
        }
        else
        {
            character.BeHit();
        }
    }
}

