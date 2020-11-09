using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGProject.Combat;
using RPGProject.Core;
using RPGProject.Movement;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;

    Fighter fighter;
    Health health;
    GameObject player;
    Mover mover;

    Vector3 guardPosition;
    float timeSinceLastSawPlayer = Mathf.Infinity;

    private void Start() {
        
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        player = GameObject.FindWithTag("Player");

        guardPosition = transform.position;

    }

    private void Update()
    {
        if(health.IsDead()) return;
        print("Enemy health is " + health.MyHealth());
        if(AttackIfIsRange() && fighter.CanAttack(player))
        {

            timeSinceLastSawPlayer = 0f;
            AttackBehaviour();

        }
        else if(timeSinceLastSawPlayer <= suspicionTime)
        {
            SuspiciousBehaviour();

        }
        else
        {
            GuardBehaviour();
        }

        timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void GuardBehaviour()
    {
        fighter.Cancel();
        mover.StartMoveAction(guardPosition);
    }

    private void SuspiciousBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void AttackBehaviour()
    {
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
