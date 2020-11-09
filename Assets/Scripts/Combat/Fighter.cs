using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGProject.Movement;
using RPGProject.Core;
using System;

namespace RPGProject.Combat
{

    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float damage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        Health target;

        float timeFromLastAttack = 0f;

        private void Update()
        {
            timeFromLastAttack += Time.deltaTime;
            if(target == null) return;

            if(target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();

            }
            

        }

        private void AttackBehaviour()
        {
            if(timeFromLastAttack >= timeBetweenAttacks){

                GetComponent<Animator>().SetTrigger("attack");
                timeFromLastAttack = 0f;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget){

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();    

        }

        public void Cancel(){
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        //Animator Event
        public void Hit(){

            if(target != null){
                target.GetComponent<Health>().TakeDamage(damage);
            }

        }
    }

}
