using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Transform player;
    public float threshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = (player.position + mousePos)/2f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, player.position.x - threshold, player.position.x + threshold);
        targetPosition.y = Mathf.Clamp(targetPosition.y, player.position.y - threshold, player.position.y + threshold);

        this.transform.position = targetPosition;
    }
}
