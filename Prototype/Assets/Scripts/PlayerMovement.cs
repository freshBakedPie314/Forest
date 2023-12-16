using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines.Interpolators;
using Cinemachine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float dashDistance = 5f;
    public float dashTime = 0.5f;
    public bool isDashing = false;

    [Header("References")]
    public Rigidbody2D rb;
    public GameObject bullet;
    public Transform bulletsPwan;
    public Light2D light;
    public GameObject flashlight;
    public Animator animator;
    public Animator playerAnimator;
    public CinemachineVirtualCamera virtualCamera;
    
    public float nervous;
    public float nervousSpeed;

    bool hasGunEquipped = true;

    [Header("Attack")]
    public GameObject axe;
    public float attackRadius;
    public Transform point;
    public LayerMask enemies;

    private float dampingValX;
    public float dampingValY;
    // Start is called before the first frame update
    void Start()
    {
        dampingValX = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping;
        dampingValY = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        Vector3 mousepos = Input.mousePosition;
        Vector3 dir = Camera.main.ScreenToWorldPoint(mousepos) - transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(!isDashing)
            rb.rotation = angle;

        //Movement
        float verical = Input.GetAxis("Vertical");
        float horixontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horixontal, verical, 0);
        rb.velocity = movement * speed;

        if (movement != Vector3.zero)
            playerAnimator.SetBool("isWalking", true);
        else
            playerAnimator.SetBool("isWalking", false);

        //Attack
        axe.SetActive(!hasGunEquipped);

        if(Input.GetMouseButtonDown(0))
        {
            if (hasGunEquipped)
            {
                Instantiate(bullet, bulletsPwan.position, transform.rotation);
                playerAnimator.SetTrigger("shoot");
            }

            else
            {
                MeeleAttack();
            }
        }


        //Flashlight
        if(Input.GetMouseButtonDown(1))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (isDashing)
        {
            dir.z = transform.position.z;
            transform.Translate(dir.normalized * dashDistance * Time.deltaTime / dashTime, Space.World);

        }

        //Nervousness
        if (flashlight.activeInHierarchy)
        {
            nervous = Mathf.Lerp(5f, 0f, nervousSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E) && nervous <= 5f)
        {
            float val = Mathf.Lerp(light.intensity, 1.1f, .1f);
            light.intensity = val;
            nervous +=nervousSpeed;
        }else
        {
            float val = Mathf.Lerp(light.intensity, 0f, .1f);
            light.intensity = val;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            hasGunEquipped = !hasGunEquipped;
            if(hasGunEquipped)
                playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("Gun"), 1f);
            else
                playerAnimator.SetLayerWeight(playerAnimator.GetLayerIndex("Gun"), 0f);
        }
        
    }

    public void MeeleAttack()
    {
        playerAnimator.SetTrigger("swing");

        Collider2D[] enimies = Physics2D.OverlapCircleAll(point.position, attackRadius, enemies);

        foreach(Collider2D c in enimies)
        {
            HealthManager enemyHealth = c.GetComponent<HealthManager>();
            if(enemyHealth != null)
            {
                enemyHealth.Damage(20f);
                Debug.Log(c.transform.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(point.position, attackRadius);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0f;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0f;

        // Wait for the dash time
        yield return new WaitForSeconds(dashTime);

        // End the dash
        isDashing = false;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = dampingValX;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = dampingValY;
    }

}
