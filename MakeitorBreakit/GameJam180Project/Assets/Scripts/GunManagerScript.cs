using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManagerScript : MonoBehaviour
{
    public Animator animator;

    public GameObject currentGunObject;

    public GameObject[] weapons;

    public GameManagerScript gameManager;

    public AudioSource breakSound;

    public bool gameOver = false;

    public int currentWeapon = 0;

    public void Start()
    {
        currentGunObject = weapons[0];
    }

    public void Update()
    {
        if (currentWeapon == 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("ShotgunHip"))
        {
            animator.SetBool("Switching Guns", false);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            breakSound.Play();
        }
    }

    public void AdvanceWeapon()
    {
        animator.SetBool("Switching Guns",true);

        breakSound.Play();

        Destroy(weapons[currentWeapon].GetComponent<GunScript>());
        currentWeapon++;
        if (currentWeapon <= weapons.Length-1)
        {
            currentGunObject = weapons[currentWeapon];
            currentGunObject.SetActive(true);
            currentGunObject.GetComponent<GunScript>().UpdateText();
        } else
        {
            Debug.Log("out of weapons");
            gameOver = true;
            gameManager.GameLost();
        }
    }
}
