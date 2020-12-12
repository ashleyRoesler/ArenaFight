using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private Attack player;              // sword owner player reference

    private bool hasCollide = false;    // true if already collided

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (player.IsAttacking() && !hasCollide && other.gameObject != player.gameObject && other.gameObject.CompareTag("Player"))
        {
            // prevent double collisions
            hasCollide = true;

            // apply damage to the player that was hit
            other.gameObject.GetComponent<PlayerController>().HP.TakeDamage(Stats.swordP);
        }
    }

    public void ResetSword()
    {
        hasCollide = false;
    }
}