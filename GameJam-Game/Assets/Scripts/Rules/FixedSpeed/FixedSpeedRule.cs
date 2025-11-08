using UnityEngine;

namespace Nidavellir.Rules.FixedSpeed
{
    public class FixedSpeedRule : MonoBehaviour
    {
        private MovementController m_movementController;
        
        private void Awake()
        {
            m_movementController = GetComponent<MovementController>();
            if (m_movementController != null)
            {
                m_movementController.ToggleMode(true);
            }
        }
        
        private void OnDisable()
        {
            if (m_movementController != null)
            {
                m_movementController.ToggleMode(false);
            }
        }
    }
}