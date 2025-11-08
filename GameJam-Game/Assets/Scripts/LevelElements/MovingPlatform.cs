using UnityEngine;

namespace Nidavellir
{
    public class MovingPlatform : MonoBehaviour
    {
        public float speed = 2f;
        public Transform[] points;

        private int m_i;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            transform.position = points[0].position;
            m_i = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector2.Distance(transform.position, points[m_i].position) < 0.1f)
            {
                m_i++;
                if (m_i == points.Length)
                {
                    m_i = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, points[m_i].position,
                speed * Time.deltaTime);
        }
    }
}
