using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class StateDead : State
{

    public override void Execute(EnemyController character)
    {
        character.BeDead();
    }
}
