using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runtime : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    
    void LateUpdate()
    {
        Surface2D.BuildNavMesh();
    }
}
