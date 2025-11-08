using Nidavellir.UI.Popups;
using UnityEngine;

namespace Nidavellir.UI
{
	public class UIController : MonoBehaviour
	{
		[SerializeField]
		private ResultScreenPopup resultPopup;

		[SerializeField]
		private HealthController playerHealthController;

		private void Start()
		{
			FindFirstObjectByType<GameManager>().OnGameOver += ShowResultPopup;

			// Subscribe to player death
			if (playerHealthController != null)
			{
				playerHealthController.OnDeath += OnPlayerDeath;
			}
			else
			{
				Debug.LogWarning("[UIController] Could not find HealthController. Player death will not trigger game over screen.");
			}
		}

		private void OnDestroy()
		{
			if (playerHealthController != null)
			{
				playerHealthController.OnDeath -= OnPlayerDeath;
			}
		}

		private void OnPlayerDeath()
		{
			ShowResultPopup(false);
		}

		private void ShowResultPopup(bool win)
		{
			resultPopup.Init(win);
			resultPopup.gameObject.SetActive(true);
		}
	}
}