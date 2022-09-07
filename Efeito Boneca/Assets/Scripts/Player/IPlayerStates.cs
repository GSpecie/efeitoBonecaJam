using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStates
{
    /// <summary>
    /// Checa o input do jogador
    /// </summary>
    void CheckInput();
    /// <summary>
    /// Movimenta o jogador
    /// </summary>
    void Move();
    /// <summary>
    /// Causa dano ao jogador
    /// </summary>
    /// <param name="damage">valor do dano</param>
    void TakeDamage(float damage);
    /// <summary>
    /// checa coisas como raycasts
    /// </summary>
    void CheckCasts();
    /// <summary>
    /// Atribui a barra de vida/energia ao jogador
    /// </summary>
    void EnergyBars();
    /// <summary>
    /// mudanças simples ao entrar em cada estado, tais como desativar e ativar um objeto
    /// </summary>
    void ChangesInStates();
    void DropCooldown();
    void CreatePlayerImpact(Vector3 impactValue);
    //void Dash();
}
