using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Attack : NetworkedBehaviour
{
    private bool hasCollide = false;
    protected AttackController player;
    protected int attackPower = 0;

    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (player.IsAttacking() && !hasCollide && !hasCollide && other.gameObject != player.gameObject && other.gameObject.CompareTag("Player"))
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
    }

    private void ApplyDamage(GameObject victim)
    {
        victim.GetComponentInChildren<Health>().TakeDamage(attackPower);
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

    public void SetPlayer(AttackController P)
    {
        player = P;
    }

    public void SetPower(int P)
    {
        attackPower = P;
    }
}