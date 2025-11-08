using System;
using Nidavellir.UI.Popups;
using UnityEngine;

namespace Nidavellir.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private ResultScreenPopup resultPopup;

        private void Start()
        {
            FindFirstObjectByType<GameManager>().OnGameOver += ShowResultPopup;
        }

        private void ShowResultPopup(bool win)
        {
            resultPopup.Init(win);
            resultPopup.gameObject.SetActive(true);
        }
    }
}