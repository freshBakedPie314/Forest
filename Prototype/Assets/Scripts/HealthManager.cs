using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HealthManager : MonoBehaviour
{
    public float health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0f)
            Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}
