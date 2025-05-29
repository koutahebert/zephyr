using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class StateAttack : State
{

    public override void Execute(EnemyController character)
    {
        if (!character.IsCasting)
        {
            character.ChangeState(new StateBattleIdle());
        }
        else
        {
            character.BeCasting();
        }
    }
}

