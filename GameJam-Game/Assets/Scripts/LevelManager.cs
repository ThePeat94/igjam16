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
        
        private void Awake()
        {
            this.m_availableRulesUI ??= FindFirstObjectByType<AvailableRulesUI>(FindObjectsInactive.Include);
            this.m_availableRulesUI.OnStartLevelClicked += HandleStartLevel;
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
        }
    }
}