using UnityEngine;
using System.Collections.Generic;

namespace Nidavellir
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScaleBackground : MonoBehaviour
    {
        [Header("Parallax Settings")]
        [SerializeField] private float parallaxSpeedX = 1f;
        [SerializeField] private float parallaxSpeedY = 0f;
        
        [Header("Tiling Settings")]
        [SerializeField] private bool enableHorizontalTiling = true;
        [SerializeField] private bool constrainVertical = true;
        
        [Header("Parent Settings")]
        [SerializeField] private bool useParentForPositioning = false;
    
        private Camera m_camera;
        private Vector3 m_lastCameraPosition;
        private float m_spriteWidth;
        private float m_viewportWidth;
        private float m_fixedYPosition;
        private SpriteRenderer m_spriteRenderer;
        private Transform m_targetTransform;
        private List<GameObject> m_tileInstances = new List<GameObject>();
        private int m_tilesNeeded;

        private void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_camera = Camera.main;

            m_targetTransform = transform;

            float height = m_camera.orthographicSize * 2f;
            m_viewportWidth = height * Screen.width / Screen.height;
            Vector2 spriteSize = m_spriteRenderer.sprite.bounds.size;

            Vector3 scale = transform.localScale;
            scale.x = m_viewportWidth / spriteSize.x;
            scale.y = height / spriteSize.y;
            transform.localScale = scale;

            m_spriteWidth = spriteSize.x * scale.x;
            
            m_lastCameraPosition = m_camera.transform.position;
            
            Vector3 cameraPosition = m_camera.transform.position;
            float currentZ = m_targetTransform.position.z;
            m_targetTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, currentZ);
            
            m_fixedYPosition = m_targetTransform.position.y;
            
            if (enableHorizontalTiling)
            {
                CreateTileInstances();
            }
        }

        private void CreateTileInstances()
        {
            m_tilesNeeded = Mathf.CeilToInt(m_viewportWidth / m_spriteWidth) + 2;
            
            for (int i = 1; i <= m_tilesNeeded; i++)
            {
                GameObject leftTile = CreateTile(-i);
                GameObject rightTile = CreateTile(i);
                
                m_tileInstances.Add(leftTile);
                m_tileInstances.Add(rightTile);
            }
        }
        
        private GameObject CreateTile(int offsetIndex)
        {
            GameObject tile = new GameObject($"Tile_{offsetIndex}");
            tile.transform.SetParent(transform.parent);
            
            SpriteRenderer tileRenderer = tile.AddComponent<SpriteRenderer>();
            tileRenderer.sprite = m_spriteRenderer.sprite;
            tileRenderer.sortingLayerName = m_spriteRenderer.sortingLayerName;
            tileRenderer.sortingOrder = m_spriteRenderer.sortingOrder;
            tileRenderer.material = m_spriteRenderer.material;
            
            tile.transform.localScale = transform.localScale;
            
            Vector3 position = transform.position;
            position.x += offsetIndex * m_spriteWidth;
            tile.transform.position = position;
            
            return tile;
        }
        
        private void LateUpdate()
        {
            if (m_camera == null) return;

            Vector3 cameraDelta = m_camera.transform.position - m_lastCameraPosition;
            
            MoveTile(transform, cameraDelta);
            
            if (enableHorizontalTiling)
            {
                foreach (GameObject tile in m_tileInstances)
                {
                    if (tile != null)
                    {
                        MoveTile(tile.transform, cameraDelta);
                        RepositionTileIfNeeded(tile.transform);
                    }
                }
                
                RepositionTileIfNeeded(transform);
            }
            
            m_lastCameraPosition = m_camera.transform.position;
        }
        
        private void MoveTile(Transform tile, Vector3 cameraDelta)
        {
            Vector3 newPosition = tile.position;
            newPosition.x += cameraDelta.x * parallaxSpeedX;
            
            if (!constrainVertical)
            {
                newPosition.y += cameraDelta.y * parallaxSpeedY;
            }
            else
            {
                newPosition.y = m_fixedYPosition;
            }
            
            tile.position = newPosition;
        }
        
        private void RepositionTileIfNeeded(Transform tile)
        {
            float cameraX = m_camera.transform.position.x;
            float tileX = tile.position.x;
            float distanceFromCamera = tileX - cameraX;
            float repositionDistance = m_spriteWidth * (m_tilesNeeded * 2 + 1); // Total tiles * sprite width
            
            if (distanceFromCamera < -(m_viewportWidth / 2f + m_spriteWidth * 1.5f))
            {
                Vector3 pos = tile.position;
                pos.x += repositionDistance;
                tile.position = pos;
            }
            else if (distanceFromCamera > (m_viewportWidth / 2f + m_spriteWidth * 1.5f))
            {
                Vector3 pos = tile.position;
                pos.x -= repositionDistance;
                tile.position = pos;
            }
        }
        
        private void OnDestroy()
        {
            foreach (GameObject tile in m_tileInstances)
            {
                if (tile != null)
                {
                    DestroyImmediate(tile);
                }
            }
            m_tileInstances.Clear();
        }
    }
}