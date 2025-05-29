using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;

public abstract class State
{
    public abstract void Execute(EnemyController character);
}
