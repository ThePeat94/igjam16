using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private float timeLimitInSeconds = 10f;
        
        [SerializeField]
        private MovementOldInput player;
        
        [SerializeField]
        private Transform startPoint;
        
        [SerializeField]
        private Target goal;
        
        [SerializeField]
        private Timer timer;
        
        // Fired when the game ends. The bool parameter indicates whether the player won (true) or lost (false).
        public event Action<bool> OnGameOver;
        
        private void Start()
        {
            player.transform.position = startPoint.position;
            goal.Initialize(OnReachedGoal);
            timer.Init(timeLimitInSeconds, OnGameTimerEnd);
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
