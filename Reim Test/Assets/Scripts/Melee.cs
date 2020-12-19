using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Melee : NetworkedBehaviour
{
    [SerializeField]
    private Attack player;                      // player reference

    private bool hasCollide = false;            // true if already collided

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (player.IsAttacking() && !hasCollide && other.gameObject != player.gameObject && other.gameObject.CompareTag("Player"))
        {
            // prevent double collisions
            hasCollide = true;

            // get the PlayerCam object (have to use a NetworkedObject)
            GameObject pcVictim = other.transform.parent.gameObject;

            // apply damage to the player that was hit
            ApplyDamage(pcVictim);

            // make sure both client and host receive health update
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendDamageToClient, pcVictim);
            }
            else
            {
                // apply host side
                InvokeServerRpc(SendDamageToHost, pcVictim);
            }
        }
    }

    private void ApplyDamage(GameObject victim)
    {
        if (player.GetSwordToggle())    // sword damage
        {
            victim.GetComponentInChildren<Health>().TakeDamage(Stats.instance.swordPower);
        }
        else    // punch damage
        {
            victim.GetComponentInChildren<Health>().TakeDamage(Stats.instance.punchPower);
        }
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

    public void ResetCollide()
    {
        hasCollide = false;
    }
}