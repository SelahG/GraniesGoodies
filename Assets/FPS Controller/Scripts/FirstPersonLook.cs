using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform character;

    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float smoothing = 1.5f;

    private Vector2 velocity;
    private Vector2 frameVelocity;

    private void Reset()
    {
        FirstPersonMovement movement =
            GetComponentInParent<FirstPersonMovement>();

        if (movement != null)
        {
            character = movement.transform;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Mouse.current == null || character == null)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        Vector2 rawFrameVelocity = mouseDelta * sensitivity;

        float smoothingAmount =
            smoothing <= 0f ? 1f : 1f / smoothing;

        frameVelocity = Vector2.Lerp(
            frameVelocity,
            rawFrameVelocity,
            smoothingAmount
        );

        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(
            -velocity.y,
            Vector3.right
        );

        character.localRotation = Quaternion.AngleAxis(
            velocity.x,
            Vector3.up
        );
    }
}