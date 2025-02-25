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

    private List<EmitterCristal> _cristalToActivate = new();
    [SerializeField] private EmitterCristal _initialCristal;
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
        _lineRenderer.SetPosition(1, hitPoint);

        //StartCoroutine(LaserRayonAnim(hitPoint));
    }

    [ContextMenu("DesactivateLaserTEST")]
    private void DesactivateLaserTEST()
    {
        DesativateLaser();
    }
    private void DesativateLaser(EmitterCristal initiateur = null)
    {
        if (initiateur == null)
        {
            _initialCristal = this;
        }

        DesativateLaserRecursive(this);
    }

    private void DesativateLaserRecursive(EmitterCristal currentCristal)
    {
        if (currentCristal.isConnectedWith.Count > 0)
        {
            currentCristal.DesactivateLR(Color.white);
            List<EmitterCristal> cristauxAClean = new List<EmitterCristal>(currentCristal.isConnectedWith);

            foreach (MirrorCristal cristal in cristauxAClean)
            {
                if (cristal.cristalConnectedWith.Count >= 2)
                {
                    foreach (EmitterCristal emitter in new List<EmitterCristal>(cristal.cristalConnectedWith))
                    {
                        if (emitter == currentCristal) continue;

                        _initialCristal._cristalToActivate.Add(emitter);
                        cristal.cristalConnectedWith.Remove(emitter);
                        break;
                    }
                }

                cristal.DesactivateColor(Color.white);
                cristal.DesactivateLR(Color.white);

                if (cristal.cristalConnectedWith.Contains(currentCristal)) cristal.cristalConnectedWith.Remove(currentCristal);
                if (currentCristal.isConnectedWith.Contains(cristal)) currentCristal.isConnectedWith.Remove(cristal);

                cristal.DesactivateCristal(cristal.colorsReceived);

                if (cristal.cristalConnectedWith.Count == 0)
                {
                    DesativateLaserRecursive(cristal);
                }
            }
        }
        else
        {
            if (_initialCristal._cristalToActivate is not null)
            {
                foreach (var cristal in _initialCristal._cristalToActivate)
                {
                    cristal.SendLaser(cristal.transform.position, cristal._cristalColor, new List<Color> { cristal._cristalColor });
                }
            }
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


    //private IEnumerator LaserRayonAnim(Vector2 finalPos)
    //{
    //    float duration = 0.2f;
    //    float elapsedTime = 0f;
    //    Vector2 startPos = transform.position;

    //    while (elapsedTime < duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = elapsedTime / duration;

    //        Vector2 currentPos = Vector2.Lerp(startPos, finalPos, t);
    //        _lineRenderer.SetPosition(1, currentPos);

    //        yield return null;
    //    }

    //    _lineRenderer.SetPosition(1, finalPos);
    //}
}
