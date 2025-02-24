using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmitterCristal : MonoBehaviour
{
    [SerializeField] protected Vector2 _cristalDir = Vector2.right;
    [SerializeField] protected List<EmitterCristal> cristalConnected = new();

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
        SendLaser(transform.position, _cristalColor, new List<Color> { _cristalColor });
    }
    protected void SendLaser(Vector2 cristalPos, Color laserColor, List<Color> colorsToSend)
    {
        Vector2 hitPos = Vector3.zero;
        RaycastHit2D hit = CreateRay(cristalPos);
        if (!hit) { return; }

        hitPos = hit.collider.transform.position;

        if (!hit.collider.gameObject.TryGetComponent(out MirrorCristal hittedCristal)) { return; }

        if (!cristalConnected.Contains(hittedCristal))
        {
            cristalConnected.Add(hittedCristal);
        }
        hittedCristal.GetLaserColorReceived(colorsToSend);
        SendLineRenderer(hitPos, laserColor);
    }
    private RaycastHit2D CreateRay(Vector2 cristalPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(cristalPos, _cristalDir, Mathf.Infinity);
        Debug.DrawLine(cristalPos, _cristalDir * 1000f, Color.red, 5f);
        return hit;
    }
    private void SendLineRenderer(Vector2 hitPoint, Color laserColor)
    {
        if (_lineRenderer == null) { return; }

        _lineRenderer.startColor = laserColor;
        _lineRenderer.endColor = laserColor;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);

        StartCoroutine(LaserRayonAnim(hitPoint));
    }

    [ContextMenu("DesactivateLaser")]
    protected void DesactivateLaser()
    {
        if (cristalConnected.Count > 0)
        {
            ResetLineRendererColor(Color.white);
            List<EmitterCristal> cristauxAClean = new List<EmitterCristal>();
            cristauxAClean = cristalConnected;

            foreach (MirrorCristal cristal in cristauxAClean)
            {
                if(cristal.cristalConnected.Count > 1)
                {
                    EmitterCristal emitter = new EmitterCristal();
                }
                ResetLineRendererColor(Color.white);
                ResetCristalColor(cristal, Color.white);
                cristal.DesactivateLaser();

                cristal.cristalConnected.Remove(this);
            }
            cristalConnected.Clear();
        }
    }

    private void ResetCristalColor(MirrorCristal cristal, Color color)
    {
        cristal.GetComponent<Light2D>().color = color;
        cristal.GetComponent<SpriteRenderer>().color = color;
    }

    private void ResetLineRendererColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        _lineRenderer.SetPosition(0, Vector2.zero);
        _lineRenderer.SetPosition(1, Vector2.zero);
    }

    private IEnumerator LaserRayonAnim(Vector2 finalPos)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Vector2 startPos = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Vector2 currentPos = Vector2.Lerp(startPos, finalPos, t);
            _lineRenderer.SetPosition(1, currentPos);

            yield return null;
        }

        _lineRenderer.SetPosition(1, finalPos);
    }
}
