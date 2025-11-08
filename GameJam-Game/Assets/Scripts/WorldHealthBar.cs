using Nidavellir;
using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
// UI references (assigned by the spawner)
public Canvas canvas;
public Image track; // background (empty)
public Image fill; // foreground (filled)

// Behavior
public Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);
public bool billboard = true;
public bool hideWhenFull = false;

// Sprites and thresholds (assigned by the spawner)
public Sprite spriteFull;
public Sprite spriteMid;
public Sprite spriteLow;
public Sprite spriteEmpty;
public float midThreshold = 0.66f;  // > midThreshold -> full sprite
public float lowThreshold = 0.33f;  // > lowThreshold -> mid sprite, else low sprite

private HealthController source;
private Transform follow;
private Camera cam;

public void Bind(HealthController src, Transform followTarget, Camera worldCamera, int sortingOrder, bool hideAtFull)
{
    source = src;
    follow = followTarget;
    cam = worldCamera != null ? worldCamera : Camera.main;
    hideWhenFull = hideAtFull;

    if (canvas != null)
    {
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = cam;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;
    }

    // initial visuals
    ApplyPercent(src != null ? src.HealthPercent : 1f);

    if (source != null)
        source.OnHealthPercentChanged += ApplyPercent;
}

private void OnDestroy()
{
    if (source != null)
        source.OnHealthPercentChanged -= ApplyPercent;
}

private void LateUpdate()
{
    if (follow != null)
        transform.position = follow.position + worldOffset;

    if (billboard)
    {
        if (cam == null) cam = Camera.main;
        if (cam != null) transform.rotation = cam.transform.rotation;
    }
}

private void ApplyPercent(float pct)
{
    pct = Mathf.Clamp01(pct);

    if (fill != null)
    {
        // choose sprite based on thresholds
        if (pct > midThreshold && spriteFull != null)      fill.sprite = spriteFull;
        else if (pct > lowThreshold && spriteMid != null)  fill.sprite = spriteMid;
        else if (spriteLow != null)                        fill.sprite = spriteLow;

        fill.fillAmount = pct;
    }

    if (track != null && spriteEmpty != null && track.sprite != spriteEmpty)
        track.sprite = spriteEmpty;

    if (hideWhenFull && track != null)
        track.gameObject.SetActive(pct < 0.999f);
}
}