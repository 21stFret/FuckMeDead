using Micosmo.SensorToolkit;
using System;
using UnityEngine;

[Serializable]
public struct BaseWeaponInfo
{
    public int[] _damage;
    public float[] _fireRate;
    public float[] _range;
}

[Serializable]
public struct WeaponEffects
{
    public AudioSource weaponAudioSource;
    public AudioClip weaponClose;
    public AudioClip weaponLoop;

    public ParticleSystem weaponEffect;
    public GameObject weaponLights;
}

public class MechWeapon : MonoBehaviour
{
    public WeaponData weaponData;
    public BaseWeaponInfo baseWeaponInfo;
    public WeaponEffects weaponEffects;

    public int damage;
    public float fireRate;
    public float range;
    public int ammo;
    public int maxAmmo;
    public float reloadTime;
    public bool isFiring;
    [Header("Main Weapon")]
    public LOSSensor sensor;
    public Vector3 aimOffest;
    [Header("Alt Weapon")]
    public WeaponUI weaponUI;
    public float weaponFuel;
    public float weaponFuelMax = 100;
    public float weaponRechargeRate;
    public float weaponFuelUseRate;
    public Sprite fuelSprite;

    public virtual void Init()
    {
        weaponFuel = weaponFuelMax;
        SetValues();

        if(weaponData.mainWeapon)
        {
            sensor.enabled = true;
        }


        if (weaponUI == null)
        {
            return;
        }

        weaponUI.UpdateWeaponUI(weaponFuel);
        weaponUI.SetFuelImage(fuelSprite);

    }

    private void SetValues()
    {
        print("Setting values for " + name);
        damage = baseWeaponInfo._damage[weaponData.level];
        fireRate = baseWeaponInfo._fireRate[weaponData.level];
        range = baseWeaponInfo._range[weaponData.level];

    }

    private void FixedUpdate()
    {
        FuelManagement();
    }

    public virtual void Fire()
    {
        isFiring = true;
        //Debug.Log("Firing " + name);
        if (weaponEffects.weaponEffect != null)
        {
            weaponEffects.weaponEffect.Play();
        }
        weaponEffects.weaponAudioSource.clip = weaponEffects.weaponLoop;
        weaponEffects.weaponAudioSource.loop = true;
        weaponEffects.weaponAudioSource.Play();
        weaponEffects.weaponLights.SetActive(true);
        weaponUI.SetUITrigger(false);
    }

    public virtual void Stop()
    {
        isFiring = false;
        //Debug.Log("Stopping " + name);
        if (weaponEffects.weaponEffect != null)
        {
            weaponEffects.weaponEffect.Stop();
        }
        weaponEffects.weaponAudioSource.Stop();
        weaponEffects.weaponAudioSource.clip = weaponEffects.weaponClose;
        weaponEffects.weaponAudioSource.loop = false;
        weaponEffects.weaponAudioSource.Play();
        weaponEffects.weaponLights.SetActive(false);
        weaponUI.SetUITrigger(true);
    }

    private void FuelManagement()
    {
        if (isFiring)
        {
            if (weaponFuel <= 0)
            {
                Stop();
                return;
            }

            weaponFuel -= weaponFuelUseRate;

        }

        if (weaponFuel >= weaponFuelMax)
        { return; }

        weaponFuel += weaponRechargeRate;

        if (weaponUI == null)
        {
            return;
        }

        weaponUI.UpdateWeaponUI(weaponFuel);

    }
}
