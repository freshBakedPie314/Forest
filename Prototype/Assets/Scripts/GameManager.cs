using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mousePosObject;
    public GameObject player;

    public float mouseMoveRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        if (Vector2.Distance(player.transform.position , mousePos) <= mouseMoveRadius)
        mousePosObject.transform.position = mousePos;
    }
}
