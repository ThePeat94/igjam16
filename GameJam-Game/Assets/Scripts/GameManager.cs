using System;
using Nidavellir.Scriptables;
using Nidavellir.UI.Rules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private AvailableRulesUI availableRulesUI;

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

        private void Awake()
        {
            availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            player ??= FindFirstObjectByType<MovementOldInput>(FindObjectsInactive.Include);
            timer ??= FindFirstObjectByType<Timer>(FindObjectsInactive.Include);

            if (availableRulesUI != null)
            {
                availableRulesUI.OnStartLevelClicked += HandleStartLevel;
            }
        }

        private void Start()
        {
            player.transform.position = startPoint.position;
            player.enabled = false;

            goal.Initialize(OnReachedGoal);
            timer.Init(levelData.LevelDurationInSeconds, OnGameTimerEnd);
            timer.StopTimer();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        private void HandleStartLevel()
        {
            if (availableRulesUI != null)
            {
                availableRulesUI.Hide();
            }

            player.enabled = true;
            timer.StartTimer();
        }

        public void Restart()
        {
            SceneManager.LoadScene(1);
        }
        
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
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
