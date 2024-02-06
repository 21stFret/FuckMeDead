using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager instance;
    public GameObject weaponsHolder;
    public MechWeapon[] _mainWeapons;
    public MechWeapon[] _altWeapons;
    public int mainWeapon;
    public int altWeapon;


    private void Awake()
    {
        // Create a singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy the duplicate instance
            Destroy(gameObject);
        }

    }

    public void SetMainWeaponIndex(int index)
    {
        mainWeapon = index;
        PlayerSavedData.instance.playerLoadout.x = index;
        PlayerSavedData.instance.SavePlayerData();
    }

    public void SetAltWeaponIndex(int index)
    {
        altWeapon = index;
        PlayerSavedData.instance.playerLoadout.y = index;
        PlayerSavedData.instance.SavePlayerData();
    }

    public void LoadWeaponsData(WeaponData[] mainWeapons, WeaponData[] altWeapons)
    {
        for (int i = 0; i < mainWeapons.Length; i++)
        {
            _mainWeapons[i].weaponData = mainWeapons[i];
        }
        for (int i = 0; i < altWeapons.Length; i++)
        {
            _altWeapons[i].weaponData = altWeapons[i];
        }

        mainWeapon = (int)PlayerSavedData.instance.playerLoadout.x;
        altWeapon = (int)PlayerSavedData.instance.playerLoadout.y;
    }

    public void UpdateWeaponData()
    {
        for (int i = 0; i < _mainWeapons.Length; i++)
        {
            PlayerSavedData.instance.UpdateMainWeaponData(_mainWeapons[i].weaponData, i);
        }
        for (int i = 0; i < _altWeapons.Length; i++)
        {
            PlayerSavedData.instance.UpdateAltWeaponData(_altWeapons[i].weaponData, i);
        }
    }

    public void GetWeaponsFromHolder(ConnectWeaponHolderToManager holder)
    {
        weaponsHolder = holder.gameObject;
        _mainWeapons = holder.mainWeapons;
        _altWeapons = holder.altWeapons;
    }
    
    public void UnlockWeapon(int index, bool mainWeapon)
    {
        var weapon = mainWeapon ? _mainWeapons[index] : _altWeapons[index];
        weapon.weaponData.unlocked = true;
        UpdateWeaponData();
    }

    public void LevelUpWeapon(int index, bool mainWeapon)
    {
        var weapon = mainWeapon ? _mainWeapons[index] : _altWeapons[index];
        if (weapon.baseWeaponInfo._cost[weapon.weaponData.level] >= PlayerSavedData.instance._playerCash)
        {
            return;
        }
        PlayerSavedData.instance._playerCash -= weapon.baseWeaponInfo._cost[weapon.weaponData.level];
        weapon.weaponData.level++;
        weapon.weaponData.exp = 0;
        UpdateWeaponData();
    }
}

[System.Serializable]
public class WeaponData
{
    public int weaponIndex;
    public bool mainWeapon;
    public bool unlocked;
    public int level;
    public int exp;
}


