using UnityEngine;

public class Magic : MonoBehaviour
{
    private bool hasCollide = false;    // true if already collided

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (!hasCollide && other.gameObject.CompareTag("Player"))
        {
            // prevent double collisions
            hasCollide = true;

            // apply damage to the player that was hit
            other.gameObject.GetComponent<PlayerController>().HP.TakeDamage(Stats.instance.magicPower);
        }

        // destroy magic projectile on collision
        Destroy(gameObject);
    }
}