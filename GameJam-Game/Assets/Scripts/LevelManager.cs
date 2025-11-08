using System;
using Nidavellir.Scriptables;
using Nidavellir.UI.Rules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private AvailableRulesUI m_availableRulesUI;
        [SerializeField] private LevelData m_levelData;
        [SerializeField] private MovementOldInput m_playerController;
        [SerializeField] private Timer m_timer;
        
        
        
        private void Awake()
        {
            this.m_availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            this.m_playerController ??= FindFirstObjectByType<MovementOldInput>(FindObjectsInactive.Include);
            this.m_timer ??= FindFirstObjectByType<Timer>(FindObjectsInactive.Include);
            this.m_availableRulesUI.OnStartLevelClicked += this.HandleStartLevel;
        }

        private void Start()
        {
            this.m_playerController.enabled = false;
            this.m_timer.StopTimer();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void HandleStartLevel()
        {
            this.m_availableRulesUI.Hide();
            this.m_playerController.enabled = true;
            this.m_timer.StartTimer();
        }
    }
}