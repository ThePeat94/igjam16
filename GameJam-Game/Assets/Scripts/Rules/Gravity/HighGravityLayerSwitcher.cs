using Nidavellir.Rules.Gravity;
using Nidavellir.Scriptables.Rules;
using UnityEngine;
using Nidavellir.Rules;

namespace Nidavellir.Rules.Gravity
{
    /// <summary>
    /// Component that switches the GameObject's layer when a High Gravity rule is active.
    /// Switches to the specified layer when High Gravity is active, and back to the original layer when it's not.
    /// </summary>
    public class HighGravityLayerSwitcher : MonoBehaviour
    {
        [SerializeField] private string m_highGravityLayerName = "Default";
        [SerializeField] private MovementOldInput m_playerMovement;
        [SerializeField] private RuleManager m_ruleManager;
        
        private int m_originalLayer;
        private int m_highGravityLayer;
        private bool m_isHighGravityActive;
        
        private void Awake()
        {
            // Store the original layer
            m_originalLayer = gameObject.layer;
            
            // Get the target layer index
            m_highGravityLayer = LayerMask.NameToLayer(m_highGravityLayerName);
            
            // Validate layer exists
            if (m_highGravityLayer == -1)
            {
                Debug.LogWarning($"HighGravityLayerSwitcher: Layer '{m_highGravityLayerName}' not found. Using layer index 0 instead.", this);
                m_highGravityLayer = 0;
            }
            
            // Find player if not assigned
            if (m_playerMovement == null)
            {
                m_playerMovement = FindFirstObjectByType<MovementOldInput>();
            }
            
            // Find RuleManager if not assigned
            if (m_ruleManager == null)
            {
                m_ruleManager = FindFirstObjectByType<RuleManager>();
            }
        }
        
        private void Update()
        {
            // Check if High Gravity is active
            bool isHighGravityActive = IsHighGravityActive();
            
            // Only switch if state changed
            if (isHighGravityActive != m_isHighGravityActive)
            {
                m_isHighGravityActive = isHighGravityActive;
                UpdateLayer();
            }
        }
        
        private bool IsHighGravityActive()
        {
            // First, try to check RuleManager for active High Gravity rules
            if (m_ruleManager != null)
            {
                bool hasHighGravityRule = m_ruleManager.HasActiveRule(ruleData =>
                {
                    if (ruleData is GravityRuleData gravityRuleData)
                    {
                        return gravityRuleData.GravityStrength == GravityRuleData.GravityMode.High;
                    }
                    return false;
                });
                
                if (hasHighGravityRule)
                    return true;
            }
            
            // Fallback: Check if player has a GravityRule component with high multiplier
            if (m_playerMovement != null)
            {
                var gravityRule = m_playerMovement.GetComponent<GravityRule>();
                if (gravityRule != null)
                {
                    // Check if the gravity multiplier indicates High gravity
                    // High gravity typically has a multiplier > 1.0 (usually 2.0 based on GravityRuleData)
                    // We check if it's significantly above normal (1.0)
                    return gravityRule.GravityMultiplier > 1.5f;
                }
            }
            
            return false;
        }
        
        private void UpdateLayer()
        {
            if (m_isHighGravityActive)
            {
                gameObject.layer = m_highGravityLayer;
            }
            else
            {
                gameObject.layer = m_originalLayer;
            }
        }
        
        private void OnDisable()
        {
            // Restore original layer when component is disabled
            gameObject.layer = m_originalLayer;
        }
    }
}

