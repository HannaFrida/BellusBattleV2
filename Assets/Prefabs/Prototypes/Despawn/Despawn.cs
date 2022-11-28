using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Despawn : MonoBehaviour
{
    [SerializeReference] VisualEffect visualEffect;


    // Start is called before the first frame update
    public void SetMesh(Mesh mesh)
    {
        Debug.Log(mesh);
        visualEffect.SetMesh("MeshToMaterialze", mesh);
        visualEffect.Play();
    }
}
