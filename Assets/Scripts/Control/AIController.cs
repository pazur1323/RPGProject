using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGProject.Combat;
using RPGProject.Core;
using RPGProject.Movement;
using RPGProject.Control;
using System;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] float dwellTime = 3f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;

    Fighter fighter;
    Health health;
    GameObject player;
    Mover mover;

    Vector3 guardPosition;
    int currentWaypointIndex = 0;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceReachWaypoint = Mathf.Infinity;

    private void Start() {
        
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        player = GameObject.FindWithTag("Player");

        guardPosition = transform.position;

    }

    private void Update()
    {
        if (health.IsDead()) return;
        if (AttackIfIsRange() && fighter.CanAttack(player))
        {

            AttackBehaviour();

        }
        else if (timeSinceLastSawPlayer <= suspicionTime)
        {
            SuspicionBehaviour();

        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimes();
    }

    private void UpdateTimes()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceReachWaypoint += Time.deltaTime;
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition;
        if(patrolPath != null){
            if(AtWaypoint()){

                timeSinceReachWaypoint = 0f;
                CycleWaypoint();
                
            }
            nextPosition = GetCurrentWaypoint();
        }
        if(timeSinceReachWaypoint > dwellTime){
                    
            mover.StartMoveAction(nextPosition);
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void CycleWaypoint()
    {
        
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehaviour()
    {
        timeSinceLastSawPlayer = 0f;
        fighter.Attack(player);
    }

    private bool AttackIfIsRange()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < chaseDistance;
        
    }

    //Called By Unity
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

    }
}
