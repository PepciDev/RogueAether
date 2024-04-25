using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    float speed = 8f;
    float jumpForce = 150f;
    float jumpXMultiplier = 5f;
    bool grounded;
    public LayerMask groundLayer;
    public bool lookRight;
    public bool lookLeft;
    bool canMove;
    float tempYVel;
    float fallMultiplier;
    float jumpStartHeight;
    bool canBoostJump = false;
    public Animator playerAnimator;
    bool canSecondJump = true;
    bool canJump = true;

    KeyCode up = KeyCode.W;
    KeyCode down = KeyCode.S;
    KeyCode left = KeyCode.A;
    KeyCode right = KeyCode.D;
    KeyCode jump = KeyCode.Space;
    KeyCode hit = KeyCode.N;

    public Camera mainCam;
    Vector3 target;

    public GameObject arm;

    int xAxis;
    int yAxis;

    float maxYBound = -1f;

    public GameObject deathParticle;
    public bool gameOver = false;

    void Start()
    {
        //kumpaan suuntaan pelaaja katsoo alussa:
        transform.eulerAngles = new Vector3(0, -180, 0);

        playerRb = GetComponent<Rigidbody>();
    }

    /*ANIMAATIOT:
    0 = idle
    1 = run
    2 = jump
    3 = fall
    4 = shield
    5 = dodge
    6 = punch forward
    7 = punch down
    8 = punch up
    9 = stagger
    */

    private void Update()
    {
        //spacen/hyppynapin nostaminen - k‰yt‰nnˆss‰ hyppynapista pit‰‰ p‰‰st‰‰ irti jotta voi hyp‰t‰ uudelleen

        if (!canJump && Input.GetKeyUp(jump))
        {
            canJump = true;
        }

        Plane plane = new Plane(Vector3.back, Vector3.zero);
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 worldPosition = ray.GetPoint(enter);
            transform.position = worldPosition;
        }
    }

    void FixedUpdate()
    {
        //Kuolema // out of bounds

        if (transform.position.y < maxYBound)
        {
            if (!gameOver)
            {
                Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);

                gameOver = true;
                Time.timeScale = 0f;
            }
        }


        //directional inputs change the variables, works like Axis' but only for one control layout

        if (Input.GetKey(up))
        {
            yAxis = 1;
        }
        else if (Input.GetKey(down))
        {
            yAxis = -1;
        }
        else if (!Input.GetKey(up) && !Input.GetKey(down))
        {
            yAxis = 0;
        }

        if (Input.GetKey(right))
        {
            xAxis = 1;
        }
        else if (Input.GetKey(left))
        {
            xAxis = -1;
        }
        else if (!Input.GetKey(right) && !Input.GetKey(left))
        {
            xAxis = 0;
        }
        //direction contol

        if (xAxis < 0f)
        {
            lookRight = false;
            lookLeft = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (xAxis > 0f)
        {
            lookLeft = false;
            lookRight = true;
            transform.eulerAngles = new Vector3(0, -180, 0);
        }

        //raycast / ground check

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down, out hit, 1f, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.99f, transform.position.z);
            canSecondJump = true;
            grounded = true;
            speed = 8f;
            canMove = true;
            canBoostJump = false;

            if (playerRb.velocity.x == 0f)
            {
                playerAnimator.SetInteger("AnimationInt", 0);
            }
            if (playerRb.velocity.x != 0)
            {
                playerAnimator.SetInteger("AnimationInt", 1);
            }

        }
        else
        {
            //AIR

            if (canSecondJump && Input.GetKey(jump) && canJump)
            {
                float velocityNormalized = playerRb.velocity.x / playerRb.velocity.x;
                if (playerRb.velocity.x < 0)
                {
                    velocityNormalized = velocityNormalized * -1f;
                }

                if (velocityNormalized == xAxis)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x * Mathf.Abs(xAxis), 6.5f);
                }
                else
                {
                    playerRb.velocity = new Vector2(jumpXMultiplier / 2.4f * xAxis, 6.5f);
                }

                canJump = false;
                canSecondJump = false;
                transform.position += new Vector3(0, 0.1f, 0);
                grounded = false;
                playerRb.AddForce(new Vector2(0, jumpForce / 2));
                jumpStartHeight = transform.position.y;
                canBoostJump = false;
                playerAnimator.SetInteger("AnimationInt", 2);
                playerAnimator.Play("FlipJump", -1, normalizedTime: 0.0f);
            }

            if (playerRb.velocity.y > -12f)
            {
                tempYVel = playerRb.velocity.y;
            }
            if (playerRb.velocity.y < 0)
            {
                playerAnimator.SetInteger("AnimationInt", 3);
                fallMultiplier = 1.08f;
            }
            else
            {
                fallMultiplier = 1f;
            }
            if (Input.GetKey(jump) && canBoostJump && playerRb.velocity.y > 0f && transform.position.y < (jumpStartHeight + 1.2f))
            {
                playerRb.AddForce(Vector2.up * 20f);
            }
            if (!Input.GetKey(jump))
            {
                canBoostJump = false;
            }

            playerRb.velocity = new Vector2((playerRb.velocity.x + xAxis / 5.4f) * 0.98f, tempYVel * fallMultiplier);
            speed = 0f;
            grounded = false;
            canMove = false;
        }

        if (canMove)
        {
            playerRb.velocity = new Vector2(xAxis * speed, 0f);
        }

        if (grounded && Input.GetKey(jump) && canJump)
        {
            canJump = false;
            playerRb.velocity = new Vector2(jumpXMultiplier * xAxis, 3f);
            transform.position += new Vector3(0, 0.1f, 0);
            grounded = false;
            playerRb.AddForce(new Vector2(0, jumpForce));
            jumpStartHeight = transform.position.y;
            canBoostJump = true;
            playerAnimator.SetInteger("AnimationInt", 2);
        }
    }
}