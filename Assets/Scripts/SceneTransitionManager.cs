using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager singleton;

    private bool isLoading;

    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToScene(int sceneIndex)
    {
        GoToSceneAsync(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        if (isLoading)
        {
            return;
        }

        if (sceneIndex < 0 ||
            sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError(
                $"Scene index {sceneIndex} is not included in Build Settings."
            );

            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    public void GoToScene(string sceneName)
    {
        if (isLoading)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Cannot load a scene with an empty name.");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        isLoading = true;

        // Ensure scene loading works when called from the pause menu.
        Time.timeScale = 1f;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneIndex);

        if (operation == null)
        {
            Debug.LogError($"Could not load scene index {sceneIndex}.");
            isLoading = false;
            yield break;
        }

        while (!operation.isDone)
        {
            yield return null;
        }

        isLoading = false;
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        Time.timeScale = 1f;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError($"Could not load scene \"{sceneName}\".");
            isLoading = false;
            yield break;
        }

        while (!operation.isDone)
        {
            yield return null;
        }

        isLoading = false;
    }
}