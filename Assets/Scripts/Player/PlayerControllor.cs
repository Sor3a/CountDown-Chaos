using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerControllor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer,doorsLayer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] RectTransform playerInMap;
    Animator animator;
    Player player;

    private Rigidbody rb;
    private bool isGrounded;
    private float verticalLookRotation;
    float horizontal, vertical;
    PhotonView pv;
    AudioSource source;

    audioTypes currentTypeWorking = audioTypes.none;

    private void OnEnable()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            Destroy(cameraTransform.gameObject);
            return;
        }

        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        if (player.playerDoingPuzzle) return;
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * moveSpeed;
        rb.MovePosition(transform.position + transform.TransformDirection(movement) * Time.fixedDeltaTime);



        // Apply camera rotation based on vertical look rotation
        cameraTransform.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
    }
    void mouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        animator.SetFloat("turning", mouseX);
        playerInMap.Rotate(-Vector3.forward * mouseX);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

    }
    void jumping()
    {
        bool beforcheckingGround = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer);
        if(beforcheckingGround == false && isGrounded && currentTypeWorking != audioTypes.Finishjumping)
        {
            currentTypeWorking = audioTypes.Finishjumping;
            AudioManager.InitializeAudio(source, currentTypeWorking);
            source.Play();
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentTypeWorking = audioTypes.Startjumping;
            AudioManager.InitializeAudio(source, currentTypeWorking);
            source.Play();
        }
    }
    void checkForDoors()
    {
        Ray r = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;
        if(Physics.Raycast(r,out hit,3f,doorsLayer))
        {
            //Debug.Log("aa");
            var hitTransform = hit.transform.root; //returnes the top of the hireachy
            //Debug.Log(hitTransform.name);
            if (hitTransform.TryGetComponent(out Door door))
            {
                door.OpenDoor();
                //Debug.Log("door");
            }
        }
    }

    void DoAnimation()
    {
        if (player.playerDoingPuzzle)
        {
            animator.SetBool("idle", true);
            animator.SetBool("running", false);
            animator.SetBool("jumping", false);
            return;
        }

        animator.SetFloat("x", horizontal);
        animator.SetFloat("y", vertical);

        if(!isGrounded)
        {
            animator.SetBool("idle", false);
            animator.SetBool("running", false);
            animator.SetBool("jumping", true);
            return;
        }

        if((vertical!=0 || horizontal!=0)&& isGrounded)
        {
            animator.SetBool("idle", false);
            animator.SetBool("running", true);
            animator.SetBool("jumping", false);
            return;
        }

        if (horizontal == 0 && vertical == 0 && isGrounded)
        {
            animator.SetBool("idle", true);
            animator.SetBool("running", false);
            animator.SetBool("jumping", false);
            return;
        }
    }
    void DoSounds()
    {

        if(animator.GetBool("running") && currentTypeWorking!= audioTypes.walking && (!source.isPlaying|| currentTypeWorking== audioTypes.idle))
        {
            currentTypeWorking = audioTypes.walking;
            AudioManager.InitializeAudio(source, currentTypeWorking);
            source.Play();
            return;
        }
        if(animator.GetBool("idle") && currentTypeWorking != audioTypes.idle && (!source.isPlaying|| currentTypeWorking == audioTypes.walking))
        {
            currentTypeWorking = audioTypes.idle;
            AudioManager.InitializeAudio(source, currentTypeWorking);
            source.Play();
            return;
        }
       
    }
    void getInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");


    }
    private void Update()
    {
        if (!pv.IsMine) return;
        DoAnimation();
        DoSounds();
        if (player.playerDoingPuzzle) return;

        getInputs();
        mouseMovement();
        jumping();
        checkForDoors();
        





    }

}
