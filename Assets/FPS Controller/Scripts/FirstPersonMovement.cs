using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9f;
    public Key runningKey = Key.LeftShift;

    /// <summary>
    /// Functions that temporarily override movement speed.
    /// The most recently added override is used.
    /// </summary>
    public List<Func<float>> speedOverrides = new List<Func<float>>();

    private Rigidbody playerRigidbody;
    private Vector2 movementInput;
    private bool runInput;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ReadInput()
    {
        if (Keyboard.current == null)
        {
            movementInput = Vector2.zero;
            runInput = false;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal -= 1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal += 1f;
        }

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            vertical -= 1f;
        }

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            vertical += 1f;
        }

        movementInput = new Vector2(horizontal, vertical);

        // Prevent diagonal movement from being faster.
        movementInput = Vector2.ClampMagnitude(movementInput, 1f);

        runInput = Keyboard.current[runningKey].isPressed;
    }

    private void ApplyMovement()
    {
        IsRunning = canRun && runInput;

        float targetMovingSpeed = IsRunning ? runSpeed : speed;

        if (speedOverrides.Count > 0)
        {
            Func<float> latestOverride =
                speedOverrides[speedOverrides.Count - 1];

            if (latestOverride != null)
            {
                targetMovingSpeed = latestOverride();
            }
        }

        Vector3 localVelocity = new Vector3(
            movementInput.x * targetMovingSpeed,
            0f,
            movementInput.y * targetMovingSpeed
        );

        Vector3 worldVelocity = transform.rotation * localVelocity;

        playerRigidbody.linearVelocity = new Vector3(
            worldVelocity.x,
            playerRigidbody.linearVelocity.y,
            worldVelocity.z
        );
    }
}