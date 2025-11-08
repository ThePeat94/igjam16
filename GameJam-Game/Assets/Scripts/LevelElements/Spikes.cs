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
                Debug.Log("HP -1");
            }
        }
    }
}
