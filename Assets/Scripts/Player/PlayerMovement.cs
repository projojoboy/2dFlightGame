using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Moving Up and Down and Rotate")] [Space(5)]
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float downwardSpeed;
    [SerializeField] private float maxDownwardSpeed;
    [SerializeField] private float rotationSpeed;
    [Header("Gravity")] [Space(5)]
    [SerializeField] private float speedToFall;
    [SerializeField] private float gravScale;
    [SerializeField] private float minusGrav;
    [Header("Gameobjects")] [Space(5)]
    [SerializeField] private GameObject wheels;

    private bool landing = false;
    private bool canGearUp = true;

    Rigidbody2D rb;
    Animator anim;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Movement();
        ChangeSpeed();
        FallDown();
        ChangeAnimation();
        GearUpDown();
    }

    void GearUpDown()
    {
        if (Input.GetKeyDown(KeyCode.E) && canGearUp)
        {
            landing = !landing;
            Debug.Log("Can land: " + landing);
            if (!landing)
            {
                wheels.SetActive(true);
                wheels.GetComponent<Animator>().Play("WheelDown");
            }
            else if (landing)
            {
                wheels.GetComponent<Animator>().Play("WheelUp");
                float i = 0;
                i += 1 * Time.deltaTime;
                if (i >= 2.5f)
                {
                    wheels.SetActive(false);
                }
            }
        }
    }

    void Movement()
    {
        var vertical = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(Time.deltaTime * speed, Time.deltaTime * downwardSpeed);

        if (vertical < 0 && downwardSpeed > -maxDownwardSpeed)
        {
            downwardSpeed--;
            RotateUpDown();
        }
        else if (vertical > 0 && downwardSpeed < maxDownwardSpeed)
        {
            downwardSpeed++;
            RotateUpDown();
        }
        else if (vertical == 0)
        {
            if (downwardSpeed > 0)
                downwardSpeed--;
            else if (downwardSpeed < 0)
                downwardSpeed++;

            RotateUpDown();
        }
    }

    void ChangeSpeed()
    {
        var horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0 && speed < maxSpeed)
            speed++;
        else if (horizontal < 0 && speed > -maxSpeed)
            speed--;
    }

    void RotateUpDown()
    {
        var vertical = Input.GetAxis("Vertical");

        if (speed > 0)
        {
            if (vertical < 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -15)), Time.deltaTime * rotationSpeed);
            }
            else if (vertical > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 15)), Time.deltaTime * rotationSpeed);
            }
            else if (vertical == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * rotationSpeed);
            }
        }
        else if (speed < 0)
        {
            if (vertical < 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 15)), Time.deltaTime * rotationSpeed);
            }
            else if (vertical > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -15)), Time.deltaTime * rotationSpeed);
            }
            else if (vertical == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * rotationSpeed);
            }
        }
    }

    void FallDown()
    {
        if(speed < speedToFall && speed > -speedToFall)
        {
            if(rb.gravityScale < gravScale)
                rb.gravityScale++;
            downwardSpeed = 0;
        }
        else
        {
            if (rb.gravityScale > 0)
                rb.gravityScale -= minusGrav * Time.deltaTime;
        }
    }

    void ChangeAnimation()
    {
        if(speed > 0)
        {
            anim.Play("MoveRight");
        }
        else if(speed < 0)
        {
            anim.Play("MoveLeft");
        }
    }

    void LandOrCrash()
    {
        if (landing && speed < speedToFall && speed > -speedToFall)
        {
            //Land

        }
        else
        {
            //Crash
            Debug.Log("Boem");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        canGearUp = false;
        LandOrCrash();
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        canGearUp = true;
    }
}
