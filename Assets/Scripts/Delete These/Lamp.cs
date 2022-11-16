using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using LightType = UnityEngine.LightType;

/**
 * @Author Markus Larsson
 */
public class Lamp : MonoBehaviour{
	//Changing the state inside the inspector.
	[SerializeField] private bool isOn;
	
	//Array filled with the children which has light components.
	private Light[] lights;

	//Materials to be used for the light bulb when active and or inactive.
	[SerializeField] private Color colour;
	[SerializeField] private int materialIndex = -1;
	private Renderer meshRenderer;
	
	//Blinking variables
	[Header("Blinking variables")]
	[Tooltip("Whether or not the current lamp is flickering.")]
	[SerializeField] private bool isFlickering;
	[Tooltip("How long the light will be off.")]
	[SerializeField] private float blinkInterval = 5.0f;
	[Tooltip("How long the light will be on.")]
	[SerializeField] private float blinkLength = 5.0f;
	private bool currentlyFlickering;

	private void Start(){
		if(materialIndex < 0){
			Debug.LogWarning("Material Index for " + name + " is not set!");
		}

		//Reference to render.
		meshRenderer = GetComponent<Renderer>();

		lights = new Light[transform.childCount];
		for(int i = 0; i < transform.childCount; i++){
			Light comp = transform.GetChild(i).GetComponent<Light>();
			if(comp != null){
				lights[i] = comp;
			}
		}

		if(lights.Length <= 0){
			Debug.LogWarning(
				name + " has no children with light objects. Did you put this script on the light source? \n" + 
				"This object needs children with light objects.");
		}
		
		//Set the inspector state to the current state.
		SetState(isOn);
	}

	private void Update(){
		if(currentlyFlickering == false && isFlickering){
			currentlyFlickering = true;
			StartCoroutine(Flickering());
		}
	}

	/**
	 * Toggle the state of the light.
	 */
	public void ToggleState(){
		SetState(!isOn);
	}
	
	/**
	 * Set the state of the light.
	 *
	 * @param State the desired state of the light.
	 */
	public void SetState(bool desiredState){
		if(desiredState){
			TurnOn();
		} else{
			TurnOff();
		}
	}

	/**
	 * Turn the light on.
	 */
	public void TurnOn(){
		isOn = true;
		
		SetChildState(true);
		ChangeColour(true);
	}
	
	/**
	 * Turn the light off.
	 */
	public void TurnOff(){
		isOn = false;

		SetChildState(false);
		ChangeColour(false);
	}

	/**
	 * Flickers the lights until isFlickering is set to 'false'.
	 */
	private IEnumerator Flickering(){
		while(isFlickering){
			//Lights on.
			TurnOff();
			yield return new WaitForSeconds(blinkInterval);
			
			//Lights off.
			TurnOn();
			yield return new WaitForSeconds(blinkLength);
		}
		currentlyFlickering = false;
	}

	/**
	 * Loops through all child objects and either enables or disabled them.
	 *
	 * @Param desiredState What the state of the children is desired.
	 */
	private void SetChildState(bool desiredState)
	{
		foreach(Light l in lights){
			if (l != null)
			{
				l.enabled = desiredState;
			}
		}
	}

	/**
	 * Change the colour of the material for the object with the given material index.
	 *
	 * @Param emission Whether or not emission should be turned on or not.
	 */
	private void ChangeColour(bool emission){
		if(materialIndex < 0){return;}

		//Gets the array and replaces the material index with the desired state material.
		//Unity requires the entire array to be replaced.
		//'sharedMaterials' is required as otherwise it might leak materials into the
		//	scenes according to Unity.
		Material[] materials = meshRenderer.sharedMaterials;
		Material material = materials[materialIndex];
		
		if(emission){
			material.EnableKeyword("_EMISSION");
			material.SetColor(Shader.PropertyToID("_EmissionColor"), colour);
		} else{
			material.DisableKeyword("_EMISSION");
		}
		
		material.color = colour;
		materials[materialIndex] = material;
		meshRenderer.sharedMaterials = materials;
	}

	/**
	 * Updates the light if the inspectorState value has been updated.
	 */
	private void OnValidate(){
		Start();
	}
}
