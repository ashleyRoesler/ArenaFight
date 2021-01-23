using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Attack : NetworkedBehaviour
{
    public Skill_Base Skill;

    private bool _hasCollide = false;
    private AttackController _player;

    #region Hit Target
    private void OnTriggerEnter(Collider other)
    {
        // damage player
        if (_player.IsAttacking() && !_hasCollide && !_hasCollide && other.gameObject != _player.gameObject && other.gameObject.CompareTag("Player"))
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
            else
            {
                // apply client side
                ApplyDamage(pcVictim);

                // apply host side
                InvokeServerRpc(SendDamageToHost, pcVictim);
            }
        }
    }
    #endregion

    #region Apply Damage
    private void ApplyDamage(GameObject victim)
    {
        victim.GetComponentInChildren<Health>().TakeDamage(Skill.Power);
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
    #endregion

    #region Misc.
    public void ResetCollide()
    {
        _hasCollide = false;
    }

    public void SetPlayer(AttackController P)
    {
        _player = P;
    }
    #endregion
}