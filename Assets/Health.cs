using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGProject.Combat{

    public class Health : MonoBehaviour {

        [SerializeField] float health = 100f;
        
        public void TakeDamage(float damage){

            if(health > 0){

                health -=damage;

            }
            print(health);

        }

    }

}
