using System.Collections;
using Nidavellir.Rules;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BreakingPlatform : MonoBehaviour
    {
        [SerializeField] private int durability = 2;
        [SerializeField] private bool listenToGravityRule = false;
        [SerializeField] private GravityRuleData requiredGravityRule;
        [SerializeField] private RuleManager m_ruleManager;
        
        private void Awake()
        {
            if (m_ruleManager == null)
            {
                m_ruleManager = FindFirstObjectByType<RuleManager>();
            }
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                // Check if we need to listen to gravity rule and if it's active
                if (listenToGravityRule && !IsRequiredGravityRuleActive())
                {
                    return;
                }
                
                if (other.transform.position.y > transform.position.y)
                {
                    StartCoroutine(DestroyAfterDelay());
                }
            }
        }
        
        private bool IsRequiredGravityRuleActive()
        {
            if (m_ruleManager == null || requiredGravityRule == null)
            {
                return false;
            }
            
            return m_ruleManager.IsRuleActive(requiredGravityRule);
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
