using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Magic : NetworkedBehaviour
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
            ApplyDamage(other.gameObject);

            // make sure both client and host receive health update
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendDamageToClient, other.gameObject);
            }
            else
            {
                // apply host side
                InvokeServerRpc(SendDamageToHost, other.gameObject);
            }
        }



        // destroy magic projectile on collision
        Destroy(gameObject);
    }

    private void ApplyDamage(GameObject victim)
    {
        victim.GetComponent<Health>().TakeDamage(Stats.instance.magicPower);
    }

    [ServerRPC]
    private void SendDamageToHost(GameObject victim)
    {
        ApplyDamage(victim);
    }

    [ClientRPC]
    private void SendDamageToClient(GameObject victim)
    {
        ApplyDamage(victim);
    }
}