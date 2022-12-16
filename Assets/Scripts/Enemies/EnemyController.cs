using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    enum State  {idle, charge, jump}

    private State aiState = State.charge;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private float jumpHeight;

    private float originY;
    private Rigidbody rb;
    private bool jumping;

    private float accDistance;
    private float accHeight;

    private void Start()
    {
        originY = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = GameManager.GetPlayerTransform().position;
        Vector3 predictedPos = GameManager.PredictPlayerMovement();

        switch (aiState)
        {
            case State.idle:
                break;
            case State.charge:
                Vector3 direction = (playerPos - rb.position);
                rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * direction.normalized);

                if (Vector3.Distance(playerPos, transform.position) < jumpDistance)
                {
                    aiState = State.jump;
                }
                break;
            case State.jump:
                if (!jumping)
                {
                    jumping = true;
                    accDistance = 0;
                    accHeight = 0;
                    StartCoroutine(DoJump());
                }
                break;
        }
    }

    public void Die()
    {
        EnemyManager.ReportDeath(this);
    }

    private IEnumerator DoJump()
    {
        while (accHeight < jumpHeight)
        {
            rb.MovePosition(transform.position + Time.deltaTime * jumpSpeed * (transform.up + transform.forward));
            yield return null;
        }
        while(accDistance < jumpDistance)
        {

            yield return null;
        }
        jumping = false;
    }
}
