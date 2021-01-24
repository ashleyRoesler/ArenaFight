using UnityEngine;

public class RangedAttack : Attack
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // damage player
        if (!_hasCollide  && other.gameObject.CompareTag("Player"))
        {
            // prevent double collisions
            _hasCollide = true;

            // get the PlayerCam object (have to use a NetworkedObject)
            GameObject pcVictim = other.transform.parent.gameObject;

            // make sure both client and host receive health update
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendDamageToClient, pcVictim);
            }
        }
    }
}