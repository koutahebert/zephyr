using UnityEngine;
using System.Collections;

public class MoveToPosition : MonoBehaviour
{
	private Vector3 target;
    private Vector3 lookAtTarget;

    private float speed = 2f;

    public void SetTarget(Vector3 newTarget, Vector3 newLookAtTarget)
    {
        target = newTarget;
        lookAtTarget = newLookAtTarget;
    }

    // Update is called once per frame
    void Update()
	{
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(lookAtTarget);
	}
}

