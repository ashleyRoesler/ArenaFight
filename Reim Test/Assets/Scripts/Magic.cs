using UnityEngine;

public class Magic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (other.gameObject.tag == "Player")
        {
            // get reference to the player controller that was hit
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            // apply damage
            player.HP.TakeDamage(Attack.magicP);
        }

        // destroy magic projectile on collision
        Destroy(gameObject);
    }
}