using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [field: SerializeField] public GameState CurrentGameState { get; set; }
        
        [field: SerializeField] public string MainMenuScene { get; set; }
        [field: SerializeField] public string GameOverScene { get; set; }
        // [field: SerializeField] public string PauseScene { get; set; }
        
        [field: SerializeField] public string CurrentScene { get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetGameState(CurrentGameState);
        }
        
        public void SetGameState(GameState gameState)
        {
            CurrentScene = SceneManager.GetActiveScene().name;
            CurrentGameState = gameState;
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    SceneManager.LoadScene(MainMenuScene);
                    break;
                case GameState.Playing:
                    SceneManager.LoadScene(CurrentScene);
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    SceneManager.LoadScene(GameOverScene);
                    break;
            }
        }
    }
}