using System.Collections;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _platformRenderer;
    [SerializeField] private float _timebreak;
    [SerializeField] private float _timerespawn;
    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        StartCoroutine(WaitForBreak());
    }

    IEnumerator WaitForBreak()
    {
        yield return new WaitForSeconds(_timebreak);
        _platformRenderer.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    private void OnTriggerExit2D(Collider2D collider2d)
    {
        StartCoroutine(WaitForRespawn());
    }
    
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(_timerespawn);
        _platformRenderer.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Floor");
    }
}
