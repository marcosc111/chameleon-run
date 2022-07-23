using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum DomainColor
{
    None,
    Red,
    Blue
}

public class MainCharacterBasics : MonoBehaviour
{
    Vector3 initialPos;

    Rigidbody rb;
    Material mat;
    bool isRed;
    DomainColor currentDomainColor = DomainColor.None;

    Color redColor, blueColor;

    public float forwardSpeed;
    public float jumpSpeed;
    public float propelerSpeed;

    bool canJump;
    void Start()
    {
        redColor = new Color(221/255f, 30/255f, 30/255f);
        blueColor = new Color(30/255f, 70/255f, 221/255f);

        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        mat.color = redColor;

        isRed = true;
        transform.Rotate(Vector3.forward * 3);
        initialPos = rb.position;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, Mathf.Max(forwardSpeed, rb.velocity.z));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
                rb.velocity = new Vector3(0, jumpSpeed, rb.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!canJump)
            {
                swapColor();
            } else
            {
                print("Jump to swap color");
            }
        }

        // reset (debug)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Red"))
        {
            currentDomainColor = DomainColor.Red;
        }
        else if (other.CompareTag("Blue"))
        {
            currentDomainColor = DomainColor.Blue;
        }
        
        if (other.CompareTag("Jumpable"))
        {
            canJump = true;

            if (!checkColorsMatch())
            {
                gameOver();
            } else
                canJump = true;
        }

        if (other.CompareTag("GameOverZone"))
        {
            gameOver();
        }

        if (other.CompareTag("WindArea"))
        {
            rb.velocity = new Vector3(0, propelerSpeed, propelerSpeed);
        }

        if (other.CompareTag("RingRed"))
        {
            if (!isRed)
                gameOver();
        }

        if (other.CompareTag("RingBlue"))
        {
            if (isRed)
                gameOver();
        }
    }

    private void swapColor()
    {
        mat.color = isRed ? blueColor : redColor;
        isRed = !isRed;
    }

    private void gameOver()
    {
        // game over
        rb.position = initialPos;
        rb.velocity = new Vector3(0, rb.velocity.y, forwardSpeed);

        if (!isRed)
            swapColor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jumpable"))
            canJump = false;

        if (other.CompareTag("Red"))
        {
            currentDomainColor = DomainColor.None;
        }
        else if (other.CompareTag("Blue"))
        {
            currentDomainColor = DomainColor.None;
        }
    }

    /** só deve ser chamada quando entra na zona de pulo */
    private bool checkColorsMatch()
    {
        return isRed
            ? currentDomainColor == DomainColor.Red
            : currentDomainColor == DomainColor.Blue;
    }
}
