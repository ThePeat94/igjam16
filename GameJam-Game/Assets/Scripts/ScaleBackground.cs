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

            // Determine which transform to use for positioning
            m_targetTransform = useParentForPositioning && transform.parent != null ? transform.parent : transform;

            float height = m_camera.orthographicSize * 2f;
            m_viewportWidth = height * Screen.width / Screen.height;
            Vector2 spriteSize = m_spriteRenderer.sprite.bounds.size;

            Vector3 scale = transform.localScale;
            scale.x = m_viewportWidth / spriteSize.x;
            scale.y = height / spriteSize.y;
            transform.localScale = scale;

            m_spriteWidth = spriteSize.x * scale.x;
            
            // Set initial camera position reference
            m_lastCameraPosition = m_camera.transform.position;
            
            // Position background at camera center
            Vector3 cameraPosition = m_camera.transform.position;
            float currentZ = m_targetTransform.position.z;
            m_targetTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, currentZ);
            
            // Store the Y position after centering on camera
            m_fixedYPosition = m_targetTransform.position.y;
            
            // Create tile instances for seamless tiling
            if (enableHorizontalTiling)
            {
                CreateTileInstances();
            }
        }

        private void CreateTileInstances()
        {
            // Calculate how many tiles we need to cover viewport plus some extra
            m_tilesNeeded = Mathf.CeilToInt(m_viewportWidth / m_spriteWidth) + 4; // +4 for buffer
            
            // Create tile instances
            for (int i = 1; i < m_tilesNeeded; i++) // Start from 1 since original is at 0
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
            
            // Copy sprite renderer properties
            SpriteRenderer tileRenderer = tile.AddComponent<SpriteRenderer>();
            tileRenderer.sprite = m_spriteRenderer.sprite;
            tileRenderer.sortingLayerName = m_spriteRenderer.sortingLayerName;
            tileRenderer.sortingOrder = m_spriteRenderer.sortingOrder;
            tileRenderer.material = m_spriteRenderer.material;
            
            // Set same scale
            tile.transform.localScale = transform.localScale;
            
            // Position tile
            Vector3 position = transform.position;
            position.x += offsetIndex * m_spriteWidth;
            tile.transform.position = position;
            
            return tile;
        }
        
        private void LateUpdate()
        {
            if (m_camera == null) return;

            Vector3 cameraDelta = m_camera.transform.position - m_lastCameraPosition;
            
            // Move all tiles with parallax
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
            
            // If tile is too far left, move it to the right
            if (distanceFromCamera < -(m_viewportWidth / 2f + m_spriteWidth))
            {
                Vector3 pos = tile.position;
                pos.x += m_spriteWidth * m_tilesNeeded;
                tile.position = pos;
            }
            // If tile is too far right, move it to the left
            else if (distanceFromCamera > (m_viewportWidth / 2f + m_spriteWidth))
            {
                Vector3 pos = tile.position;
                pos.x -= m_spriteWidth * m_tilesNeeded;
                tile.position = pos;
            }
        }
        
        private void OnDestroy()
        {
            // Clean up tile instances
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