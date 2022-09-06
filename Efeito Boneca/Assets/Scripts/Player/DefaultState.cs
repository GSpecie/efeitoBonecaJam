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
        //if (player.dashButton && player.currentHealth > 10f) Dash();
    }

    public void Shoot()
    {
        player.muzzleVFX.Play();
        player.bullets[player.bulletIndex].transform.position = player.bulletPoint.position;
        player.bullets[player.bulletIndex].transform.rotation = player.bulletPoint.rotation;
        player.bullets[player.bulletIndex].ResetTiming();
        player.bullets[player.bulletIndex].gameObject.SetActive(true);
        player.bulletIndex++;
        if (player.bulletIndex == player.bullets.Length - 1) player.bulletIndex = 0;
    }

    public void Life()
    {
        player.healthPercentage = player.currentHealth / player.maximumHealth;
        //player.currentHealthBar.rectTransform.localScale = new Vector3(1, player.healthPercentage, 1);
        //player.currentHealthBar.fillAmount = player.healthPercentage;


        if (player.currentHealth >= player.maximumHealth) player.currentHealth = player.maximumHealth;

        if (player.currentHealth <= 0)
        {
            player.currentHealth = 0;
        }
    }

    public void Move()
    {
        player.moveSpeed = 15f;

        Vector3 leftHorizontalMovement = player.horizontal * player.leftJoystick.x;
        Vector3 leftVerticalMovement = player.vertical * player.leftJoystick.y;

        player.direction = (leftHorizontalMovement + leftVerticalMovement).normalized;

        if (player.direction != Vector3.zero) player.transform.forward = player.direction;

        Vector3 fakeGravity = new Vector3(0, Physics.gravity.y, 0) * Time.deltaTime;

        player.rb.velocity = fakeGravity + player.direction * player.moveSpeed + player.externalForce + player.dashPower;

        if (player.isGamepad == true)
        {
            if (player.rightJoystick.x != 0) player.lastRightJoystickHorizontal = player.rightJoystick.x;

            player.rightHorizontalMovement = player.rightHorizontal * player.moveSpeed * Time.deltaTime * player.lastRightJoystickHorizontal;

            if (player.rightJoystick.y != 0) player.lastRightJoystickVertical = player.rightJoystick.y;

            player.rightVerticalMovement = player.rightVertical * player.moveSpeed * Time.deltaTime * player.lastRightJoystickVertical;

            player.playerFacingDirection = (player.rightHorizontalMovement + player.rightVerticalMovement);

            Vector3 newFaceLook = new Vector3(player.playerFacingDirection.x, 0, player.playerFacingDirection.z);

            if (player.playerFacingDirection != Vector3.zero) player.transform.forward = player.playerFacingDirection;
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

                player.transform.LookAt(new Vector3(pointToLook.x, player.transform.position.y, pointToLook.z));
            }
        }

        //player.CharacterAnim.SetFloat("positionX", player.leftJoystickHorizontal);
        //player.CharacterAnim.SetFloat("positionY", player.leftJoystickHorizontal);

        //if (player.leftJoystickHorizontal != 0 || player.leftJoystickVertical != 0)
        //{
        //    player.CharacterAnim.SetBool("isRunning", true);
        //}
        //else player.CharacterAnim.SetBool("isRunning", false);
    }
    public void TakeDamage(float damage)
    {
        player.currentHealth -= damage;
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
    }

    public void CreatePlayerImpact(Vector3 ImpactValue)
    {
        player.externalForce += ImpactValue;
    }

    public void Dash()
    {
        Vector3 leftHorizontalMovement = player.horizontal * player.leftJoystick.x;
        Vector3 leftVerticalMovement = player.vertical * player.leftJoystick.y;

        player.direction = (leftHorizontalMovement + leftVerticalMovement).normalized;

        //player.CharacterAnim.SetTrigger("dash");

        TakeDamage(5f);
        if (player.leftJoystick != Vector2.zero)
        {
            player.dashPower += player.direction * player.dashMultiplier;
        }
        else
        {
            player.dashPower += player.transform.forward * player.dashMultiplier;
        }
    }
}
