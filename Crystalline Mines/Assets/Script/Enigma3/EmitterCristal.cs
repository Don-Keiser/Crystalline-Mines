using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmitterCristal : MonoBehaviour
{
    [SerializeField] protected Vector2 _cristalDir = Vector2.right;
    protected LineRenderer _lineRenderer;
    protected Color _cristalColor;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _cristalColor = GetComponent<Light2D>().color;
    }
    [ContextMenu("SendLaserEmitterTEST")]
    private void SendLaserEmitterTEST()
    {
        SendLaser(transform.position, _cristalColor, new List<Color> {_cristalColor});
    }
    protected virtual void ClearMirrorColorList(MirrorCristal hittedCristal) { }
    protected void SendLaser(Vector2 cristalPos, Color laserColor, List<Color> colorsToSend)
    {
        Vector2 hitPos = Vector3.zero;
        RaycastHit2D hit = CreateRay(cristalPos);
        if (!hit) { return; }

        hitPos = hit.collider.transform.position;

        if (!hit.collider.gameObject.TryGetComponent(out MirrorCristal hittedCristal)) { return; }

        ClearMirrorColorList(hittedCristal);
        hittedCristal.GetLaserColorReceived(colorsToSend);
        SendLineRenderer(hitPos, laserColor);
    }
    private void SendLineRenderer(Vector2 hitPoint, Color laserColor)
    {
        if (_lineRenderer == null) { return; }

        _lineRenderer.startColor = laserColor;
        _lineRenderer.endColor = laserColor;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, hitPoint);
    }

    private RaycastHit2D CreateRay(Vector2 cristalPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(cristalPos, _cristalDir, Mathf.Infinity);
        Debug.DrawLine(cristalPos, _cristalDir * 1000f, Color.red, 5f);
        return hit;
    }

}
