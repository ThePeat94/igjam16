using Nidavellir.Rules.FixedSpeed;
using UnityEngine;

namespace Nidavellir.LevelElements
{
    public class DashPlatform : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<MovementController>();
                var fixedSpeedRule = player.GetComponent<FixedSpeedRule>();
                if (other.transform.position.y >= transform.position.y)
                {
                    if (!player.isDashing && fixedSpeedRule == null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}