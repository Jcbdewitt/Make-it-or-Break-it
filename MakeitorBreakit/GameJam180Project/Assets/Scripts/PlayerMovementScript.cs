using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public GunManagerScript gunManager;

    private float speed = 12f;
    public float runSpeed = 19f;
    public float walkSpeed = 12f;
    public float hitTimer = 3f;

    public Vector3 move;

    public bool running = false;
    public bool onHitTimer = false;
    public bool gameLost = false;

    void Update()
    {
        if (!gameLost)
        {
            move = Vector3.zero;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!gunManager.gameOver)
            {
                if (!gunManager.currentGunObject.GetComponent<GunScript>().reloading)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        speed = runSpeed;
                        running = true;
                    }
                    else
                    {
                        speed = walkSpeed;
                        running = false;
                    }
                }
                else
                {

                    speed = walkSpeed;

                }
            }

            move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);
        }
    }

    public void Hit()
    {
        if (!onHitTimer)
        {
            onHitTimer = true;
            StartCoroutine(HitWaitTimer());
            if (!gunManager.gameOver)
            {
                gunManager.currentGunObject.GetComponent<GunScript>().BreakGun();
            }
        }
    }

    IEnumerator HitWaitTimer()
    {
        yield return new WaitForSeconds(hitTimer);
        onHitTimer = false;
    }
}
