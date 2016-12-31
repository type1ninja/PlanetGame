using System;
using UnityEngine;

public class Gravity : MonoBehaviour {
    
    //List of gravity objects and mass components, found as children on the object "GravityObjects"
    static Transform[] gravityTransforms;
    static GravityMass[] gravityMasses;
    //ROTATION_SPEED determines how quickly characters reorient themselves toward gravity sources
    static float ROTATION_SPEED = 50.0f;

    //The rigidbody for applying forces
    private Rigidbody rigbod;

    //Variables for use in FixedUpdate()
    private Vector3 force;
    private Vector3 strongestForce;
    private Vector3 dirToTarget;

    void Start() {
        //Assign the gravityTransforms and gravityMasses list values if they haven't been assigned yet
        if (gravityTransforms == null) {
            Transform gravityObjectsParent = GameObject.Find("GravityObjects").transform;
            gravityTransforms = new Transform[gravityObjectsParent.childCount];
            gravityMasses = new GravityMass[gravityObjectsParent.childCount];

            int children = gravityTransforms.Length;
            for (int i = 0; i < children; ++i) {
                gravityTransforms[i] = gravityObjectsParent.GetChild(i);
                gravityMasses[i] = gravityTransforms[i].GetComponent<GravityMass>();
            }
        }
        //Get the rigidbody for this character
        rigbod = GetComponent<Rigidbody>();
    }

    //Physics calculations
    void FixedUpdate() {
        //force is changed with each iteration of for and applied to the character
        force = Vector3.zero;
        //strongestForce is the record of the strongest pull force, used to determine orientation
        strongestForce = Vector3.zero;

        //Iterate through gravityTransforms (and gravityMasses) to apply the forces
        for (int i = 0; i < gravityTransforms.Length; i++) {
            //Call GetGravityForce() with the current place to get the next attractor's force
            force = GetGravityForce(gravityTransforms[i], gravityMasses[i]);
            //Check if the new force is the strongest yet. If it is, record it
            if (force.magnitude > strongestForce.magnitude)
            {
                strongestForce = force;
            }
            rigbod.AddForce(force, ForceMode.Impulse);
        }
        //Orient in the correct direction
        //Rotate the "up" direction of the player slowly away from the direction of the strongestForce
        transform.up = Vector3.Lerp(transform.up, -strongestForce.normalized, Time.fixedDeltaTime * ROTATION_SPEED);
        //Instant rotation version:
        //transform.up = -strongestForce.normalized;
    }

    private Vector3 GetGravityForce(Transform attractTransform, GravityMass attractMass)
    {
        //Get the direction to the attractor
        // Make sure the length is one, so we can scale it up easily with the force
        Vector3 dir = (attractTransform.position - transform.position).normalized;

        //Force is the direction * the strength of the pull
        //Because we're ignoring the Gravitational Constant and mass of the player, 
        //pull strength = mass / distance squared
        Vector3 force = dir * (float)((rigbod.mass * attractMass.mass) / Math.Pow(Vector3.Distance(transform.position, attractTransform.position), 2));
        return force;
    }
}