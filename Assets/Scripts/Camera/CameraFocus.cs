using Cinemachine;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class CameraFocus : MonoBehaviour
{
	[SerializeField] public List<Transform> _targets;
	private CinemachineTargetGroup _targetGroup;

	private float timer;
	private bool hasReachedTime;

	private void Start(){
		_targets = new();
        _targetGroup = GetComponent<CinemachineTargetGroup>();

    }

    private void Update()
    {
		if (hasReachedTime) return;

        if(timer >= 0.2f)
        {
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");  // Used for when changing level
			foreach (GameObject tr in players)
			{
				_targets.Add(tr.transform);
                _targetGroup.AddMember(tr.transform, 1, 1);
            }
            hasReachedTime = true;
		}
		timer += Time.deltaTime;
    }

	public bool AddTarget(Transform t)
	{
		if (_targets.Contains(t)){return false;}
		
		_targets.Add(t);
		_targetGroup.AddMember(t, 1, 1);
        return true;
	}
	public bool RemoveTarget(Transform t){
		if (_targets.Contains(t) == false){return false;}
		
		_targets.Remove(t);
        _targetGroup.RemoveMember(t);
        return true;
	}
}
/*
 *Markus gamla script
 *[RequireComponent(typeof(Camera))]
 *
 	[SerializeField, Tooltip("Offset relative to its' starting position.")]
	private Vector3 offset;
	[SerializeField, Tooltip("How smooth the camera repositions itself.")]
	private float smoothTime = 0.5f;
	[SerializeField, Tooltip("The furthest out the camera can zoom out.")]
	private float minZoom = 40.0f;
	[SerializeField, Tooltip("The closest in the camera can zoom in.")]
	private float maxZoom = 10.0f;
	[SerializeField] 
	private float zoomLimiter = 50.0f;
	[SerializeField, Tooltip("Whether or not start zoom should be used.")]
	private bool useStartZoom = true;

	[SerializeField] public List<Transform> _targets;
	private Vector3 _velocity;
	private Camera _cam;
	private bool _isOrthographic;
	private Vector3 startPos;

	private float timer;
	private bool hasReachedTime;

	private void Start(){
		_targets = new();
		_cam = GetComponent<Camera>();
	}

    private void Update()
    {
		if (hasReachedTime) return;

        if(timer >= 0.2f)
        {
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");  // Used for when changing level
			foreach (GameObject tr in players)
			{
				_targets.Add(tr.transform);
			}
			hasReachedTime = true;
		}
		timer += Time.deltaTime;
    }

    private void LateUpdate()
	{
		Bounds bounds = GetTargetsBounds();
		Reposition(bounds);
		Focus(bounds);
	}

    
     //* Adds a target for the camera to follow.
     //* Returns false if a copy of the given transform already exists.
     
public bool AddTarget(Transform t)
{
	if (_targets.Contains(t)) { return false; }

	_targets.Add(t);
	return true;
}


//*Removes a target for the camera to stop following.
// * Returns false if the given transform never existed.


//public bool RemoveTarget(Transform t)
//{
//    if (_targets.Contains(t) == false) { return false; }

//    _targets.Remove(t);
//    return true;
//}

///*
// * Repositions the camera over time to the center of the bounds offset with the given offset.
 
private void Reposition(Bounds bounds)
{
	Vector3 median = GetMedian(bounds);
	Vector3 newPos = median + offset;
	transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, smoothTime);
}


//*Focuses the camera over time based on the width of the bounding box scaled with the zoom limiter.


private void Focus(Bounds bounds)
{
	float newZoom = Mathf.Lerp(maxZoom, minZoom, (bounds.size.x + bounds.size.y) / zoomLimiter);
	if (_isOrthographic)
	{
		_cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, newZoom, Time.deltaTime);
	}
	else
	{
		_cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
	}

}


//*Returns the center of the bounding box.


private Vector3 GetMedian(Bounds bounds)
{
	if (_targets.Count == 1)
	{
		return _targets[0].position;
	}

	return bounds.center;
}


//*Gets the a bounding box encapsulating all targets.


private Bounds GetTargetsBounds()
{
	Vector3 centerPos = _targets.Count == 0 ? Vector3.zero : _targets[0].position;

	Bounds bounds = new Bounds(centerPos, Vector3.zero);
	for (int i = 0; i < _targets.Count; i++)
	{
		bounds.Encapsulate(_targets[i].position);
	}
	return bounds;
}

private void OnValidate()
{
	_isOrthographic = gameObject.GetComponent<Camera>().orthographic;
}
*/