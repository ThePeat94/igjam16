using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Manager
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager instance;
        private static SceneDictionary dictionary;
        public static int CurrentLevelIndex { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                LoadDictionary();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static void LoadDictionary()
        {
            if (dictionary == null)
            {
                dictionary = Resources.Load<SceneDictionary>("Data/SceneDictionary");
                if (dictionary == null)
                {
                    Debug.LogError("SceneDictionary not found in Resources/Data/!");
                }
            }
        }

        public static void LoadMainMenuScene()
        {
            LoadDictionary();
            if (dictionary != null)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(dictionary.MainMenuScene.name);
            }
        }

        public static void LoadLevelScene(int level)
        {
            if (level >= dictionary.LevelScenes.Count)
            {
                Debug.LogWarning("Level " + level + " is out of range! Returning to Main Menu.");
                LoadMainMenuScene();
                return;
            }
            CurrentLevelIndex = level;
            UnityEngine.SceneManagement.SceneManager.LoadScene(dictionary.LevelScenes[level].name);
        }
        
        public static void ReloadCurrentLevel()
        {
            LoadLevelScene(CurrentLevelIndex);
        }
    }
}