using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltWeaponController : MonoBehaviour
{
    public InputActionAsset primaryActions;
    InputActionMap gameplayActionMap;
    public PlayerInput playerInput;
    public InputAction FireInputAction;
    public MechWeapon equipedWeapon;

    public float aimZ;
    public float aimX;
    public float rotationSpeed;

    public bool isAiming;

    public void Init(MechWeapon mechWeapon)
    {
        equipedWeapon = mechWeapon;
        ResetAim();
        SetWeaponInputs();
    }

    private void SetWeaponInputs()
    {
        gameplayActionMap = primaryActions.FindActionMap("Gameplay");
        FireInputAction = gameplayActionMap.FindAction("Fire");
        FireInputAction.performed += GetFireInput;
        FireInputAction.canceled += FireRelease;
    }

    public void ClearWeaponInputs()
    {
        FireInputAction.performed -= GetFireInput;
        FireInputAction.canceled -= FireRelease;
    }

    private void GetFireInput(InputAction.CallbackContext context)
    {
        Fire();
    }

    private void FireRelease(InputAction.CallbackContext context)
    {
        Stop();
    }
    public void Fire()
    {
        equipedWeapon.Fire();
    }

    public void Stop()
    {
        equipedWeapon.Stop();
    }

    public void Update()
    {
        Aiming();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();
        var cam = Camera.main;
        if (playerInput.currentControlScheme == "PC")
        {
            movementVector.x -= cam.pixelWidth / 2;
            movementVector.y -= cam.pixelHeight / 2;
        }

        //movementVector.Normalize();
        if (movementVector == Vector2.zero)
        {
            ResetAim();
            return;
        }
        isAiming = true;
        aimX = movementVector.x;
        aimZ = movementVector.y;
    }

    private void Aiming()
    {
        if (!isAiming)
        {
            return;
        }
        Vector3 lookDirection = new Vector3(aimX, 0, aimZ);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        equipedWeapon.transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, rotationSpeed * Time.deltaTime);
    }

    private void ResetAim()
    {
        isAiming = false;
        aimX = 1;
        aimZ = 1;
    }


}
