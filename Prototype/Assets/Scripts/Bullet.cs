using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float despwanTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despwan());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right*speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<HealthManager>().health -= 50f;
        }
    }

    IEnumerator Despwan()
    {
        yield return new WaitForSeconds(despwanTimer);
        Destroy(gameObject);
    }
}
