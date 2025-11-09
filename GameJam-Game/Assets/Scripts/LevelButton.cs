using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir
{
    public class LevelButton : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI label;

        private string _sceneName;
        private int _levelNumber;
        private Action<string, int> _onClick;

        private void Awake()
        {
            if (button == null) button = GetComponent<Button>();
        }
    
        public void Init(int levelNumber, string sceneName, bool unlocked, Action<string, int> onClick)
        {
            _levelNumber = levelNumber;
            _sceneName = sceneName;
            _onClick = onClick;

            if (label != null) label.text = levelNumber.ToString();

            if (button != null)
            {
                button.interactable = true;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => _onClick?.Invoke(_sceneName, _levelNumber));
            }
        }
    }
}