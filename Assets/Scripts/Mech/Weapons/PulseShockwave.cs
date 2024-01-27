using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PulseShockwave : MonoBehaviour
{
    public ParticleSystem pulsewave;
    public float range;
    public float forceMagnitude;
    public LayerMask crawlerLayer;
    private float timeElapsed;
    public float rechargeTime;
    public bool canUsePulseWave;
    public Image cover;
    public GameObject buttonIcon;
    public AudioClip pulseWaveSound;

    public void PlayPulseWave(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (!canUsePulseWave)
            {
                return;
            }
            PulseWave();
        }
    }

    private void PulseWave()
    {
        canUsePulseWave = false;
        ActivateButton(false);
        pulsewave.Play();
        AudioManager.instance.PlaySFXFromClip(pulseWaveSound);
        ApplyForceToCrawlers();
    }

    private void Update()
    {
        if (canUsePulseWave)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
        var percentage = 1 - (timeElapsed / rechargeTime);
        cover.fillAmount = percentage;

        if (timeElapsed >= rechargeTime)
        {
            canUsePulseWave = true;
            timeElapsed = 0;
            ActivateButton(true);
        }
    }

    private void ActivateButton(bool value)
    {
        buttonIcon.SetActive(value);
    }

    private void ApplyForceToCrawlers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, crawlerLayer);
        foreach (Collider collider in colliders)
        {
            Crawler crawler = collider.GetComponent<Crawler>();
            if (crawler != null)
            {
                crawler.StartCoroutine(crawler.SwitchOffNavMesh(0.2f));
                Rigidbody crawlerRigidbody = crawler.GetComponent<Rigidbody>();
                if (crawlerRigidbody != null)
                {
                    Vector3 forceDirection = (crawler.transform.position - transform.position).normalized;
                    crawlerRigidbody.AddForce(forceDirection * forceMagnitude , ForceMode.Impulse);
                }
            }
        }
    }
}