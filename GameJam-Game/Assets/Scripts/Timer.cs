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
        
        public void Init(float time, Action onTimerEnd)
        {
            this.time = time;
            this.onTimerEnd = onTimerEnd;
        }

        public void Update()
        {
            if (time <= 0f)
            {
                return;
            }
            
            time = Mathf.Max(time - Time.deltaTime, 0f);
            if (time <= 0f)
            {
                onTimerEnd?.Invoke();
            }
            
            timerText.SetText(FormatSeconds(time));
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