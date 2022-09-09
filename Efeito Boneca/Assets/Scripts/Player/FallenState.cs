using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenState : IPlayerStates
{
    Player player;

    public FallenState(Player playerObj)
    {
        player = playerObj;
    }


    public void ChangesInStates()
    {
        throw new System.NotImplementedException();
    }

    public void CheckCasts()
    {
        throw new System.NotImplementedException();
    }

    public void CheckInput()
    {
        throw new System.NotImplementedException();
    }


    public void Fallen()
    {

    }

    public void CreatePlayerImpact(Vector3 impactValue)
    {
        throw new System.NotImplementedException();
    }

    public void Dash()
    {
        throw new System.NotImplementedException();
    }

    public void DropCooldown()
    {
        throw new System.NotImplementedException();
    }

    public void EnergyBars()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(Vector3 impactValue)
    {
        throw new System.NotImplementedException();
    }
}
