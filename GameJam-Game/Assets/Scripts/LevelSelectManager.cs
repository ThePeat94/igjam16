using Nidavellir.Rules;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nidavellir
{
public class LevelSelectManager : MonoBehaviour
{
[Header("UI - Level Select")]
[SerializeField] private GameObject m_levelPanel; 
[SerializeField] private Transform m_gridParent; 
[SerializeField] private LevelButton m_levelButtonPrefab;
[SerializeField] private Button m_backButton; 
[SerializeField] private Button m_shopButton; 
[SerializeField] private Button m_headline; 

    [Header("Shop UI")]
    [SerializeField] private GameObject m_shopPanel;                 
    [SerializeField] private Button m_shopBackButton;               
    [SerializeField] private RuleShop m_ruleShop;                   

    [Header("Scenes")]
    [SerializeField] private string m_mainMenuScene = "StartMenu";

    [Header("Levels")]
    [SerializeField] private int m_levelCount = 10;
    [SerializeField] private string m_levelNameTemplate = "Level {0}";

    private void Awake()
    {
        if (!m_ruleShop)
            m_ruleShop = FindFirstObjectByType<RuleShop>();
    }

    private void Start()
    {
        if (m_backButton)     m_backButton.onClick.AddListener(GoBackToMenu);
        if (m_shopButton)     m_shopButton.onClick.AddListener(ShowShop);
        if (m_shopBackButton) m_shopBackButton.onClick.AddListener(BackToStartFromShop);

        BuildGrid();

        if (m_shopPanel)  m_shopPanel.SetActive(false);
        if (m_levelPanel) m_levelPanel.SetActive(true);
    }

    private void GoBackToMenu()
    {
        if (!Application.CanStreamedLevelBeLoaded(m_mainMenuScene))
        {
            Debug.LogError($"Scene '{m_mainMenuScene}' is not in Build Settings or the name is wrong.");
            return;
        }
        SceneManager.LoadScene(m_mainMenuScene, LoadSceneMode.Single);
    }

    private void BuildGrid()
    {
        if (!m_gridParent || !m_levelButtonPrefab) return;

        for (int i = m_gridParent.childCount - 1; i >= 0; i--)
            Destroy(m_gridParent.GetChild(i).gameObject);

        for (int i = 1; i <= m_levelCount; i++)
        {
            string sceneName = string.Format(m_levelNameTemplate, i);
            var btn = Instantiate(m_levelButtonPrefab, m_gridParent);
            btn.Init(i, sceneName, true, OnLevelClicked);

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
                Debug.LogWarning($"Scene '{sceneName}' is not in Build Settings.");
        }
    }

    private void OnLevelClicked(string sceneName, int levelNumber)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ShowShop()
    {
        Debug.Log("LevelSelectManager.ShowShop clicked");

        if (m_shopPanel && !m_shopPanel.activeSelf)
            m_shopPanel.SetActive(true);

        if (!m_ruleShop)
            m_ruleShop = FindFirstObjectByType<RuleShop>();

        if (m_ruleShop)
            m_ruleShop.ShowShop();
        else
            Debug.LogError("RuleShop not found. Make sure a RuleShop exists in this scene or is DontDestroyOnLoad.");

        if (m_levelPanel) m_levelPanel.SetActive(false);
        if (m_headline)   m_headline.gameObject.SetActive(false);
        if(m_backButton) m_backButton.gameObject.SetActive(false);
        if(m_shopButton) m_shopButton.gameObject.SetActive(false);
    }

    public void BackToStartFromShop()
    {
        if (m_shopPanel)  m_shopPanel.SetActive(false);
        if (m_levelPanel) m_levelPanel.SetActive(true);
        if(m_backButton) m_backButton.gameObject.SetActive(true);
        if(m_shopButton) m_shopButton.gameObject.SetActive(true);
    }
}
}