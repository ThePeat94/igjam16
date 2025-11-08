using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Nidavellir
{
    public class Water : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("einfach schwimmen, einfach schimmen, einfach schwimmen, schwimmen, schwimmen");
        }
    }
}
