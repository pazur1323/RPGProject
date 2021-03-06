﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGProject.Core{

    public class Health : MonoBehaviour {

        [SerializeField] float health = 100f;
        bool isDead = false;

        public bool IsDead(){

            return isDead;
        }

        public float MyHealth(){
            return health;
        }
        
        public void TakeDamage(float damage){

            if(health > 0){

                health = Mathf.Max(health-damage,0);

            }
            if(health == 0)
            {
                Die();
            }
            print(health);

        }

        private void Die()
        {
            if(!isDead){

                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();

            }
        }

    }

}
