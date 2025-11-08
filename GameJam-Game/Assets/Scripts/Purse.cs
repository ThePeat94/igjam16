using UnityEngine;

namespace Nidavellir
{
    public class Purse : MonoBehaviour
    {
        private int m_coinCount = 0;

        public void AddCoin()
        {
            m_coinCount++;
            Debug.Log("Coin collected! Total coins: " + m_coinCount);
        }
        
        public int GetCoinCount() => m_coinCount;
        
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
