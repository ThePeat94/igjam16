using System;
using TMPro;
using UnityEngine;

namespace Nidavellir
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timerText;
        
        private float time;
        private Action onTimerEnd;
        private bool stopped;
        
        public void Init(float time, Action onTimerEnd)
        {
            this.time = time;
            this.onTimerEnd = onTimerEnd;
            FindFirstObjectByType<GameManger>().OnGameOver += OnGameOver;
        }

        public void Update()
        {
            if (stopped)
            {
                return;
            }
            
            time = Mathf.Max(time - Time.deltaTime, 0f);
            if (time <= 0f)
            {
                stopped = true;
                onTimerEnd?.Invoke();
            }
            
            timerText.SetText(FormatSeconds(time));
        }

        private void OnGameOver(bool win)
        {
            stopped = true;
        }
        
        private string FormatSeconds(float seconds)
        {
            int minutes = (int)(seconds / 60);
            int secs = (int)(seconds % 60);
            int centiseconds = (int)((seconds - (int)seconds) * 100);
    
            return $"{minutes:D2}:{secs:D2}:{centiseconds:D2}";
        }
    }
}