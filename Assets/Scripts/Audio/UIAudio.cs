using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class UIAudio : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler
{
    [Header("Audio Names")]
    [SerializeField] private string clickAudioName;
    [SerializeField] private string hoverEnterAudioName;
    [SerializeField] private string hoverExitAudioName;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickAudioName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverEnterAudioName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlaySound(hoverExitAudioName);
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySound(hoverEnterAudioName);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        PlaySound(hoverExitAudioName);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlaySound(clickAudioName);
    }

    private void PlaySound(string soundName)
    {
        if (string.IsNullOrWhiteSpace(soundName))
        {
            return;
        }

        if (AudioManager.instance == null)
        {
            Debug.LogWarning(
                $"UIAudio on {gameObject.name} could not find an AudioManager."
            );

            return;
        }

        AudioManager.instance.Play(soundName);
    }
}