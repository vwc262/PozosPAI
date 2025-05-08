using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Lean.Touch;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.EventSystems;

public class RayCastSelectSitio : MonoBehaviour
{
    // Hola Boy

    public Camera camera;

    public LayerMask hitLayer;
    public float rayDistance = 4000;
    
    public GameObject proxy;

    public Vector2 mousePos;
    public Vector3 rayOrigin;
    public Vector3 rayDir;

    public float gizmosSphereRadius = 100;
    public Vector3[] spherePositions;
    public bool drawGizmos;

    public float screenWidth = 1920;
    public float screenWidthFactor = 1;
    public float screenHeight = 1080;
    public float screenHeightFactor = 1;
    
    public void DoClickOverMap(Vector2 _dirRay)
    {
	    mousePos.x = _dirRay.x;
	    mousePos.y = _dirRay.y;
	    
	    RaycastHit hit;
	    Ray ray =  GetRay();

	    if (Physics.Raycast(ray, out hit,hitLayer))
	    {
		    proxy.transform.position = hit.point;

		    if (hit.collider.GetComponentInParent<ControlMarcadorSitio>() != null)
		    {
			    hit.collider.GetComponentInParent<ControlMarcadorSitio>().SetSelectedSitio();
			    hit.collider.GetComponentInParent<ControlMarcadorSitio>().CreateSphere();
		    }
	    }
    }
    
    [Button]
    public Ray GetRay()
    {
	    Ray ray;
		    
	    if (camera.orthographic)
	    {
		    ray = camera.ScreenPointToRay(new Vector3(
			    mousePos.x * screenWidth*screenWidthFactor, 
			    mousePos.y * screenHeight*screenHeightFactor, 
			    0));
	    }
	    else
	    {
		    var posX = Mathf.Lerp(-112*screenWidthFactor, 112*screenWidthFactor, mousePos.x);
		    var posY = Mathf.Lerp(-64*screenHeightFactor, 64*screenHeightFactor, mousePos.y);
		    var posZ = 100;

		    rayDir = new Vector3(posX, posY, posZ);
		    rayDir = transform.TransformVector(rayDir);
		    rayOrigin = camera.transform.position;
		    
		    ray = new Ray(rayOrigin, rayDir);
	    }
	    
	    if(drawGizmos)
		    Debug.DrawLine(ray.origin, ray.origin + rayDistance *ray.direction, Color.cyan, 1);
	    
	    return ray;
    }
    
    public void OnDrawGizmos()
    {
		if(!drawGizmos)
			return;
		
	    Gizmos.color = Color.cyan;
	    for (float i = 0; i <= 1; i+= 0.1f)
	    {
		    var pos = Vector3.Lerp(camera.transform.position, proxy.transform.position, i);
			Gizmos.DrawSphere(pos, 600*i);
	    }	   
	    Gizmos.color = Color.white;
	    for (int i = 0; i < spherePositions.Length; i++)
	    {
		    var pos = transform.position +  transform.TransformVector(spherePositions[i]);
			Gizmos.DrawSphere(pos, gizmosSphereRadius);
	    }
    }
	
	void OnEnable()
	{
		LeanTouch.OnFingerTap += HandleFingerTap;
	}

	void OnDisable()
	{
		LeanTouch.OnFingerTap -= HandleFingerTap;
	}

	void HandleFingerTap(Lean.Touch.LeanFinger finger)
	{
		//Debug.Log("You just tapped the screen with finger " + finger.Index + " at " + finger.ScreenPosition);
		DoClickOverMap(finger.ScreenPosition);
	}
}
