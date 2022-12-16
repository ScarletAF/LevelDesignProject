using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Orientation")]
    [SerializeField] private Quaternion referenceRotation;

    [Header("Walking")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accDuration;
    [SerializeField] private float decDuration;
    [SerializeField] private AnimationCurve accCurve;
    [SerializeField] private AnimationCurve decCurve;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float fallDuration;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private AnimationCurve fallCurve;

    private Rigidbody rigid;

    private Vector2 moveInput = Vector2.zero;

    private float moveTime;
    private Vector3 lastMovement;

    private float jumpTime;
    private bool jumping;
    private bool falling;
    private float groundPositionY;

    private bool decellerating;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        groundPositionY = transform.position.y;
        moveTime = 0;
        decellerating = false;
        lastMovement = new Vector3();
        //referenceRotation = transform.localRotation;
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }

    private void HandleRotation()
    {
        //if (IsMoving())
        //{
        //    float angle = Vector2.SignedAngle(Vector2.right, moveInput) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(Vector3.up * angle);
        //}
    }

    private void HandleMovement()
    {
        Vector3 jump;
        Vector3 movement;
        Vector3 currentPosition = rigid.position;

        jump = CalculateJumping();
        movement = CalculateMovement();
        currentPosition.y = groundPositionY;
        rigid.MovePosition(currentPosition + moveSpeed * Time.deltaTime * movement + jump);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        if (IsMoving())
        {
            decellerating = false;

            moveTime += Time.fixedDeltaTime;
            if (moveTime <= accDuration)
            {
                movement *= accCurve.Evaluate(moveTime / accDuration);
            }

            lastMovement = movement;
        }
        else if (!decellerating)
        {
            moveTime = decDuration;
            moveTime -= Time.fixedDeltaTime;
            movement = decCurve.Evaluate(moveTime / decDuration) * lastMovement;
            decellerating = true;
        }
        else if (moveTime > 0)
        {
            moveTime -= Time.fixedDeltaTime;
            movement =  decCurve.Evaluate(moveTime / decDuration) * lastMovement;
        }

        return referenceRotation * movement;
    }

    private Vector3 CalculateJumping()
    {
        Vector3 jump = new Vector3(0, 0, 0);
        if (jumping)
        {
            jumpTime += Time.fixedDeltaTime;
            jump.y = Mathf.Lerp(0f, jumpHeight, jumpCurve.Evaluate(jumpTime / jumpDuration));

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
            jump.y = Mathf.Lerp(jumpHeight, 0f, fallCurve.Evaluate(jumpTime / fallDuration));

            if (jumpTime >= fallDuration)
            {
                falling = false;
            }
        }

        return jump;
    }

    private void SetJumping()
    {
        if(!jumping && !falling)
        {
            jumping = true;
            jumpTime = 0;
        }
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        SetJumping();
    }
}
