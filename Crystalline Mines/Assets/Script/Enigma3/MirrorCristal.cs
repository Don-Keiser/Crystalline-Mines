using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MirrorCristal : EmitterCristal
{
    [SerializeField] private List<Color> colorsReceived = new List<Color>();
    [SerializeField] private List<MirrorCristal> _cristalConnected = new();
    [SerializeField] private float _animDuration = 1f;
    private Color _laserColor;

    private SpriteRenderer _spriteRenderer;
    private Light2D _light;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
    }
    public void GetLaserColorReceived(List<Color> color)
    {
        foreach (Color c in color)
        {
            if (!colorsReceived.Contains(c))
            {
                colorsReceived.Add(c);
            }
        }
        StartCoroutine(FillCristalColor());
    }
    private IEnumerator FillCristalColor()
    {
        SetRightColor();
        Color lightColor = _light.color;
        Color spriteColor = _spriteRenderer.color;
        for (float t = _animDuration; t > 0.01f; t -= Time.deltaTime)
        {
            lightColor.a += t;
            spriteColor.a += t;

            _light.color = lightColor;
            _spriteRenderer.color = spriteColor;
            yield return null;
        }

        lightColor.a = 1f;
        _light.color = lightColor;

        spriteColor.a = 1f;
        _spriteRenderer.color = lightColor;
    }

    private void SetRightColor()
    {
        _laserColor = GetAverageColor(colorsReceived);
        _laserColor.a = 0;

        _light.color = _laserColor;
        _spriteRenderer.color = _laserColor;

       TimerManager.StartTimer(_animDuration, () =>
       {
           _laserColor.a = 1;
           SendLaser(transform.position, _laserColor, colorsReceived);
       });
    }

    protected override void ClearMirrorColorList(MirrorCristal hittedCristal)
    {
        base.ClearMirrorColorList(hittedCristal); //Empty

        if (!_cristalConnected.Contains(hittedCristal))
        {
            _cristalConnected.Add(hittedCristal);
        }
    }
    private Color GetAverageColor(List<Color> colors)
    {
        if (colors.Count <= 0) { return new Color(0, 0, 0, 0); }

        Vector3 rgbColor = Vector3.zero;
        foreach (var color in colors)
        {
            rgbColor.x += color.r;
            rgbColor.y += color.g;
            rgbColor.z += color.b;
        }
        rgbColor.x = rgbColor.x == 0 ? 0 : rgbColor.x / colors.Count;
        rgbColor.y = rgbColor.y == 0 ? 0 : rgbColor.y / colors.Count;
        rgbColor.z = rgbColor.z == 0 ? 0 : rgbColor.z / colors.Count;

        return new Color(rgbColor.x, rgbColor.y, rgbColor.z, 1);
    }
}
