using UnityEngine;

public class Magic : MonoBehaviour
{
    private bool hasCollide = false;

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (!hasCollide && other.gameObject.tag == "Player")
        {
            // prevent double collisions
            hasCollide = true;

            // apply damage to the player that was hit
            other.gameObject.GetComponent<PlayerController>().HP.TakeDamage(Stats.magicP);
        }

        // destroy magic projectile on collision
        Destroy(gameObject);
    }
}