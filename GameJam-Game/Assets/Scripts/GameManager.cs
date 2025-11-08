using System;
using Nidavellir.Scriptables;
using Nidavellir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LevelData levelData;

        [SerializeField]
        private MovementOldInput player;

        [SerializeField]
        private Transform startPoint;

        [SerializeField]
        private Target goal;

        [SerializeField]
        private Timer timer;

        public LevelData LevelData => levelData;
        
        // Fired when the game ends. The bool parameter indicates whether the player won (true) or lost (false).
        public event Action<bool> OnGameOver;

        private void Start()
        {
            player.transform.position = startPoint.position;
            player.enabled = false;

            goal.Initialize(OnReachedGoal);
            timer.Init(levelData.LevelDurationInSeconds, OnGameTimerEnd);
            timer.StopTimer();

            if (levelData.IsRandomRuleLevel)
            {
                StartLevel();
                return;
            }

            var uiController = FindFirstObjectByType<UIController>();
            if (uiController != null)
            {
                uiController.ShowRulesSelection();
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        public void StartLevel()
        {
            player.enabled = true;
            timer.StartTimer();
        }

        public void Restart()
        {
            Manager.SceneManager.ReloadCurrentLevel();
        }
        
        public void BackToMainMenu()
        {
            Manager.SceneManager.LoadMainMenuScene();
        }

        public void NextLevel()
        {
            Manager.SceneManager.LoadLevelScene(Manager.SceneManager.CurrentLevelIndex + 1);
        }

        private void OnGameTimerEnd()
        {
            EndGame(false);
        }

        private void OnReachedGoal()
        {
            EndGame(true);
        }

        private void EndGame(bool win)
        {
            if (win)
            {
                Debug.Log("You Won!");
            }
            else
            {
                Debug.Log("You Lost!");
            }
            
            OnGameOver?.Invoke(win);
        }
    }
}
