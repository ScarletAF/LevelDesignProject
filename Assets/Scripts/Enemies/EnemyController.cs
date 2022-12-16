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
    private float jumpDistance;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float fallDuration;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private AnimationCurve fallCurve;

    private float jumpTime;
    private bool jumping;
    private bool falling;
    private bool waiting;
    private Rigidbody rb;
    private float groundPositionY;
    private Vector3 lastDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        waiting = false;
        groundPositionY = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector3 direction;
        Vector3 playerPos = GameManager.GetPlayerTransform().position;
        Vector3 predictedPos = GameManager.PredictPlayerMovement();

        switch (aiState)
        {
            case State.idle:
                if (!waiting)
                {
                    StartCoroutine(ChargeAfterDelay(jumpCooldown));
                }
                break;
            case State.charge:
                direction = (playerPos - rb.position);
                rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * direction.normalized);

                if (Vector3.Distance(playerPos, transform.position) < jumpDistance)
                {
                    aiState = State.jump;
                    SetJumping();

                    lastDirection = (predictedPos - rb.position);
                }

                break;
            case State.jump:
                Vector3 jump = CalculateJumping();
                Vector3 movement = lastDirection.normalized;
                Vector3 currentPosition = rb.position;
                currentPosition.y = groundPositionY;

                rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * movement + jump);
                break;
        }
    }

    public void Die()
    {
        EnemyManager.ReportDeath(this);
    }

    private void SetJumping()
    {
        if (!jumping && !falling)
        {
            Debug.Log("start jump");
            jumping = true;
            jumpTime = 0;
        }
    }

    private Vector3 CalculateJumping()
    {
        Vector3 jump = new Vector3(0, 0, 0);
        if (jumping)
        {
            jumpTime += Time.fixedDeltaTime;
            jump.y = Mathf.Lerp(0f, jumpHeight, jumpTime / jumpDuration);

            if (jumpTime >= jumpDuration)
            {
                jumping = false;
                jumpTime = 0;
                falling = true;
            }
        }
        else if (falling)
        {
            jumpTime += Time.fixedDeltaTime;
            jump.y = Mathf.Lerp(jumpHeight, 0f, jumpTime / fallDuration);

            if (jumpTime >= fallDuration)
            {
                falling = false;
            }
        }

        return jump;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (aiState == State.jump && collision.gameObject.CompareTag("Ground"))
        {
            aiState = State.idle;
        }
        else if (aiState == State.jump && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerState>().Respawn();
        }
    }

    private IEnumerator ChargeAfterDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        aiState = State.charge;
        waiting = false;
    }
}
