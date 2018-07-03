using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	
	public bool _isAiming;
	public GameObject _particles;
	public float _rotateSpeed;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
		if (_isAiming)
		{
			
			_particles.SetActive (false);
			
		} else {
			
			_particles.SetActive(true);
			
		}
		
		if (_particles.activeSelf == true)
		{
			
			_particles.transform.Rotate (Vector3.forward * _rotateSpeed);
			
		}
		
	}
	
}