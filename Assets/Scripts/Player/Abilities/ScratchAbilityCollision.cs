using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollisionHandler : MonoBehaviour
{
    private readonly int _damage = 20;

    // Start is called before the first frame update
    void Start()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy")){
            collider.GetComponent<BossHealth>().TakeDamage(_damage);
        }   
    }
}
