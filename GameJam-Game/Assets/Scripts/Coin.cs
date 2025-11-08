using System;
using UnityEngine;

namespace Nidavellir
{
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Purse>().AddCoin();
                Destroy(gameObject);
            }
        }
    }
}
