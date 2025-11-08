using UnityEngine;

namespace Nidavellir.LevelElements
{
    public class GrassPlatform : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                Debug.Log("Walking through grass, enemies alerted, slowing down!");
            }
        }
    }
}
