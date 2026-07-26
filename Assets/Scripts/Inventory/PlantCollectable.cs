using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class PlantCollectable : MonoBehaviour
{

    [Header("Input")]
    public InputActionReference depositButton;

    private XRGrabInteractable grabInteractable;
    private PlantID plantID;

    private bool isHeld = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        plantID = GetComponent<PlantID>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        if (depositButton != null)
            depositButton.action.Enable();
    }

    void OnDisable()
    {
        if (depositButton != null)
            depositButton.action.Disable();
    }

    void Update()
    {
        if (!isHeld)
            return;

        if (depositButton != null &&
            depositButton.action.WasPressedThisFrame())
        {
            DepositPlant();
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    void DepositPlant()
    {
        bool accepted = InventoryManager.Instance.CollectPlant(plantID);

        if (!accepted)
            return;

        // if (collectParticles != null)
        // {
        //     Instantiate(
        //         collectParticles,
        //         particleSpawn.position,
        //         Quaternion.identity);
        // }

        Destroy(gameObject);
    }
}
