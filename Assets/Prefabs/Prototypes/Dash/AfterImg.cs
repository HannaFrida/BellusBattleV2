using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;

public class AfterImg : MonoBehaviour
{
    [SerializeReference] private float activeTime = 0.5f;
    [Header ("Mesh related")]
    [SerializeReference] private float meshRefreshRate = 0.1f;
    private bool isTrailActive;
    [SerializeReference] private float shaderVarRate = 0.1f;
    [SerializeReference] private float shaderVarRefreshRate = 0.05f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Transform positionToSpawn;
    [SerializeReference] private float destroyDelay = 0.4f;
    [Header("shader related")]
    public Material mat;
    [SerializeReference] private string shaderVarRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartTrail()
    {
        if (!isTrailActive)
        {
            Debug.Log("hej");
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
  
    }
    IEnumerator ActivateTrail (float time)
    {
        while(time > 0)
        {
            time -= meshRefreshRate;
            
            

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }
                for(int i=0; i<skinnedMeshRenderers.Length; i++)
                {
                    GameObject meshes = new GameObject();
                    meshes.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                    MeshRenderer mr = meshes.AddComponent<MeshRenderer>();
                    MeshFilter mf = meshes.AddComponent<MeshFilter>();

                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);

                    mf.mesh = mesh;
                    mr.material = mat;
                StartCoroutine(AnimateMaterialFloat(mr.material, 0,shaderVarRate,shaderVarRefreshRate));
                Destroy(meshes, destroyDelay);

            }
            



            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }
    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);
        while( valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }

    }
}
