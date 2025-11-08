using Nidavellir;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthController))]
public class AutoWorldHealthBar : MonoBehaviour
{
[Header("Sprites (assign your slices)")]
public Sprite fullSprite; // green/full
public Sprite midSprite; // yellow
public Sprite lowSprite; // red
public Sprite emptySprite; // empty/track (background)

[Header("Layout")]
public bool useBackgroundSpriteSize = true;      // sizes from emptySprite if available, else from fullSprite
public Vector2 size = new Vector2(1.8f, 0.25f);  // used if auto-size is off
public Vector2 padding = new Vector2(0f, 0f);    // inner padding for FILL (pixels on sprite)
public Vector3 offset = new Vector3(0f, 1.2f, 0f);
public int sortingOrder = 200;

[Header("Behavior")]
public bool hideWhenFull = false;
public bool destroyOnDeath = true;
public float midThreshold = 0.66f;
public float lowThreshold = 0.33f;

private HealthController hc;
private WorldHealthBar bar;

private void Awake()
{
    hc = GetComponent<HealthController>();
}

private void Start()
{
    CreateBar();
    if (hc != null && destroyOnDeath)
        hc.OnDeath += HandleDeath;
}

private void OnDestroy()
{
    if (hc != null && destroyOnDeath)
        hc.OnDeath -= HandleDeath;
}

private void HandleDeath()
{
    if (bar != null)
        Destroy(bar.gameObject);
}

private void CreateBar()
{
    if (fullSprite == null || midSprite == null || lowSprite == null || emptySprite == null)
    {
        Debug.LogError($"[{name}] AutoWorldHealthBar: assign all four sprites (full, mid, low, empty).", this);
        return;
    }

    // Decide size from empty or full
    Sprite refSprite = useBackgroundSpriteSize && emptySprite != null ? emptySprite : fullSprite;
    Vector2 worldSize = refSprite.rect.size / Mathf.Max(1f, refSprite.pixelsPerUnit);

    // Root
    var root = new GameObject($"{gameObject.name}_HealthBar");
    root.layer = gameObject.layer;
    root.transform.position = transform.position + offset;

    // Canvas
    var canvas = root.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.WorldSpace;
    canvas.worldCamera = Camera.main;
    canvas.overrideSorting = true;
    canvas.sortingOrder = sortingOrder;
    var rt = canvas.GetComponent<RectTransform>();
    rt.sizeDelta = worldSize;
    rt.pivot = new Vector2(0.5f, 0.5f);

    // TRACK (background)
    var trackGO = new GameObject("Track");
    trackGO.transform.SetParent(root.transform, false);
    var trackRT = trackGO.AddComponent<RectTransform>();
    trackRT.anchorMin = Vector2.zero;
    trackRT.anchorMax = Vector2.one;
    trackRT.offsetMin = Vector2.zero;
    trackRT.offsetMax = Vector2.zero;
    var trackImg = trackGO.AddComponent<Image>();
    trackImg.sprite = emptySprite;
    trackImg.type = Image.Type.Simple;
    trackImg.raycastTarget = false;

    // FILL (foreground)
    var fillGO = new GameObject("Fill");
    fillGO.transform.SetParent(trackGO.transform, false);
    var fillRT = fillGO.AddComponent<RectTransform>();
    fillRT.anchorMin = new Vector2(0f, 0f);
    fillRT.anchorMax = new Vector2(1f, 1f);

    // convert padding (pixels on sprite) to world offsets
    Vector2 padWorld = padding / Mathf.Max(1f, refSprite.pixelsPerUnit);
    fillRT.offsetMin = new Vector2(padWorld.x, padWorld.y);
    fillRT.offsetMax = new Vector2(-padWorld.x, -padWorld.y);

    var fillImg = fillGO.AddComponent<Image>();
    fillImg.sprite = fullSprite; // initial; WorldHealthBar will swap based on %.
    fillImg.type = Image.Type.Filled;
    fillImg.fillMethod = Image.FillMethod.Horizontal;
    fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;
    fillImg.fillAmount = 1f;
    fillImg.raycastTarget = false;

    // Logic
    bar = root.AddComponent<WorldHealthBar>();
    bar.canvas = canvas;
    bar.track = trackImg;
    bar.fill = fillImg;
    bar.worldOffset = offset;
    bar.hideWhenFull = hideWhenFull;
    bar.spriteFull = fullSprite;
    bar.spriteMid = midSprite;
    bar.spriteLow = lowSprite;
    bar.spriteEmpty = emptySprite;
    bar.midThreshold = midThreshold;
    bar.lowThreshold = lowThreshold;

    bar.Bind(hc, transform, Camera.main, sortingOrder, hideWhenFull);
}
}