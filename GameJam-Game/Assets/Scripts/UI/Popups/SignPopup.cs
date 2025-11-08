using TMPro;
using UnityEngine;

namespace Nidavellir.UI.Popups
{
    public class SignPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI signText;

        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
       public void ShowSignPopup(string text)
       {
           signText.text = text;
           gameObject.SetActive(true);
       }

       public void HideSignPopup()
       {
           gameObject.SetActive(false);
       }
    }
}