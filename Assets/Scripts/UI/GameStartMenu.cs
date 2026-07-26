using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject about;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button aboutButton;
    [SerializeField] private Button quitButton;

    [Header("Return Buttons")]
    [SerializeField]
    private List<Button> returnButtons =
        new List<Button>();

    [Header("Scene Loading")]
    [SerializeField] private int gameplaySceneIndex = 1;

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EnableMainMenu();
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (optionButton != null)
        {
            optionButton.onClick.AddListener(EnableOptions);
        }

        if (aboutButton != null)
        {
            aboutButton.onClick.AddListener(EnableAbout);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        foreach (Button returnButton in returnButtons)
        {
            if (returnButton != null)
            {
                returnButton.onClick.AddListener(EnableMainMenu);
            }
        }
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartGame);
        }

        if (optionButton != null)
        {
            optionButton.onClick.RemoveListener(EnableOptions);
        }

        if (aboutButton != null)
        {
            aboutButton.onClick.RemoveListener(EnableAbout);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(QuitGame);
        }

        foreach (Button returnButton in returnButtons)
        {
            if (returnButton != null)
            {
                returnButton.onClick.RemoveListener(EnableMainMenu);
            }
        }
    }

    public void StartGame()
    {
        if (SceneTransitionManager.singleton == null)
        {
            Debug.LogError(
                "No SceneTransitionManager exists in the scene."
            );

            return;
        }

        HideAll();

        SceneTransitionManager.singleton.GoToSceneAsync(
            gameplaySceneIndex
        );
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game.");

        Application.Quit();
    }

    public void HideAll()
    {
        SetPageStates(false, false, false);
    }

    public void EnableMainMenu()
    {
        SetPageStates(true, false, false);
    }

    public void EnableOptions()
    {
        SetPageStates(false, true, false);
    }

    public void EnableAbout()
    {
        SetPageStates(false, false, true);
    }

    private void SetPageStates(
        bool showMainMenu,
        bool showOptions,
        bool showAbout)
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(showMainMenu);
        }

        if (options != null)
        {
            options.SetActive(showOptions);
        }

        if (about != null)
        {
            about.SetActive(showAbout);
        }
    }
}