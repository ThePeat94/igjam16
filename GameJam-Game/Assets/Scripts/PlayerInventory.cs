using System;
using System.Collections.Generic;
using Nidavellir.Scriptables.Rules;
using UnityEngine;

namespace Nidavellir
{
    public class PlayerInventory : MonoBehaviour
    {
        private static PlayerInventory s_instance;
        
        public List<RuleData> PurchasedRules { get; private set; } = new();

        public static PlayerInventory Instance => s_instance;
        
        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(this);
                return;
            }

            Destroy(this);
        }
    }
}