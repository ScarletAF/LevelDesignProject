using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    [Tooltip("How far the fist goes.")]
    [SerializeField]
    private float punchMaxDistance;

    [Tooltip("How long the swing lasts.")]
    [SerializeField]
    private float punchTimeMax;

    [SerializeField]
    private AnimationCurve outCurve;
    [SerializeField]
    private AnimationCurve inCurve;

    private float punchTime;
    private bool isPunching;

    private float attackHitTime;

    private void Start()
    {
        punchTime = 0;
        attackHitTime = punchTimeMax / 2;
    }

    public void Punch()
    {
        if (!isPunching)
        {
            isPunching = true;
            StartCoroutine(SlideGameObject(transform.forward));
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (isPunching && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Die();
            Debug.Log("Pucnh hit");
        }
        else if (isPunching && other.CompareTag("Invis"))
        {
            other.GetComponent<InvisibleDoor>().Appear();
        }
    }

    private void MoveFist(Vector3 direction)
    {
        punchTime += Time.deltaTime;
        Vector3 movement = new Vector3();

        if (punchTime < attackHitTime)
        {
            movement =  punchMaxDistance * outCurve.Evaluate(punchTime / attackHitTime) * direction;
        }
        else if (punchTime < punchTimeMax)
        {
            float returnTime = punchTime - attackHitTime;
            float returnMax = punchTimeMax - attackHitTime;
            movement = punchMaxDistance * outCurve.Evaluate(returnTime / returnMax) * -direction;
        }

        transform.Translate(movement * Time.deltaTime, transform.parent);
    }

    private IEnumerator SlideGameObject(Vector3 direction)
    {
        while (isPunching)
        {
            MoveFist(direction);
            if (punchTime > punchTimeMax)
            {
                isPunching = false;
                punchTime = 0;
            }
            yield return null;
        }
    }
}
