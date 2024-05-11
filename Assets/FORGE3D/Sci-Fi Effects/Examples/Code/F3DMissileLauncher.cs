﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FORGE3D
{
    public class F3DMissileLauncher : MonoBehaviour
    {
        public Transform missilePrefab;
        public Transform target;
        public Transform[] socket;
        public Transform explosionPrefab;

        private F3DMissile.MissileType missileType;

        public Text missileTypeLabel;

        // Use this for initialization
        private void Start()
        {
            missileType = F3DMissile.MissileType.Guided;
            //missileTypeLabel.text = "Missile crateType: Unguided";
        }

        // Spawns explosion
        public void SpawnExplosion(Vector3 position)
        {
            F3DPoolManager.Pools["GeneratedPool"]
                .Spawn(explosionPrefab, position, Quaternion.identity, null);
            F3DAudioController.instance.RailGunHit(position);
        }

        public void LaunchMissiles(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                LaunchMissile();
            }
        }


        public void LaunchMissile()
        {
            var randomSocketId = Random.Range(0, socket.Length);
            var tMissile = F3DPoolManager.Pools["GeneratedPool"].Spawn(missilePrefab,
                socket[randomSocketId].position, socket[randomSocketId].rotation, null);

            if (tMissile != null)
            {
                var missile = tMissile.GetComponent<F3DMissile>();

                missile.launcher = this;
                missile.missileType = missileType;

                if (target != null)
                    missile.target = target;
            }
        }

        // Processes input for launching missile
        private void ProcessInput()
        {
            /*
            if (Input.GetMouseButtonDown(0))
            {
                var randomSocketId = Random.Range(0, socket.Length);
                var tMissile = F3DPoolManager.Pools["GeneratedPool"].Spawn(missilePrefab,
                    socket[randomSocketId].position, socket[randomSocketId].rotation, null);

                if (tMissile != null)
                {
                    var missile = tMissile.GetComponent<F3DMissile>();

                    missile.launcher = this;
                    missile.missileType = missileType;

                    if (target != null)
                        missile.target = target;
                }
            }
            */

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                missileType = F3DMissile.MissileType.Unguided;
                missileTypeLabel.text = "Missile crateType: Unguided";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                missileType = F3DMissile.MissileType.Guided;
                missileTypeLabel.text = "Missile crateType: Guided";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                missileType = F3DMissile.MissileType.Predictive;
                missileTypeLabel.text = "Missile crateType: Predictive";
            }
        }

        // Update is called once per frame
        private void Update()
        {
            ProcessInput();
        }
    }
}