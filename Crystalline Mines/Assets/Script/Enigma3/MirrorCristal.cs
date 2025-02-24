using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MirrorCristal : EmitterCristal
{
    [SerializeField] private List<Color> colorsReceived = new List<Color>();
    [SerializeField] private float _animDuration = 1.5f;
    private Color _laserColor;

    private SpriteRenderer _spriteRenderer;
    private Light2D _light;
    public void GetLaserColorReceived(List<Color> color)
    {
        foreach (Color c in color)
        {
            if (!colorsReceived.Contains(c))
            {
                colorsReceived.Add(c);
            }
        }
        SetRightColor();
    }

    private void SetRightColor()
    {
        _laserColor = GetAverageColor(colorsReceived);

        StartCoroutine(ColorTransition(_laserColor)); // Lancer l'animation de couleur

        TimerManager.StartTimer(0.75f, () => SendLaser(transform.position, _laserColor, colorsReceived));
    }
    private IEnumerator ColorTransition(Color targetColor)
    {
        Light2D light = GetComponent<Light2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color startColor = sprite.color;
        float elapsedTime = 0f;

        while (elapsedTime < _animDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _animDuration;

            Color currentColor = Color.Lerp(startColor, targetColor, t);
            light.color = currentColor;
            sprite.color = currentColor;

            yield return null; 
        }

        light.color = targetColor;
        sprite.color = targetColor;
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
