using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public float gravityScale;
    public CharacterController controller;
    public Transform playerTransform;
    public GameObject pastLighting;
    public GameObject pastWorld;
    public GameObject futureLighting;
    public GameObject futureWorld;

    private float storedMoveSpeed;
    private Vector3 moveDirection;
    private bool isInFuture;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerTransform = this.GetComponent<Transform>();
        storedMoveSpeed = moveSpeed;
        isInFuture = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        CheckForInput();
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }

        }
       moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
       controller.Move(moveDirection * Time.deltaTime);
    }

    void CheckForInput()
    {
        //this ifstatement is what kills you
        //todo: make sure to update this later, to have a solution that isn't so rigid
        if(this.transform.position.y <= -100.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = storedMoveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(isInFuture)
            {
                controller.detectCollisions = false;
                //this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 200.0f, playerTransform.position.z);
                pastLighting.SetActive(true);
                pastWorld.SetActive(true);
                futureLighting.SetActive(false);
                futureWorld.SetActive(false);
                isInFuture = false;
                controller.detectCollisions = true;
            }
            else if(!isInFuture)
            {
                controller.detectCollisions = false;
                //this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 200.0f, playerTransform.position.z);
                pastLighting.SetActive(false);
                pastWorld.SetActive(false);
                futureLighting.SetActive(true);
                futureWorld.SetActive(true);
                isInFuture = true;
                controller.detectCollisions = true;
            }
        }
    }
}
