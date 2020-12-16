using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField]
    private Attack player;              // player reference

    private bool hasCollide = false;    // true if already collided

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (player.IsAttacking() && !hasCollide && other.gameObject != player.gameObject && other.gameObject.CompareTag("Player"))
        {
            // prevent double collisions
            hasCollide = true;

            // apply damage to the player that was hit
            if (player.GetSwordToggle())    // sword damage
            {
                other.gameObject.GetComponent<PlayerController>().HP.TakeDamage(Stats.swordP);
            }
            else                           // punch damage
            {
                other.gameObject.GetComponent<PlayerController>().HP.TakeDamage(Stats.punchP);
            }
        }
    }

    public void ResetCollide()
    {
        hasCollide = false;
    }
}