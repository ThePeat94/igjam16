using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Popups
{
    public class PauseMenuPopup : MonoBehaviour
    {
        [SerializeField]
        private Button resumeButton;

        [SerializeField]
        private Button backToMenuButton;

        [SerializeField]
        private Button restartButton;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnPauseChanged -= OnPauseChanged;
            }
        }

        private void OnPauseChanged(bool isPaused)
        {
            gameObject.SetActive(isPaused);
        }

        private void OnResumeButtonClicked()
        {
            if (gameManager != null)
            {
                gameManager.Resume();
            }
        }

        private void OnBackToMenuButtonClicked()
        {
            gameManager.BackToMainMenu();
        }

        private void OnRestartButtonClicked()
        {
            gameManager.Restart();
        }
    }
}