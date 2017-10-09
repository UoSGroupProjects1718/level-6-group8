using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class GuardPatrol : MonoBehaviour {

    public Transform[] Points;
    private int _destPoint = 0;
    public NavMeshAgent Agent { get; private set; }
    public ThirdPersonCharacter Character { get; private set; } // the character we are controlling



    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Character = GetComponent<ThirdPersonCharacter>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        // Agent.autoBraking = false;

        Agent.updateRotation = true;
        Agent.updatePosition = true;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (Points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        Agent.SetDestination(Points[_destPoint].position);
        Character.Move(Agent.remainingDistance > Agent.stoppingDistance ? Agent.desiredVelocity : Vector3.zero,
            false, false);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        _destPoint = (_destPoint + 1) % Points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!Agent.pathPending && Agent.remainingDistance < Agent.stoppingDistance)
            GotoNextPoint();
    }
}
