using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
	public class Coin : MonoBehaviour
	{
		private Purse m_purse;
		private string m_coinId;
		private bool m_isCollected = false;

		private void Awake()
		{
			m_purse = Purse.Instance;
			
			// Generate unique ID from rounded position and scene name
			Vector3 roundedPos = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z)
			);
			string sceneName = SceneManager.GetActiveScene().name;
			m_coinId = $"{sceneName}_{roundedPos.x}_{roundedPos.y}_{roundedPos.z}";
		}

		private void Start()
		{
			// Check if coin has been permanently collected
			if (m_purse != null && m_purse.IsCoinCollected(m_coinId))
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player") && !m_isCollected && m_purse != null)
			{
				m_isCollected = true;
				m_purse.AddTemporaryCoin(m_coinId);
				Destroy(gameObject);
			}
		}
	}
}