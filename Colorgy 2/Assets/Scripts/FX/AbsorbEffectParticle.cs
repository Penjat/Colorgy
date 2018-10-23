using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbEffectParticle : MonoBehaviour {
	
	private static string TAG = "ABSORB PARTICLE: ";

	private GameObject target;
	public Renderer rend;

	private float speed = 0.0f;
	private static float maxSpeed = 120.0f;
	private static float acceleration = 20.0f;

	public void SetUp(GameObject g,int val){
		target = g;
		Color c = Calc.GetColor(val);
		c.a = 0.4f;
		rend.material.color = c;
		//TODO change the color
	}
	public bool MoveToTarget(){
		//returns true when it reaches the target
		//returns false if is still looking
		if(!target){
			return false;
		}
		speed += acceleration * Time.deltaTime;
		if(speed > maxSpeed){
			speed = maxSpeed;

		}

		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

		float distance = Vector3.Distance(target.transform.position, transform.position);

		//when it reaches the target
		if(Mathf.Abs(distance)< 0.05f){
			//Debug.Log(TAG + "destroying partilce");
			return true;
		}
		return false;
	}
}
