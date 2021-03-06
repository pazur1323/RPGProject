﻿using System.Collections;
using System.Collections.Generic;
using RPGProject.Combat;
using RPGProject.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

namespace RPGProject.Movement
{

    public class Mover : MonoBehaviour, IAction
    {

        NavMeshAgent navMeshAgent;
        Health health;
        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination){

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {

            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            anim.SetFloat("forwardSpeed", localVelocity.z);

        }
    }

}