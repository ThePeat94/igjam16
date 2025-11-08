using Nidavellir.UI.Popups;
using UnityEngine;

namespace Nidavellir
{
    public class Sign : MonoBehaviour
    {
        [SerializeField] private SignPopup signPopup;
        [SerializeField] private string signText;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && signPopup != null)
            {
                signPopup.ShowSignPopup(signText);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                signPopup.HideSignPopup();
            }
        }
    }
}
