using UnityEngine;

namespace Nidavellir
{
    public class GameManger : MonoBehaviour
    {
        [SerializeField]
        private float TimeLimitInSeconds = 10f;
        
        [SerializeField]
        private Transform player;
        
        [SerializeField]
        private Transform startPoint;
        
        [SerializeField]
        private Target goal;
        
        [SerializeField]
        private Timer timer;
        
        private void Start()
        {
            player.position = startPoint.position;
            goal.Initialize(() => EndGame(true));
            timer.Init(TimeLimitInSeconds, () => EndGame(false));
        }

        private void EndGame(bool win)
        {
            if (win)
            {
                Debug.Log("You Won!");
            }
            else
            {
                Debug.Log("You Lost!");
            }
        }
    }
}
