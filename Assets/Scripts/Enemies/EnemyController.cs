using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    enum State  {idle, charge, jump}

    private float originY;
    private Rigidbody rb;

    private State aiState = State.charge;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private float jumpHeight;

    private void Start()
    {
        originY = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 playerPos = GameManager.PredictPlayerMovement();

        switch (aiState)
        {
            case State.idle:
                break;
            case State.charge:
                rb.MovePosition(transform.position + Time.deltaTime * moveSpeed * (playerPos - transform.position));

                if (Vector3.Distance(playerPos, transform.position) < jumpDistance)
                {
                    aiState = State.jump;
                }
                break;
            case State.jump:
                break;
        }
    }

    public void Die()
    {
        EnemyManager.ReportDeath(this);
    }
}
