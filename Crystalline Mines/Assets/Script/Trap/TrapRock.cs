using System.Collections;
using UnityEngine;

public class Roch : MonoBehaviour
{
    private bool _spawned;
    [SerializeField] private GameObject _roch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_spawned == false)
        {
            Instantiate(_roch, transform.position, transform.rotation);
            StartCoroutine(WaitForSpawn());
        }
    }

    IEnumerator WaitForSpawn()
    {
        _spawned = true;
        yield return new WaitForSeconds(2f);
        _spawned = false;
    }
}
