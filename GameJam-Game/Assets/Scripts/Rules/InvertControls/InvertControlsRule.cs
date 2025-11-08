using UnityEngine;

namespace Nidavellir.Rules.InvertControls
{
    public class InvertControlsRule : MonoBehaviour
    {
        private MovementController movement;
        
        private void Awake()
        {
            movement = GetComponent<MovementController>();
            movement.InvertControls(true);
        }

        private void OnDestroy()
        {
            movement.InvertControls(false);
        }
    }
}