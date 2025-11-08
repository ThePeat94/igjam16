using System.Collections;
using UnityEngine;

namespace Nidavellir
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BreakingPlatform : MonoBehaviour
    {
        public int durability = 2;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                if (other.transform.position.y > transform.position.y)
                {
                    StartCoroutine(DestroyAfterDelay());
                }
            }
        }
        
        IEnumerator DestroyAfterDelay()
        {
            StartCoroutine(ShakeEffect(durability - 0.5f));
            yield return new WaitForSeconds(durability);
            Destroy(gameObject);
        }
        
        IEnumerator ShakeEffect(float duration = 1f, float magnitude = 0.05f)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }
}
