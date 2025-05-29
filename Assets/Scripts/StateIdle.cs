using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class StateIdle : State
{

    public override void Execute(EnemyController character)
    {
        if (character.IsDead)
        {
            character.ChangeState(new StateDead());
        }
        else if (character.IsMoving)
        {
            character.ChangeState(new StateMove());
        }
        else
        {
            character.BeIdle();
        }
    }
}
