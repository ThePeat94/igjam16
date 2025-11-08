using UnityEngine;

namespace Nidavellir.Rules.FixedSpeed
{
    public class FixedSpeedRule : MonoBehaviour
    {
        private MovementOldInput m_movementOldInput;
        
        private void Awake()
        {
            m_movementOldInput = GetComponent<MovementOldInput>();
            if (m_movementOldInput != null)
            {
                m_movementOldInput.ToggleMode(true);
            }
        }
        
        private void OnDisable()
        {
            if (m_movementOldInput != null)
            {
                m_movementOldInput.ToggleMode(false);
            }
        }
    }
}