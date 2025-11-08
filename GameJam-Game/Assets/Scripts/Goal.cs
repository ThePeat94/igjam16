using System;
using UnityEngine;

namespace Nidavellir
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Registered Collision");
            if (other.gameObject.CompareTag("Player")) 
            {
                Debug.Log("Goal Reached!");
            }
        }
    }
}
