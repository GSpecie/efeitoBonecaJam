using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IPlayerStates
{
    Player player;

    public DeadState(Player playerObj)
    {
        player = playerObj;
    }

    public void ChangesInStates()
    {
    }

    public void CheckCasts()
    {
    }

    public void CheckInput()
    {
    }

    public void CreatePlayerImpact(Vector3 impactValue)
    {
    }

    public void Fallen()
    {

    }
    public void Dash()
    {
    }

    public void DropCooldown()
    {
    }

    public void EnergyBars()
    {
    }

    public void Move()
    {
        player.rb.velocity = Vector3.zero;
    }

    public void TakeDamage(Vector3 impactValue)
    {
    }
}
