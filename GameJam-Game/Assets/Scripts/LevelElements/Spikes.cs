using System;
using UnityEngine;

namespace Nidavellir
{
    public class Spikes : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<HealthController>().ProcessDamage(1);
            }
        }
    }
}
