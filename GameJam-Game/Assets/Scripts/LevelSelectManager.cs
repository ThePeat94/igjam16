using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nidavellir
  {
    public class LevelSelectManager : MonoBehaviour
    {
        [Header("UI")]
        public Transform gridParent;
        public LevelButton levelButtonPrefab;
        public Button backButton;

        [Header("Scenes")]
        public string mainMenuScene = "StartMenu";

        [Header("Levels")]
        public int levelCount = 10;
        public string levelNameTemplate = "Level {0}";

        private void Start()
        {
            if (backButton) backButton.onClick.AddListener(GoBackToMenu);
            BuildGrid();
        }

        private void GoBackToMenu()
        {
            const string menu = "StartMenu";
            if (!Application.CanStreamedLevelBeLoaded(menu))
            {
                Debug.LogError($"Scene '{menu}' is not in Build Settings or the name is wrong.");
                return;
            }
            SceneManager.LoadScene(menu, LoadSceneMode.Single);
        }

        private void BuildGrid()
        {
            for (int i = gridParent.childCount - 1; i >= 0; i--)
                Destroy(gridParent.GetChild(i).gameObject);

            for (int i = 1; i <= levelCount; i++)
            {
                string sceneName = string.Format(levelNameTemplate, i);
                var btn = Instantiate(levelButtonPrefab, gridParent);
                btn.Init(i, sceneName, true, OnLevelClicked);

                if (!Application.CanStreamedLevelBeLoaded(sceneName))
                    Debug.LogWarning($"Scene '{sceneName}' is not in Build Settings.");
            }
        }

        private void OnLevelClicked(string sceneName, int levelNumber)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}