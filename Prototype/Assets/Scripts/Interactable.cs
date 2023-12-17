using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactEvent;
    public bool canInteract;
    public KeyCode interactKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactKey) && canInteract)
        {
            interactEvent.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
            canInteract = true;
        else
            canInteract = false;
    }
}
