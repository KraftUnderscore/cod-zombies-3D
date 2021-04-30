using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("DEAD");
        gameObject.SetActive(false);
    }
}
