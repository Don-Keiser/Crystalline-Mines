using System;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interact_Crystal crystal = collision.GetComponent<Interact_Crystal>();
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Respawn();
        }
        else if(crystal != null)
        {
            collision.transform.position = crystal.initialPosition;
            crystal.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            crystal.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
