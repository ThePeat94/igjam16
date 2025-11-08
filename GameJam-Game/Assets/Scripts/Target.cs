using System;
using UnityEngine;

namespace Nidavellir
{
    public class Target : MonoBehaviour
    {
        private Action onTargetReached;
        
        public void Initialize(Action onTargetReached)
        {
            this.onTargetReached = onTargetReached;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) 
            {
                onTargetReached?.Invoke();
            }
        }
    }
}
