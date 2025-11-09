using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class LevelProgressView : MonoBehaviour
    {
        public GameObject noStar;
        public GameObject halfStar;
        public GameObject oneStar;
        
        
        public void Init(bool unlocked, int coins, LevelData levelData)
        {
            var totalCoins = levelData != null ? levelData.MaxCoins : 0;
            
            if (!unlocked)
            {
                noStar.SetActive(false);
                halfStar.SetActive(false);
                oneStar.SetActive(false);
                return;
            }
            
            if(!PlayerInventory.Instance.PlayedLevels.Contains(levelData))
            {
                noStar.SetActive(false);
                halfStar.SetActive(false);
                oneStar.SetActive(false);
                return;
            }

            float progress = totalCoins > 0 ? (float)coins / totalCoins : 0f;

            if (progress >= 1f)
            {
                noStar.SetActive(false);
                halfStar.SetActive(false);
                oneStar.SetActive(true);
            }
            else if (progress > 0f)
            {
                noStar.SetActive(false);
                halfStar.SetActive(true);
                oneStar.SetActive(false);
            }
            else
            {
                noStar.SetActive(true);
                halfStar.SetActive(false);
                oneStar.SetActive(false);
            }
        }
    }
}
