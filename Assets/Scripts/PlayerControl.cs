using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawn;
    [SerializeField] private KeyCode UP = KeyCode.Keypad5;
    [SerializeField] private KeyCode DOWN = KeyCode.Keypad2;
    [SerializeField] private KeyCode LEFT = KeyCode.Keypad1;
    [SerializeField] private KeyCode RIGHT = KeyCode.Keypad3;
    [SerializeField] private KeyCode CameraUP = KeyCode.Keypad7;
    [SerializeField] private KeyCode CameraDOWN = KeyCode.Keypad9;
    [SerializeField] private KeyCode CameraLEFT = KeyCode.Keypad4;
    [SerializeField] private KeyCode CameraRIGHT = KeyCode.Keypad6;
    [SerializeField] private KeyCode CameraZoomIn = KeyCode.Keypad8;
    [SerializeField] private KeyCode CameraZoomOut = KeyCode.Keypad0;
    [SerializeField] private KeyCode CameraReset = KeyCode.Z;
    [SerializeField] private KeyCode Attack1 = KeyCode.F;
    [SerializeField] private KeyCode Attack2 = KeyCode.G;
    [SerializeField] private KeyCode Jump = KeyCode.D;
    [SerializeField] private KeyCode Run = KeyCode.S;
    [SerializeField] private KeyCode Restart = KeyCode.Escape;
    [SerializeField] [Range(1.0f, 10.0f)] private float Agility = 5.0f;

    private Camera mainCamera;
    private Animator playerAnime;
    private Animator weaponAnime;
    private ArrayList actionQueue;
    //private RigidbodyConstraints originalConstraints;
    private bool freezeMove;
    private bool freezeRotate;
    private bool secondJump;
    private bool landed;
    private GameObject arrowSpawned;
    
    void Start()
    {
        mainCamera = Camera.main;
        playerAnime = GetComponent<Animator>();
        weaponAnime = weapon.GetComponent<Animator>();
        actionQueue = new ArrayList();
        //originalConstraints = player.GetComponent<Rigidbody>().constraints;
        freezeMove = false;
        freezeRotate = false;
        secondJump = false;
        landed = true;
        arrowSpawned = null;
        playerAnime.PlayInFixedTime("103_idle");
    }
    
    void FixedUpdate()
    {
        if (Input.GetKey(UP) || Input.GetKey(DOWN) || Input.GetKey(LEFT) || Input.GetKey(RIGHT))
        {
            string action = (Input.GetKey(Run)) ? "103_run" : "10003_walk";
            performAction(action);
        }
        else
        {
            performAction("103_idle");
        }

        if (Input.GetKeyDown(CameraReset))
        {
            resetCamera();
        }

        if (Input.GetKey(CameraUP) || Input.GetKey(CameraDOWN) || Input.GetKey(CameraLEFT) || Input.GetKey(CameraRIGHT) || Input.GetKey(CameraZoomIn) || Input.GetKey(CameraZoomOut))
        {
            adjustCamera();
        }

        if (Input.GetKeyDown(Attack1))
        {
            string currentAnime = playerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (actionQueue.Count == 0 || currentAnime.Equals("103_normalatk4") || currentAnime.Equals("10003_attack"))
            {
                if (actionQueue.Count == 0)
                    actionQueue.Add("103_normalatk4");
                else if (currentAnime.Equals("103_normalatk4") && actionQueue.Count == 1)
                    actionQueue.Add("10003_attack");
                else if (currentAnime.Equals("10003_attack") && actionQueue.Count == 1)
                    actionQueue.Add("103_fastshot");
            }
        }
        else if (Input.GetKeyDown(Attack2))
        {
            if (actionQueue.Count == 0)
                actionQueue.Add("103_common");
        }
        else if (Input.GetKeyDown(Jump))
        {
            if (actionQueue.Count == 0)
            {
                actionQueue.Add("103_jump3");
                actionQueue.Add("103_glide3");
                actionQueue.Add("103_landing2");
                secondJump = true;
            }
            else if (playerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("103_jump3") || playerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("103_glide3"))
            {
                if (secondJump)
                {
                    secondJump = false;
                    player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
                }
            }
        }

        if (Input.GetKeyDown(Restart))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Terrain"))
            landed = true;
    }

    private void performAction(string animeName)
    {
        if (actionQueue.Count != 0) // Jump, Attack
        {
            string currentAnime = playerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (!currentAnime.Equals(actionQueue[0]))
            {
                playerAnime.PlayInFixedTime(actionQueue[0].ToString());
                switch (actionQueue[0].ToString())
                {
                    case "103_jump3": // Jump
                        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
                        landed = false;
                        break;
                    case "103_landing2": // Land
                        freezeMove = true;
                        freezeRotate = true;
                        break;
                    case "103_normalatk4":
                        weaponAnime.PlayInFixedTime("10003_attack_2b");
                        freezeMove = true;
                        break;
                    case "10003_attack":
                        weaponAnime.PlayInFixedTime("10003_attack_b");
                        freezeMove = true;
                        break;
                    case "103_fastshot":
                        weaponAnime.PlayInFixedTime("10003_fastshot_b");
                        freezeMove = true;
                        break;
                    case "103_common":
                        freezeMove = true;
                        freezeRotate = true;
                        break;
                }
            }
            else if (currentAnime.Equals("103_jump3") || currentAnime.Equals("103_glide3"))
            {
                if (currentAnime.Equals("103_jump3") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    actionQueue.RemoveAt(0);

                if (landed)
                {
                    while (!actionQueue[0].ToString().Equals("103_landing2"))
                        actionQueue.RemoveAt(0);
                }
            }
            else if (currentAnime.Equals("103_normalatk4") || currentAnime.Equals("10003_attack") || currentAnime.Equals("103_fastshot"))
            {
                //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                if (currentAnime.Equals("103_normalatk4") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f ||
                    currentAnime.Equals("10003_attack") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f ||
                    currentAnime.Equals("103_fastshot") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
                {
                    if (arrowSpawned == null)
                    {
                        Quaternion angle = Quaternion.Euler(0, player.transform.eulerAngles.y + transform.localEulerAngles.y, 0);
                        arrowSpawned = Instantiate(arrow, arrowSpawn.position, angle);
                        arrowSpawned.transform.localPosition += arrowSpawned.transform.forward * 1.2f;
                        if (currentAnime.Equals("103_normalatk4"))
                            arrowSpawned.transform.localPosition += arrowSpawned.transform.up * 0.1f;
                        else
                            arrowSpawned.transform.localPosition += arrowSpawned.transform.up * 0.15f;
                        freezeRotate = true;
                    }
                    else
                    {
                        if (currentAnime.Equals("103_normalatk4") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.50f ||
                            currentAnime.Equals("10003_attack") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.60f ||
                            currentAnime.Equals("103_fastshot") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f)
                            arrowSpawned.GetComponent<Rigidbody>().velocity = arrowSpawned.transform.forward * 50.0f;
                    }
                }
                if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    actionQueue.RemoveAt(0);
                    arrowSpawned = null;
                    freezeMove = false;
                    freezeRotate = false;
                    //player.GetComponent<Rigidbody>().constraints = originalConstraints;
                }
            }
            else if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                actionQueue.RemoveAt(0);
                freezeMove = false;
                freezeRotate = false;
            }
        }
        else
        {
            playerAnime.PlayInFixedTime(animeName);
            weaponAnime.PlayInFixedTime("10003_idle");
        }

        if (!freezeMove)
        {
            if (!animeName.Equals("103_idle"))
            {
                float moveSpeed = (Input.GetKey(Run)) ? (float)(Agility * 1.5f) / 20.0f : (float)(Agility) / 20.0f;
                rotateCharacter();
                moveCharacter(moveSpeed);
            }
        }
    }

    private void moveCharacter(float moveSpeed)
    {
        float radDeg = (player.transform.eulerAngles.y + transform.localEulerAngles.y) * Mathf.Deg2Rad;
        player.transform.position += new Vector3(Mathf.Sin(radDeg) * moveSpeed, 0, Mathf.Cos(radDeg) * moveSpeed);
    }

    private void rotateCharacter()
    {
        float playerAngle = 0.0f;
        if (Input.GetKey(UP) && Input.GetKey(LEFT)) playerAngle = -45.0f;
        else if (Input.GetKey(UP) && Input.GetKey(RIGHT)) playerAngle = 45.0f;
        else if (Input.GetKey(DOWN) && Input.GetKey(LEFT)) playerAngle = -135.0f;
        else if (Input.GetKey(DOWN) && Input.GetKey(RIGHT)) playerAngle = 135.0f;
        else if (Input.GetKey(UP)) playerAngle = 0.0f;
        else if (Input.GetKey(DOWN)) playerAngle = 180.0f;
        else if (Input.GetKey(LEFT)) playerAngle = -90.0f;
        else if (Input.GetKey(RIGHT)) playerAngle = 90.0f;
        transform.localEulerAngles = new Vector3(0, playerAngle, 0);
    }

    private void adjustCamera()
    {
        if (Input.GetKey(CameraLEFT) || Input.GetKey(CameraRIGHT))
        {
            if (!freezeRotate)
            {
                resetCamera();
                float rotDir = Input.GetKey(CameraLEFT) ? -2.5f : 2.5f;
                player.transform.eulerAngles += new Vector3(0, rotDir, 0);
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        if (Input.GetKey(CameraUP) || Input.GetKey(CameraDOWN))
        {
            float cameraPosition = mainCamera.gameObject.transform.localPosition.y;
            if (Input.GetKey(CameraUP) && cameraPosition > 1.0f)
            {
                mainCamera.gameObject.transform.localPosition -= new Vector3(0, 0.25f, 0);
                mainCamera.gameObject.transform.localEulerAngles -= new Vector3(3.0f, 0, 0);
            }
            else if (Input.GetKey(CameraDOWN) && cameraPosition < 6.0f)
            {
                mainCamera.gameObject.transform.localPosition += new Vector3(0, 0.25f, 0);
                mainCamera.gameObject.transform.localEulerAngles += new Vector3(3.0f, 0, 0);
            }
        }

        if (Input.GetKey(CameraZoomIn) || Input.GetKey(CameraZoomOut))
        {
            float cameraZoom = mainCamera.gameObject.transform.localPosition.z;
            if (Input.GetKey(CameraZoomIn) && cameraZoom < -2.5f)
                mainCamera.gameObject.transform.localPosition += new Vector3(0, 0, 0.1f);
            else if (Input.GetKey(CameraZoomOut) && cameraZoom > -7.5f)
                mainCamera.gameObject.transform.localPosition -= new Vector3(0, 0, 0.1f);
        }
    }

    private void resetCamera()
    {
        player.transform.eulerAngles += transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
