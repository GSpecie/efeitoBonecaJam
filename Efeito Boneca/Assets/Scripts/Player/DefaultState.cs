using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DefaultState : IPlayerStates
{
    Player player;

    public DefaultState(Player playerObj)
    {
        player = playerObj;
    }

    public void CheckInput()
    {
        player.leftJoystick = player.actionMap.Default.Movement.ReadValue<Vector2>();
        player.rightJoystick = player.actionMap.Default.RightStickDirection.ReadValue<Vector2>();
        player.shootButton = player.actionMap.Default.Shoot.triggered;
        player.dashButton = player.actionMap.Default.Dash.triggered;

        if (player.shootButton) Shoot();
        if (player.dashButton) Dash();
    }

    public void Shoot()
    {
        if(player.cooldownToShoot <= 0 && player.currentShootEnergy > 15f)
        {
            player.cooldownToShoot = player.cooldownToShootOriginal;
            player.currentShootEnergy -= 30f;
            player.currentDashEnergy += 15f;
            Recoil();
            player.sFX_shoot.Play();
            player.muzzleVFX.Play();
            player.animatorChar.SetTrigger("Shoot");

            player.bullets[player.bulletIndex].transform.position = player.bulletPoint.position;
            player.bullets[player.bulletIndex].transform.rotation = player.bulletPoint.rotation;
            player.bullets[player.bulletIndex].ResetTiming();
            player.bullets[player.bulletIndex].gameObject.SetActive(true);
            player.bulletIndex++;
            if (player.bulletIndex == player.bullets.Length - 1) player.bulletIndex = 0;
        }
    }

    public void Recoil()
    {
        player.recoilForce += -player.playerChar.transform.forward * player.recoilPower;
    }

    public void EnergyBars()
    {
        player.animatorChar.SetFloat("EnergyMovement", player.currentDashEnergy + player.currentShootEnergy);
        //Debug.Log(player.currentDashEnergy + player.currentShootEnergy);
        MovementEnergy();
        ShootEnergy();
    }

    public void MovementEnergy()
    {
        player.dashEnergyPercentage = player.currentDashEnergy / player.maximumDashEnergy;
        player.currentDashBar.fillAmount = player.dashEnergyPercentage;

        if (player.cooldownToDash <= 0) player.currentDashEnergy += 0.1f;

        if (player.currentDashEnergy >= player.maximumDashEnergy)
        {
            player.currentDashEnergy = player.maximumDashEnergy;
            player.dashPower = player.superDash;
        }
        else if (player.currentDashEnergy >= 71)
        {

            player.dashPower = player.highDash;
        }
        else if (player.currentDashEnergy >= 41)
        {

            player.dashPower = player.mediumDash;
        }
        else if (player.currentDashEnergy >= 11)
        {

            player.dashPower = player.mininumDash;
        }

        if (player.currentDashEnergy <= 0)
        {
            player.currentDashEnergy = 0;
        }
    }

    public void ShootEnergy()
    {
        player.shootEnergyPercentage = player.currentShootEnergy / player.maximumShootEnergy;
        player.currentShootBar.fillAmount = player.shootEnergyPercentage;

        if (player.cooldownToShoot <= 0) player.currentShootEnergy += 0.1f;

        if (player.currentShootEnergy >= player.maximumDashEnergy)
        {
            player.currentShootEnergy = player.maximumShootEnergy;
            player.recoilPower = player.superRecoil;
        }
        else if (player.currentShootEnergy >= 71)
        {
            player.moveSpeed = player.highSpeed;
            player.recoilPower = player.highRecoil;
        }
        else if (player.currentShootEnergy >= 41)
        {
            player.moveSpeed = player.mediumSpeed;
            player.recoilPower = player.mediumRecoil;
        }
        else if (player.currentShootEnergy >= 11)
        {
            player.moveSpeed = player.minimumSpeed;
            player.recoilPower = player.mininumRecoil;
        }

        if (player.currentShootEnergy <= 0)
        {
            player.currentShootEnergy = 0;
        }
    }

    public void Move()
    {
        //player.moveSpeed = 15f;

        Vector3 leftHorizontalMovement = player.horizontal * player.leftJoystick.x;
        Vector3 leftVerticalMovement = player.vertical * player.leftJoystick.y;

        player.direction = (leftHorizontalMovement + leftVerticalMovement).normalized;

        if (player.direction != Vector3.zero) player.playerChar.transform.forward = player.direction;

        Vector3 fakeGravity = new Vector3(0, Physics.gravity.y, 0) * Time.deltaTime;

        player.rb.velocity = fakeGravity + player.direction * player.moveSpeed + player.externalForce + player.dashForce + player.recoilForce;

        if (player.isGamepad == true)
        {
            if (player.rightJoystick.x != 0) player.lastRightJoystickHorizontal = player.rightJoystick.x;

            player.rightHorizontalMovement = player.rightHorizontal * player.moveSpeed * Time.deltaTime * player.lastRightJoystickHorizontal;

            if (player.rightJoystick.y != 0) player.lastRightJoystickVertical = player.rightJoystick.y;

            player.rightVerticalMovement = player.rightVertical * player.moveSpeed * Time.deltaTime * player.lastRightJoystickVertical;

            player.playerFacingDirection = (player.rightHorizontalMovement + player.rightVerticalMovement);

            Vector3 newFaceLook = new Vector3(player.playerFacingDirection.x, 0, player.playerFacingDirection.z);

            if (player.playerFacingDirection != Vector3.zero) player.playerChar.transform.forward = player.playerFacingDirection;
        }
        else if (player.isGamepad == false)
        {
            player.cameraRay = Camera.main.ScreenPointToRay(player.actionMap.Default.MouseDirection.ReadValue<Vector2>());

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLenght;

            if (groundPlane.Raycast(player.cameraRay, out rayLenght))
            {
                Vector3 pointToLook = player.cameraRay.GetPoint(rayLenght);
                Debug.DrawLine(player.cameraRay.origin, pointToLook, Color.blue);

                player.playerChar.transform.LookAt(new Vector3(pointToLook.x, player.transform.position.y, pointToLook.z));
            }
        }

        player.animatorChar.SetFloat("PositionX", player.leftJoystick.x);
        player.animatorChar.SetFloat("PositionY", player.leftJoystick.y);

        if (player.actionMap.Default.Movement.ReadValue<Vector2>().x != 0 || player.actionMap.Default.Movement.ReadValue<Vector2>().y != 0)
        {
            player.animatorChar.SetBool("IsRunning", true);
        }
        else player.animatorChar.SetBool("IsRunning", false);
    }
    public void TakeDamage (Vector3 impactValue)
    {
        player.externalForce += impactValue;

        player.animatorChar.SetTrigger("TakeDamage");

        if (player.isFallen == false)
        {
            player.isFallen = true;
            player.sFX_identityFallen.Play();
            player.myLife.AttVisualFeedback();
        }
        
    }


    public void Fallen()
    {
            player.animatorIdentity.SetBool("IsFallen", true);
            player.myMeshRenderer.material.mainTexture = player.dollTexture;
            //player.cooldownToRaise -= Time.fixedDeltaTime;
    }

    public void CheckCasts()
    {
  
    }

    public void ChangesInStates()
    {
        //others
        player.rb.isKinematic = false;
    }

    public void DropCooldown()
    {
        player.cooldownToDash -= Time.fixedDeltaTime;
        player.cooldownToShoot -= Time.fixedDeltaTime;
    }

    public void CreatePlayerImpact(Vector3 ImpactValue)
    {
        player.externalForce += ImpactValue;
    }

    public void Dash()
    {
        if (player.cooldownToDash <=0 && player.currentDashEnergy > 10f)
        {
            Vector3 leftHorizontalMovement = player.horizontal * player.leftJoystick.x;
            Vector3 leftVerticalMovement = player.vertical * player.leftJoystick.y;

            player.direction = (leftHorizontalMovement + leftVerticalMovement).normalized;

            //player.CharacterAnim.SetTrigger("dash");

            //TakeDamage(5f);
            player.cooldownToDash = player.cooldownToDashOriginal;
            player.currentDashEnergy -= 30f;
            player.currentShootEnergy += 15f;
            player.dashVFX.Play();
            player.sFX_dash.Play();
            player.animatorChar.SetTrigger("Dash");

            if (player.leftJoystick != Vector2.zero)
            {
                player.dashForce += player.direction * player.dashPower;
            }
            else
            {
                player.dashForce += player.playerChar.transform.forward * player.dashPower;
            }
        }

    }
}
