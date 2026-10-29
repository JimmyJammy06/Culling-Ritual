using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Debug.Log("Well well well");
        }
    }
}
