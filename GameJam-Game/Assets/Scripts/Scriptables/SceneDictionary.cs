using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "SceneDictionary", menuName = "Data/Scene Dictionary", order = 0)]
    public class SceneDictionary : ScriptableObject
    {
        [SerializeField]
        private SceneAsset mainMenuScene;
        
        [SerializeField] 
        private List<SceneAsset> levelScenes;
        
        public SceneAsset MainMenuScene => mainMenuScene;
        public IReadOnlyList<SceneAsset> LevelScenes => levelScenes;
    }
}