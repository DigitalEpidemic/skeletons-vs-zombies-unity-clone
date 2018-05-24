using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour {

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDistance = 10f;
    [SerializeField] private GameObject playerCircle;
    [SerializeField] private Slider healthBar;

    private float nextFire;
    private float shootRate = 2.567f;
    private bool isAttacking = false;

    private Transform targetedEnemy;
    bool enemyClicked;

    bool running;
    private Animator anim;
    private NavMeshAgent navAgent;

    private Camera mainCam;

    [SyncVar] private int health = 100;
    private int bulletDamage = 35;

    private NetworkAnimator networkAnim;

    void Start () {
        Assert.IsNotNull (bulletPrefab);
        Assert.IsNotNull (bulletSpawnPoint);
        Assert.IsNotNull (playerCircle);
        Assert.IsNotNull (healthBar);

        anim = GetComponent<Animator> ();
        navAgent = GetComponent<NavMeshAgent> ();
        networkAnim = GetComponent<NetworkAnimator> ();

        // Deactivates all Cameras
        mainCam = this.transform.Find ("Player Camera").Find ("Camera Thing").GetComponent<Camera> ();
        mainCam.gameObject.SetActive (false);
    }

    public override void OnStartLocalPlayer () {
        playerCircle.SetActive (true);
        tag = "Player";

    }

    void Update () {
        healthBar.value = health;

        // Activate local Camera
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

                anim.SetTrigger ("Attack");
                networkAnim.SetTrigger ("Attack");
                StartCoroutine (WaitForAttack ());

            }
            navAgent.isStopped = true;
            running = false;
        }
    }

    [Command]
    void CmdFire () {
        
        GameObject fireball = Instantiate (bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
        fireball.GetComponent<Rigidbody> ().velocity = fireball.transform.forward * 4f;

        NetworkServer.Spawn (fireball);

        Destroy (fireball, 3.5f);
    }

    IEnumerator WaitForAttack () {
        yield return new WaitForSeconds (1.567f);
        CmdFire ();
    }

    void OnCollisionEnter (Collision target) {
        // CompareTag has better performance vs .tag
        if (target.gameObject.CompareTag ("Bullet")) {

            if (isLocalPlayer) {
                //Debug.Log ("Hit detected");
            }

            TakeDamage ();
        }
    }

    void TakeDamage () {
        if (!isServer) {
            return;
        }

        health -= bulletDamage;

        if (health <= 0) {
            health = 0;
            Debug.Log ("Dead");
        }
    }

} // PlayerScript