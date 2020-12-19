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

            // get the PlayerCam object (have to use a NetworkedObject)
            GameObject pcVictim = other.transform.parent.gameObject;

            // make sure both client and host receive health update
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendDamageToClient, pcVictim);
            }
            else
            {
                // apply client side
                ApplyDamage(pcVictim);

                // apply host side
                InvokeServerRpc(SendDamageToHost, pcVictim);
            }
        }

        // destroy magic projectile on collision
        Destroy(gameObject);
    }

    private void ApplyDamage(GameObject victim)
    {
        victim.GetComponentInChildren<Health>().TakeDamage(Stats.instance.magicPower);
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