using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Popups
{
    public class ResultScreenPopup : MonoBehaviour
    {
        [SerializeField]
        private Button nextLevelButton;
        
        [SerializeField]
        private Button playAgainButton;
        
        [SerializeField]
        private Button retryButton;
        
        [SerializeField]
        private Button backToMenuButton;
        
        [SerializeField]
        private TextMeshProUGUI resultText;
        
        private void Awake()
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            playAgainButton.onClick.AddListener(OnRetryButtonClicked);
            retryButton.onClick.AddListener(OnRetryButtonClicked);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        }

        public void Init(bool win)
        {
            resultText.text = win ? "You Win!" : "Game Over!";
            nextLevelButton.gameObject.SetActive(win);
            playAgainButton.gameObject.SetActive(win);
            retryButton.gameObject.SetActive(!win);
        }

        private void OnNextLevelButtonClicked()
        {
            FindFirstObjectByType<GameManager>().NextLevel();
        }
        
        private void OnBackToMenuButtonClicked()
        {
            FindFirstObjectByType<GameManager>().BackToMainMenu();
        }

        private void OnRetryButtonClicked()
        {
            FindFirstObjectByType<GameManager>().Restart();
        }
    }
}