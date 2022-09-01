using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToSkinnedMesh : MonoBehaviour
{
    [ContextMenu("Convert to skinned mesh")]
    void Convert()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
        
       
        DestroyImmediate(gameObject.GetComponent<MeshFilter>());
        DestroyImmediate(this);
    }
}
