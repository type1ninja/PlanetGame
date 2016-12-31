﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

//Taken from here: http://answers.unity3d.com/questions/8676/make-rigidbody-walk-like-character-controller.html
//I have probably made some modifications
public class RigidbodyFPS : MonoBehaviour
{
    #region Variables (private)

    private bool grounded = false;
    private Vector3 groundVelocity;
    private CapsuleCollider capsule;
    private Rigidbody rigbod;

    // Inputs Cache
    private bool jumpFlag = false;

    #endregion

    #region Properties (public)

    // Speeds
    public float walkSpeed = 8.0f;
    public float walkBackwardSpeed = 4.0f;
    public float runSpeed = 14.0f;
    public float runBackwardSpeed = 6.0f;
    public float sidestepSpeed = 8.0f;
    public float runSidestepSpeed = 12.0f;
    public float maxVelocityChange = 10.0f;

    // Air
    public float inAirControl = 0.0f;
    public Vector3 jumpForce = new Vector3(0, 10.0f, 0);

    // Can Flags
    public bool canRunSidestep = true;
    public bool canJump = true;
    public bool canRun = true;

    #endregion

    #region Unity event functions

    /// <summary>
    /// Use for initialization
    /// </summary>
    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        rigbod = GetComponent<Rigidbody>();
        rigbod.freezeRotation = true;
        rigbod.useGravity = true;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Cache the input
        if (Input.GetButtonDown("Jump"))
            jumpFlag = true;
    }

    /// <summary>
    /// Update for physics
    /// </summary>
    void FixedUpdate()
    {

        // Cache the input
        var inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // On the ground
        if (grounded)
        {
            // Apply a force that attempts to reach our target velocity
            var velocityChange = CalculateVelocityChange(inputVector);
            velocityChange = transform.TransformDirection(velocityChange);
            //Debug.DrawLine(transform.position, transform.position + velocityChange, Color.red, 10);
            rigbod.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && jumpFlag)
            {
                jumpFlag = false;
                //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(jumpForce), Color.red, 10);
                rigbod.AddForce(transform.TransformDirection(jumpForce), ForceMode.Impulse);
            }

            // By setting the grounded to false in every FixedUpdate we avoid
            // checking if the character is not grounded on OnCollisionExit()
            grounded = false;
        }
        // In mid-air
        else
        {
            // Uses the input vector to affect the mid air direction
            var velocityChange = transform.TransformDirection(inputVector) * inAirControl;
            rigbod.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    // Unparent if we are no longer standing on our parent
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == transform.parent)
            transform.parent = null;
    }

    // If there are collisions check if the character is grounded
    void OnCollisionStay(Collision col)
    {
        TrackGrounded(col);
    }

    void OnCollisionEnter(Collision col)
    {
        TrackGrounded(col);
    }

    #endregion

    #region Methods

    // From the user input calculate using the set up speeds the velocity change
    private Vector3 CalculateVelocityChange(Vector3 inputVector)
    {
        // Calculate how fast we should be moving
        var relativeVelocity = transform.TransformDirection(inputVector);
        if (inputVector.z > 0)
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Sprint")) ? runSpeed : walkSpeed;
        }
        else
        {
            relativeVelocity.z *= (canRun && Input.GetButton("Sprint")) ? runBackwardSpeed : walkBackwardSpeed;
        }
        relativeVelocity.x *= (canRunSidestep && Input.GetButton("Sprint")) ? runSidestepSpeed : sidestepSpeed;

        // Calcualte the delta velocity
        var currRelativeVelocity = rigbod.velocity - groundVelocity;
        var velocityChange = relativeVelocity - currRelativeVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    // Check if the base of the capsule is colliding to track if it's grounded
    private void TrackGrounded(Collision collision)
    {
        var maxHeight = capsule.bounds.min.y + capsule.radius * .9f;
        foreach (var contact in collision.contacts)
        {

            if (contact.point.y < maxHeight)
            {
                if (isKinematic(collision))
                {
                    // Get the ground velocity and we parent to it
                    groundVelocity = collision.rigidbody.velocity;
                    transform.parent = collision.transform;
                }
                else if (isStatic(collision))
                {
                    // Just parent to it since it's static
                    transform.parent = collision.transform;
                }
                else
                {
                    // We are standing over a dynamic object,
                    // set the groundVelocity to Zero to avoid jiggers and extreme accelerations
                    groundVelocity = Vector3.zero;
                }

                // Esta en el suelo
                grounded = true;
            }

            break;
        }
    }

    private bool isKinematic(Collision collision)
    {
        return isKinematic(GetComponent<Collider>().transform);
    }

    private bool isKinematic(Transform transform)
    {
        Rigidbody otherRigBod = transform.GetComponent<Rigidbody>();
        return otherRigBod && otherRigBod.isKinematic;
    }

    private bool isStatic(Collision collision)
    {
        return isStatic(collision.transform);
    }

    private bool isStatic(Transform transform)
    {
        return transform.gameObject.isStatic;
    }

    #endregion
}