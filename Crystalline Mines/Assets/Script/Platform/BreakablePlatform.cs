using System.Collections;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _platformRenderer;
    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        StartCoroutine(WaitForBreak());
    }

    IEnumerator WaitForBreak()
    {
        yield return new WaitForSeconds(2f);
        _platformRenderer.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    private void OnTriggerExit2D(Collider2D collider2d)
    {
        StartCoroutine(WaitForRespawn());
    }
    
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(10f);
        _platformRenderer.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Floor");
    }
}
