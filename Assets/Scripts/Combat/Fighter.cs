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
            transform.LookAt(target.transform);
            if(timeFromLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeFromLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(CombatTarget combatTarget){

            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();

        }

        public void Attack(CombatTarget combatTarget){

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();    

        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //Animator Event
        public void Hit(){

            if(target == null) return;
            target.TakeDamage(damage);

        }
    }

}
