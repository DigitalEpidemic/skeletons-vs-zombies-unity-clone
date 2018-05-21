﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDistance = 10f;
    private float nextFire;
    private float shootRate = 2f;
    private bool isAttacking = false;

    private Transform targetedEnemy;
    bool enemyClicked;

    bool running;
    private Animator anim;
    private NavMeshAgent navAgent;

    private Camera mainCam;


    void Start () {
        Assert.IsNotNull (bulletPrefab);
        Assert.IsNotNull (bulletSpawnPoint);
        anim = GetComponent<Animator> ();
        navAgent = GetComponent<NavMeshAgent> ();
        mainCam = GameObject.Find ("Camera Thing").GetComponent<Camera> ();
    }

    void Update () {

        if (isLocalPlayer) {
            if (!mainCam.gameObject.activeInHierarchy) {
                mainCam.gameObject.SetActive (true);
            }
        }

        if (!isLocalPlayer) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown ("Fire2")) {
            // out sends the value back into the original variable
            if (Physics.Raycast (ray, out hit, 100)) {
                if (hit.collider.CompareTag ("Enemy")) {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;

                } else {
                    isAttacking = false;
                    running = true;
                    enemyClicked = false;
                    // Walk to point
                    navAgent.destination = hit.point;
                    navAgent.isStopped = false;
                }
            }
        }

        if (enemyClicked) {
            MoveAndShoot ();
        }

        if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
            running = false;
        } else {
            if (!isAttacking) {
                running = true;
            }
        }

        anim.SetBool ("isRunning", running);
    }

    void MoveAndShoot () {
        if (targetedEnemy == null) {
            return;
        }

        // Move towards enemy
        navAgent.destination = targetedEnemy.position;
        if (navAgent.remainingDistance >= shootDistance) {
            navAgent.isStopped = false;
            running = true;
        }

        if (navAgent.remainingDistance <= shootDistance) {
            // Look at enemy
            transform.LookAt (targetedEnemy);

            // Sets time between attacks
            if (Time.time > nextFire) {
                isAttacking = true;
                nextFire = Time.time + shootRate;
                Fire ();
            }
            navAgent.isStopped = true;
            running = false;
        }
    }

    void Fire () {
        anim.SetTrigger ("Attack");
        GameObject fireball = Instantiate (bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
        fireball.GetComponent<Rigidbody> ().velocity = fireball.transform.forward * 4f;
        Destroy (fireball, 3.5f);
    }

} // PlayerScript