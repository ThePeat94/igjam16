using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class Purse : MonoBehaviour
    {
        private static Purse s_instance;

        public static Purse Instance => s_instance;

        private int m_coinCount = 0;
        private HashSet<string> m_permanentlyCollectedCoins = new HashSet<string>();
        private HashSet<string> m_temporaryCoins = new HashSet<string>();

        public int CoinCount => this.m_coinCount;

        private void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            s_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private GameManager m_currentGameManager;

        private void Start()
        {
            StartCoroutine(SubscribeToGameManagerWhenReady());
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameManager();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Clear temporary coins when scene loads (e.g., on retry/restart)
            // This allows coins to be collected again on retry
            ClearTemporaryCoins();
            
            // Re-subscribe when a new scene loads
            StartCoroutine(SubscribeToGameManagerWhenReady());
        }

        private IEnumerator SubscribeToGameManagerWhenReady()
        {
            // Wait a frame to ensure GameManager is initialized
            yield return null;
            
            // Unsubscribe from previous GameManager if exists
            UnsubscribeFromGameManager();
            
            // Try to find GameManager
            m_currentGameManager = FindFirstObjectByType<GameManager>();
            if (m_currentGameManager != null)
            {
                m_currentGameManager.OnGameOver += OnGameOver;
            }
            else
            {
                // If not found, try again after a short delay (in case scene is still loading)
                yield return new WaitForSeconds(0.1f);
                m_currentGameManager = FindFirstObjectByType<GameManager>();
                if (m_currentGameManager != null)
                {
                    m_currentGameManager.OnGameOver += OnGameOver;
                }
            }
        }

        private void UnsubscribeFromGameManager()
        {
            if (m_currentGameManager != null)
            {
                m_currentGameManager.OnGameOver -= OnGameOver;
                m_currentGameManager = null;
            }
        }

        private void OnGameOver(bool win)
        {
            if (win)
            {
                // Player reached goal: confirm temporary coins as permanent
                ConfirmTemporaryCoins();
            }
            else
            {
                // Player lost: clear temporary coins
                ClearTemporaryCoins();
            }
        }

        public bool IsCoinCollected(string coinId)
        {
            return m_permanentlyCollectedCoins.Contains(coinId);
        }

        public void AddTemporaryCoin(string coinId)
        {
            if (m_temporaryCoins.Add(coinId))
            {
                m_coinCount++;
                Debug.Log($"Temporary coin collected! Total coins: {m_coinCount}");
            }
        }

        private void ConfirmTemporaryCoins()
        {
            // Move temporary coins to permanent collection
            foreach (var coinId in m_temporaryCoins)
            {
                m_permanentlyCollectedCoins.Add(coinId);
            }
            
            int confirmedCount = m_temporaryCoins.Count;
            m_temporaryCoins.Clear();
            
            Debug.Log($"Confirmed {confirmedCount} temporary coins as permanent. Total permanent coins: {m_permanentlyCollectedCoins.Count}");
        }

        private void ClearTemporaryCoins()
        {
            int lostCount = m_temporaryCoins.Count;
            m_coinCount -= lostCount;
            m_temporaryCoins.Clear();
            
            if (lostCount > 0)
            {
                Debug.Log($"Lost {lostCount} temporary coins. Remaining coins: {m_coinCount}");
            }
        }

        public void AddCoin()
        {
            m_coinCount++;
            Debug.Log("Coin collected! Total coins: " + m_coinCount);
        }

        public void ResetCoins()
        {
            m_coinCount = 0;
            m_permanentlyCollectedCoins.Clear();
            m_temporaryCoins.Clear();
            Debug.Log("Coin count reset.");
        }

        public void SpendCoins(int amount)
        {
            if (amount <= m_coinCount)
            {
                m_coinCount -= amount;
            }
            else
            {
                Debug.Log("Not enough coins to spend!");
            }
        }
    }
}