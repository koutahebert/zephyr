using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
	private float outOfCombatX = 0.86f;
	private float outOfCombatY = 1.49f;
	private float outOfCombatZ = -2.28f;

	private float combatX = -2f;
	private float combatY = 8f;
	private float combatZ = 1f;

	public float speed = 12f;

    private bool rotating = false;

    private Vector3 target;
    private Quaternion targetRotation;

    private void Start()
    {
        target = new Vector3(outOfCombatX, outOfCombatY, outOfCombatZ);
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SwapCamera(bool isInCombat)
    {
        if (isInCombat)
        {
            target.x = combatX;
            target.y = combatY;
            target.z = combatZ;
            targetRotation = Quaternion.Euler(70, 45, 0);
        }
        else
        {
            target.x = outOfCombatX;
            target.y = outOfCombatY;
            target.z = outOfCombatZ;
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator RotateCamera()
    {
        rotating = true;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, t);
            yield return null;
        }
        rotating = false;
    }

    // Update is called once per frame
    void Update()
	{
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
        if (!rotating)
        {
            StartCoroutine(RotateCamera());
        }
    }
}

