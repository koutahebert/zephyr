using UnityEngine;
using System.Collections;

public class MobController : EnemyController
{
	// Use this for initialization
	void Start()
	{
		status = GetComponent<MobStatus>();
		playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();

		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animation>();

		moveScript = GetComponent<MoveToPosition>();
		moveScript.enabled = false;

		ChangeState(new StateIdle());
	}

    IEnumerator StartBattleCoroutine()
	{
		yield return new WaitForSeconds(2f);
		moveScript.enabled = false;
		IsMoving = false;
	}

	public override void StartBattle(Vector3 playerMagicCircle, Vector3 enemyMagicCircle)
	{
		IsInBattle = true;
		IsMoving = true;

		moveScript.enabled = true;
		moveScript.SetTarget(enemyMagicCircle, playerMagicCircle);
		StartCoroutine(StartBattleCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
		if (IsInBattle)
		{
			// do things
		}

		// execute animation
        GetState.Execute(this);
    }
}

