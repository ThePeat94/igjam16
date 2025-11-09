using UnityEngine;

namespace Nidavellir
{
	public class Coin : MonoBehaviour
	{
		private Purse m_purse;

		private void Awake()
		{
			m_purse = FindFirstObjectByType<Purse>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				m_purse.AddCoin();
				Destroy(gameObject);
			}
		}
	}
}