using UnityEngine;

namespace Nidavellir
{
    public class GameManger : MonoBehaviour
    {
        [SerializeField]
        private Transform Player;
        
        [SerializeField]
        private Transform StartPoint;
        
        [SerializeField]
        private BoxCollider GoalCollider;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Player.position = StartPoint.position;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
