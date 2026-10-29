using System.Runtime.CompilerServices;
using UnityEngine;

public class Damage : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // do damage
        }
        else if (collision.gameObject.CompareTag("Player"))
        {

        }
            Destroy(this.gameObject);
    }
}
