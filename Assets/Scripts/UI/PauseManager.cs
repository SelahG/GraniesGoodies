using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Player Scripts To Disable")]
    [Tooltip(
        "Assign FirstPersonLook, FirstPersonMovement, " +
        "PlayerInteractions, and Zoom here."
    )]
    [SerializeField] private Behaviour[] playerScripts;

    [Header("Scene Loading")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public bool IsPaused { get; private set; }

    private bool[] previousScriptStates;

    private void Start()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        LockCursor();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (IsPaused)
        {
            return;
        }

        IsPaused = true;

        StoreAndDisablePlayerScripts();

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "PauseManager has no pause menu UI assigned."
            );
        }

        Time.timeScale = 0f;

        UnlockCursor();
    }

    public void Resume()
    {
        if (!IsPaused)
        {
            return;
        }

        IsPaused = false;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;

        RestorePlayerScripts();
        LockCursor();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (SceneTransitionManager.singleton == null)
        {
            Debug.LogError(
                "No SceneTransitionManager exists in the scene."
            );

            return;
        }

        SceneTransitionManager.singleton.GoToScene(
            mainMenuSceneName
        );
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quitting game.");

        Application.Quit();
    }

    private void StoreAndDisablePlayerScripts()
    {
        previousScriptStates =
            new bool[playerScripts.Length];

        for (int i = 0; i < playerScripts.Length; i++)
        {
            Behaviour playerScript = playerScripts[i];

            if (playerScript == null)
            {
                continue;
            }

            previousScriptStates[i] = playerScript.enabled;
            playerScript.enabled = false;
        }
    }

    private void RestorePlayerScripts()
    {
        if (previousScriptStates == null)
        {
            return;
        }

        for (int i = 0; i < playerScripts.Length; i++)
        {
            Behaviour playerScript = playerScripts[i];

            if (playerScript == null)
            {
                continue;
            }

            playerScript.enabled =
                previousScriptStates[i];
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}