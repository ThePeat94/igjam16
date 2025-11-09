using UnityEngine;

namespace Nidavellir.Rules.NoWallGrab
{
    public class NoWallGrabRule : MonoBehaviour
    {
        private MovementController m_movementController;
        private bool m_originalCanWallGrab;

        private void Awake()
        {
            m_movementController = GetComponent<MovementController>();
            if (m_movementController != null)
            {
                m_originalCanWallGrab = m_movementController.canWallGrab;
            }
        }

        private void OnEnable()
        {
            if (m_movementController != null)
            {
                m_movementController.canWallGrab = false;
            }
        }

        private void OnDestroy()
        {
            if (m_movementController != null)
            {
                m_movementController.canWallGrab = m_originalCanWallGrab;
            }
        }
    }
}
