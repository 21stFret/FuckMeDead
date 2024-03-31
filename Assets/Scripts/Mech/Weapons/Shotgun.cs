using Micosmo.SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MechWeapon
{
    public bool hasTarget;
    public GameObject gunturret;
    private Animator _animator;
    public ProjectileWeapon weaponController;

    private float _timer;

    public int shotsPerBurst;
    public float spreadAngle;
    public float stunTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        var target = sensor.GetNearestDetection("Enemy");

        if (target != null)
        {
            hasTarget = true;
            //gunturret.transform.LookAt(target.transform);
            gunturret.transform.forward = Vector3.Lerp(gunturret.transform.forward, target.transform.position - gunturret.transform.position + aimOffest, Time.deltaTime * 2f);
        }
        else
        {
            hasTarget = false;
            _timer = 0.0f;
        }

        if (hasTarget)
        {
            _timer += Time.deltaTime;
            if (_timer > fireRate)
            {
                for (int i = 0; i < shotsPerBurst; i++)
                {
                    int newI = i - (shotsPerBurst / 2);
                    weaponController.Shotgun(damage, force, newI, spreadAngle, shotsPerBurst, i, stunTime);
                }
                _timer = 0.0f;
                _animator.SetTrigger("Recoil");
            }
        }

    }
}

