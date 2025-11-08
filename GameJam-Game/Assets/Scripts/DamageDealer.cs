using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageDealer : MonoBehaviour
    {
        public enum AmountMode
        {
            Flat,
            PercentOfMax
        }

        public AmountMode amountMode = AmountMode.PercentOfMax;
        public float amount = 0.25f;
        public float hitCooldown = 0.5f;
        public LayerMask targetLayers;
        public bool useTrigger = true;

        // NEW: gate damage to attack windows
        public bool onlyWhenArmed = true;
        public HealthController owner;

        private bool armed;
        private readonly Dictionary<HealthController, float> lastHit = new();

        private void Awake()
        {
            if (owner == null) owner = GetComponentInParent<HealthController>();
            armed = !onlyWhenArmed;
        }

        public void Arm() => armed = true;
        public void Disarm() => armed = !onlyWhenArmed ? true : false;

        public void ArmFor(float seconds)
        {
            StartCoroutine(ArmWindow(seconds));
        }

        private IEnumerator ArmWindow(float seconds)
        {
            armed = true;
            yield return new WaitForSeconds(seconds);
            Disarm();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (useTrigger) TryHit(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (useTrigger) TryHit(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!useTrigger) TryHit(other.gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!useTrigger) TryHit(other.gameObject);
        }

        private void TryHit(GameObject go)
        {
            if (onlyWhenArmed && !armed) return;

            if (targetLayers != 0 && ((1 << go.layer) & targetLayers.value) == 0) return;

            var hc = go.GetComponentInParent<HealthController>();
            if (hc == null) return;
            if (owner != null && hc == owner) return;

            float now = Time.time;
            if (lastHit.TryGetValue(hc, out var t) && now - t < hitCooldown) return;
            lastHit[hc] = now;

            float dmg = amountMode == AmountMode.PercentOfMax ? hc.MaxHealth * amount : amount;
            dmg = Mathf.Clamp(dmg, 0f, hc.CurrentHealth);

            if (dmg > 0f)
            {
                hc.DamageMode = DamageMode.Damage;
                hc.ProcessDamage(dmg);
            }
        }
    }
}