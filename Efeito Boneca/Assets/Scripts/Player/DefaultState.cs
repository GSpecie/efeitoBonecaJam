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
        //player.rightJoystickHorizontal = player.actionMap.Default.Direction.ReadValue<float>();
        //player.rightJoystickVertical = player.actionMap.Default.Direction.ReadValue<float>();

        //player.dashButton = player.actionMap.Default.Dash.ReadValue<InputBinding>();

        //player.cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;

        if (groundPlane.Raycast(player.cameraRay, out rayLenght))
        {
            Vector3 pointToLook = player.cameraRay.GetPoint(rayLenght);
            Debug.DrawLine(player.cameraRay.origin, pointToLook, Color.blue);

            player.transform.LookAt(new Vector3(pointToLook.x, player.transform.position.y, pointToLook.z));
        }

        //if (player.dashButton && player.currentHealth > 10f) Dash();
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


        Vector3 fakeGravity = new Vector3(0, Physics.gravity.y, 0) * Time.deltaTime;

        player.rb.velocity = fakeGravity + player.direction * player.moveSpeed + player.externalForce + player.dashPower;

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
