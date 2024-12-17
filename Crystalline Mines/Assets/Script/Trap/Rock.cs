using UnityEngine;

public class Rock : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.layer == LayerMask.NameToLayer("TrapCrystal"))
            {
                Animation.Instance.DeadTrapCrystalAnimation();
                TimerManager.StartTimer(0.5f, (() => collision.gameObject.GetComponent<Player>().Respawn()));
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
