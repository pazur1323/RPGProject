using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGProject.Combat;
using RPGProject.Core;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;

    Fighter fighter;
    Health health;
    GameObject player;

    private void Start() {
        
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        player = GameObject.FindWithTag("Player");

    }

    private void Update()
    {
        if(health.IsDead()) return;
        print("Enemy health is " + health.MyHealth());
        if(AttackIfIsRange() && fighter.CanAttack(player)){

            fighter.Attack(player);

        }
        else{

            fighter.Cancel();
        }
    }

    private bool AttackIfIsRange()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < chaseDistance;
        
    }
}
