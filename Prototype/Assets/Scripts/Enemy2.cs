using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    public float shootingRange = 10f;
    public float shootCooldown = 1f;
    public GameObject bullet;
    public Transform point;
    public  Transform player;
    public LayerMask layermask;
    public NavMeshAgent agent;
    void Start()
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        // Check if the player is within a certain distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            // Raycast towards the player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, shootingRange, layermask);
            // Check if the player is hit by the raycast
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                GetComponent<Rigidbody2D>().rotation = Mathf.Atan2((player.position - transform.position).y ,
                    (player.position - transform.position).x)*Mathf.Rad2Deg;
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        Vector3 dashDirection = (player.position - transform.position).normalized;

        Debug.Log("Shoot");
        Instantiate(bullet, transform.position, transform.rotation);

        // Cooldown before the enemy can dash again
        yield return new WaitForSeconds(shootCooldown);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
