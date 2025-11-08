using UnityEngine;

namespace Nidavellir.Rules.InvertControls
{
    public class InvertControlsRule : MonoBehaviour
    {
        private MovementOldInput movement;
        
        private void Awake()
        {
            movement = GetComponent<MovementOldInput>();
            movement.InvertControls(true);
        }

        private void OnDestroy()
        {
            movement.InvertControls(false);
        }
    }
}