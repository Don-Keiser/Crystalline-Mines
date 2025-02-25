using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmitterCristal : MonoBehaviour
{
    [SerializeField] protected Vector2 _cristalDir = Vector2.right;
    [SerializeField] protected List<EmitterCristal> cristalConnectedWith = new();
    [SerializeField] protected List<EmitterCristal> isConnectedWith = new();

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

        if (!isConnectedWith.Contains(hittedCristal))
        {
            isConnectedWith.Add(hittedCristal);
        }
        if (!hittedCristal.cristalConnectedWith.Contains(this))
        {
            hittedCristal.cristalConnectedWith.Add(this);
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
    private void DesativateLaser()
    {
        if (isConnectedWith.Count > 0)
        {
            DesactivateLR(Color.white);

            List<EmitterCristal> cristauxAClean = new List<EmitterCristal>(isConnectedWith);

            foreach (MirrorCristal cristal in cristauxAClean)
            {
                if (cristal.isConnectedWith.Count < 2)
                {
                    // Debug pour vérifier l'état du cristal avant modification
                    Debug.Log("Désactivation de : " + cristal.name);

                    cristal.DesactivateColor(Color.white);
                    cristal.DesactivateLR(Color.white);

                    // Suppression après désactivation
                    if (cristal.cristalConnectedWith.Contains(this)) { cristal.cristalConnectedWith.Remove(this); }
                    if (this.isConnectedWith.Contains(cristal)) { this.isConnectedWith.Remove(cristal); }

                    cristal.DesactivateCristal(cristal.colorsReceived);

                    // Vérifier que le cristal a bien été mis à jour
                    Debug.Log("Couleur après désactivation: " + cristal.GetComponent<SpriteRenderer>().color);

                    // Éviter une récursion infinie en s’assurant que l'on ne rappelle pas sur un cristal déjà désactivé
                    if (cristal.cristalConnectedWith.Count == 0)
                    {
                        cristal.DesativateLaser();
                    }
                }
            }
        }
        else
        {
            Debug.Log("Fin de la désactivation.");
            return;
        }
    }


    private void DesactivateColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<Light2D>().color = color;
    }
    private void DesactivateLR(Color color)
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
