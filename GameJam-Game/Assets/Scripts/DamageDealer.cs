using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageDealer : MonoBehaviour
    {
        public enum AmountMode { Flat, PercentOfMax }

        public AmountMode amountMode = AmountMode.PercentOfMax;
        public float amount = 0.25f;      // 0.25 = 25% of Max if PercentOfMax, else flat HP
        public float hitCooldown = 0.5f;  // per-target delay
        public LayerMask targetLayers;    // who this can hurt
        public bool useTrigger = true;    // true if this collider is a Trigger

        private readonly Dictionary<HealthController, float> lastHit = new();

        private void OnTriggerEnter2D(Collider2D other) { if (useTrigger) TryHit(other.gameObject); }
        private void OnTriggerStay2D(Collider2D other)  { if (useTrigger) TryHit(other.gameObject); }

        private void OnCollisionEnter2D(Collision2D other) { if (!useTrigger) TryHit(other.gameObject); }
        private void OnCollisionStay2D(Collision2D other)  { if (!useTrigger) TryHit(other.gameObject); }

        private void TryHit(GameObject go)
        {
            if (targetLayers != 0 && ((1 << go.layer) & targetLayers.value) == 0) return;

            var hc = go.GetComponentInParent<HealthController>();
            if (hc == null) return;

            float now = Time.time;
            if (lastHit.TryGetValue(hc, out var t) && now - t < hitCooldown) return;
            lastHit[hc] = now;

            float dmg = amountMode == AmountMode.PercentOfMax ? hc.MaxHealth * amount : amount;
            dmg = Mathf.Clamp(dmg, 0f, hc.CurrentHealth); // avoid exception when overkilling

            if (dmg > 0f)
            {
                hc.DamageMode = DamageMode.Damage;
                hc.ProcessDamage(dmg);
            }
        }
    }
}