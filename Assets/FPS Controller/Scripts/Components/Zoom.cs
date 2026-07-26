using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Zoom : MonoBehaviour
{
    private Camera attachedCamera;

    public float defaultFOV = 60f;
    public float maxZoomFOV = 15f;

    [Range(0f, 1f)]
    public float currentZoom;

    public float sensitivity = 1f;

    private void Awake()
    {
        attachedCamera = GetComponent<Camera>();

        if (attachedCamera != null)
        {
            defaultFOV = attachedCamera.fieldOfView;
        }
    }

    private void Update()
    {
        if (attachedCamera == null)
        {
            return;
        }

        // Only read player input while the game is running.
        if (Application.isPlaying && Mouse.current != null)
        {
            float scrollAmount = Mouse.current.scroll.ReadValue().y;

            // The new Input System commonly reports 120 units per scroll notch.
            scrollAmount /= 120f;

            currentZoom += scrollAmount * sensitivity * 0.05f;
            currentZoom = Mathf.Clamp01(currentZoom);
        }

        attachedCamera.fieldOfView = Mathf.Lerp(
            defaultFOV,
            maxZoomFOV,
            currentZoom
        );
    }
}