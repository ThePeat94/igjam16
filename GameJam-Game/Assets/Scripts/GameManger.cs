using UnityEngine;

namespace Nidavellir
{
    public class GameManger : MonoBehaviour
    {
        [SerializeField]
        private Transform player;
        
        [SerializeField]
        private Transform startPoint;
        
        [SerializeField]
        private Target goal;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player.position = startPoint.position;
            goal.Initialize(EndGame);
        }

        private void EndGame()
        {
            Debug.Log("You Won!");
        }
    }
}
