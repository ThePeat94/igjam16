using UnityEngine;

namespace Nidavellir
{
    public class OneTimePlatform : MonoBehaviour
    {
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.transform.position.y >= transform.position.y)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}