using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using NavMeshPlus.Extensions;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public float seeDistance;
    public LayerMask flashlightMask;
    public GameObject player;
    public Rigidbody2D rb;
    public float speed;
    public bool inside = false;
    public NavMeshAgent agent;

    public bool knowsPosition = false;
    public Vector2 lastKnownLocation;

    public float stunTImer;
    float stunTimerVal;

    public float attackTimer;
    float attackTimerVal;
    public GameObject flashlight;

    public float stopingDistance;
    public float damage = 10f;
    public Follow cam;

    public float playerSeeDistanceNoLight;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false; 
        agent.speed = speed;
        lastKnownLocation = transform.position;
        agent.stoppingDistance = stopingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = speed;

        if(Vector2.Distance(player.transform.position , transform.position) <= playerSeeDistanceNoLight)
        {
            knowsPosition=true;
        }
        else if (flashlight.activeInHierarchy)
        {
            RaycastHit2D rayTop = Physics2D.Raycast(transform.position, (top.position - transform.position), seeDistance , flashlightMask);
            RaycastHit2D rayBottom = Physics2D.Raycast(transform.position, (bottom.position - transform.position), seeDistance, flashlightMask) ;
            //Debug.DrawRay(transform.position, (top.position - transform.position), Color.red);
            if (rayTop.collider != null || rayBottom.collider != null)
            {
                Debug.Log(rayTop.collider.tag);
                if (rayTop.collider.CompareTag("GameController") || rayBottom.collider.CompareTag("GameController"))
                    knowsPosition = true;
                else
                    knowsPosition = false;
            }
            else
                knowsPosition = false;
        }
        else
            knowsPosition = false;

        if (inside)
        {
            if(stunTimerVal <= 0f)
            {
                agent.enabled = true;
                stunTimerVal = stunTImer;
                agent.speed = speed;
                Teleport();
            }
            else
            {
                agent.speed = 0f;
                agent.enabled = false;
                stunTimerVal -= Time.deltaTime; 
            }
        }
        else
        {
            stunTimerVal = stunTImer;
            agent.enabled = true;
        }

        if (knowsPosition) {
            if (agent.enabled)
            {
                agent.SetDestination(player.transform.position);
                lastKnownLocation = player.transform.position;
            }
        }
        else
        {
            if(agent.enabled)
            agent.SetDestination(lastKnownLocation); 
        }

        if(Vector2.Distance(player.transform.position, transform.position) <= stopingDistance)
        {
            if(attackTimerVal <= 0f)
            {
                player.GetComponent<HealthManager>().Damage(damage);
                attackTimerVal = attackTimer;
                cam.ShakeCamera(.25f , .12f);
                Teleport();
            }
            else
            {
                attackTimerVal -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "GameController")
            inside = true;
        else
            inside = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "GameController")
            inside = false;
    }


    public void Teleport()
    {
        float x = UnityEngine.Random.Range(0.0f, 1.0f);
        float y = UnityEngine.Random.Range(0.0f, 1.0f);

        Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(x, y));

        // Check the distance between the new position and the player's position
        float distanceToPlayer = Vector2.Distance(pos, player.transform.position);

        // Keep generating a new position until it meets the criteria
        while (distanceToPlayer < 5f)
        {
            x = UnityEngine.Random.Range(0.0f, 1.0f);
            y = UnityEngine.Random.Range(0.0f, 1.0f);

            pos = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
            distanceToPlayer = Vector2.Distance(pos, player.transform.position);
        }

        // Set the new position
        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position , playerSeeDistanceNoLight);
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
}
