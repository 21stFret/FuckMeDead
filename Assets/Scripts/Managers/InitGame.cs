using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public MechLoadOut mechLoadOut;

    private void OnEnable()
    {
        //Invoke("DelayedStart", 0.2f);
    }

    private void Start()
    {
        DelayedStart();
    }

    private void DelayedStart()
    {
        print("Init Main Menu");
        Time.timeScale = 1;
        PlayerSavedData.instance.LoadPlayerData();
        AudioManager.instance.Init();
        WeaponsManager.instance.LoadWeaponsData(PlayerSavedData.instance._mainWeaponData, PlayerSavedData.instance._altWeaponData);
        mechLoadOut.Init();
    }
}
