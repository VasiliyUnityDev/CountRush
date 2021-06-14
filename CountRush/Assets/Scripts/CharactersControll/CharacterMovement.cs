using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public bool canMove = false;
    public float posZ;
    [Range(0,1)]
    public float sensitivityMultiplier;
    private float deltaThreshold;
    Vector2 firstTouchPosition;
    float finalTouchX, finalTouchZ;
    Vector2 curTouchPosition;
    public float minXPos;
    public float maxXPos;

    public float what;

    public static float speedMove;

    Rigidbody rbPlayer;
    Animator anim;
 
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - posZ);

        speedMove = 3f;

        ResetInputValues();
    } 

    void Update()
    {
        HandleMovementWithSlide();
    } 

    private void FixedUpdate()
    {
        what = speedMove;
        if (canMove)
        {
            HandleEndlessRun();
            anim.SetBool("Running", true);
            UIManager.instance.MenuClose();
        }
    } 

    void ResetInputValues()
    {
        rbPlayer.velocity = new Vector3(0f, rbPlayer.velocity.y, rbPlayer.velocity.z);
        firstTouchPosition = Vector2.zero;
        finalTouchX = 0f;
        curTouchPosition = Vector2.zero;
    } 

    void HandleEndlessRun()
    {
        rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, rbPlayer.velocity.y, speedMove);
    } 

    void HandleMovementWithSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            curTouchPosition = Input.mousePosition;
            Vector2 touchDelta = (curTouchPosition - firstTouchPosition);

            if (firstTouchPosition == curTouchPosition)
            {
                rbPlayer.velocity = new Vector3(0f, rbPlayer.velocity.y, rbPlayer.velocity.z);
            }

            finalTouchX = transform.position.x;

            if (Mathf.Abs(touchDelta.x) >= deltaThreshold)
            {
                finalTouchX = (transform.position.x + (touchDelta.x * sensitivityMultiplier));
            }

            rbPlayer.position = new Vector3(finalTouchX, transform.position.y, transform.position.z);
            rbPlayer.position = new Vector3(Mathf.Clamp(rbPlayer.position.x, minXPos, maxXPos), rbPlayer.position.y, rbPlayer.position.z);

            firstTouchPosition = Input.mousePosition;

            canMove = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetInputValues();
            canMove = false;
            anim.SetBool("Running", false);
        }
    } 
}
