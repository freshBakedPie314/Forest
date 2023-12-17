using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string noteText;
    public void NoteTrigger()
    {
        Debug.Log(noteText);
    }
}
