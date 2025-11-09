using UnityEngine;

namespace Nidavellir
{
    public class Purse : MonoBehaviour
    {
        private static Purse s_instance;

        public static Purse Instance => s_instance;

        private int m_coinCount = 0;

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

        public void AddCoin()
        {
            m_coinCount++;
            Debug.Log("Coin collected! Total coins: " + m_coinCount);
        }

        public void ResetCoins()
        {
            m_coinCount = 0;
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