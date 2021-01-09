using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GunScript : MonoBehaviour
{
    public Camera fpsCam;

    public Animator animator;

    public GunManagerScript gunManager;

    public GameObject crosshair;
    public GameObject impactEffect;
    public GameObject[] breakableParts;

    public ParticleSystem[] connectedParticles;

    public ParticleSystem particles;

    public PlayerMovementScript playerScript;

    public TextMeshProUGUI ammoText;

    public AudioSource audioSource;

    public float reloadTime = 5f;
    public float fireRate = 5f;
    public float impactForce = 30f;
    public float damage = 10f;
    public float range = 100f;
    public float throwForce = 100f;
    public float kickForce = 5f;
    public float particleMoveDistance = 0f;
    private float nextTimeToFire = 0f;
    private float maxParticles;

    public int maxAmmo = 30;
    public int ammo;
    private int currentParticle = 0;

    public string nameOfReloading = "";
    public string nameOfHip = "";

    public bool reloading = false;
    public bool partsBroken = false;
    public bool moveParticlesOnBreak = false;
    private bool reloadAnimationPlaying = false;

    private void Start()
    {
        maxParticles = connectedParticles.Length;
        ammo = maxAmmo;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AcceptInput();

        CheckAnimationFrame();

        //Debug button
        if (Input.GetKeyDown(KeyCode.P))
        {
            BreakGun();
        }
    }

    private void CheckAnimationFrame()
    {
        if (reloading && animator.GetCurrentAnimatorStateInfo(0).IsName(nameOfReloading))
        {
            animator.SetBool("Reloading", false);
            reloadAnimationPlaying = true;
        }

        if (reloading && reloadAnimationPlaying && animator.GetCurrentAnimatorStateInfo(0).IsName(nameOfHip))
        {
            reloadAnimationPlaying = false;
            reloading = false;
            ammo = maxAmmo;
            ammoText.text = ammo.ToString();
        }
    }

    private void AcceptInput()
    {
        if (!reloading)
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && !playerScript.running && ammo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }

            if (Input.GetMouseButton(1))
            {
                animator.SetBool("Aiming", true);
                crosshair.SetActive(false);
            }
            else
            {
                animator.SetBool("Aiming", false);
                crosshair.SetActive(true);
            }

            if (playerScript.running)
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
        }

        if (!reloading && Input.GetKeyDown(KeyCode.R) && ammo != maxAmmo)
        {
            reloading = true;
            currentParticle = 0;
            animator.SetBool("Running", false);
            animator.SetBool("Aiming", false);
            animator.SetBool("Reloading", true);
        }
    }

    public void PlayParticles()
    {
        if (maxParticles > 1)
        {
            connectedParticles[currentParticle].Play();
            currentParticle++;
            if (currentParticle >= maxParticles)
            {
                currentParticle = 0;
            }
        } else
        {
            connectedParticles[0].Play();
        }
    }

    public void Shoot()
    {
        PlayParticles();

        ApplyKick();

        ammo -= 1;

        UpdateText();

        audioSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            TargetScript target = hit.transform.GetComponent<TargetScript>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    public void ApplyKick()
    {
        fpsCam.gameObject.GetComponent<MouseLookScript>().xRotation -= kickForce;
    }

    public void UpdateText()
    {
        ammoText.text = ammo.ToString();
    }

    public void BreakGun()
    {
        if (!partsBroken)
        {
            gunManager.breakSound.Play();

            partsBroken = true;

            for (int i = 0; i < breakableParts.Length; i++)
            {
                GameObject breakingObject = breakableParts[i];

                breakingObject.transform.parent = null;
                breakingObject.GetComponent<BoxCollider>().enabled = true;
                breakingObject.GetComponent<Rigidbody>().isKinematic = false;
                breakingObject.GetComponent<Rigidbody>().useGravity = true;
                breakingObject.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce);
            }

            if (moveParticlesOnBreak)
            {
                foreach (ParticleSystem p in connectedParticles)
                {
                    p.gameObject.transform.localPosition = new Vector3(p.gameObject.transform.localPosition.x + particleMoveDistance, p.gameObject.transform.localPosition.y, p.gameObject.transform.localPosition.z);
                }
            }
        } else
        {
            gunManager.AdvanceWeapon();

            transform.parent = null;

            GetComponent<BoxCollider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;

            GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce);

        }
    }
}
