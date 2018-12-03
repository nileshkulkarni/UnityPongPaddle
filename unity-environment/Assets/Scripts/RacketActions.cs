using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RacketActions : MonoBehaviour {

	Vector3 originalPos;
	// public Rigidbody racket;
	public GameObject myPong;
	public float posUp = 0.1f;
	public float velUp = 0.3f;
	Vector3 init_normal = new Vector3(0.0f, 1.0f, 0.0f);

	public System.Random rand = new System.Random(); 
	Vector3 wind_force = new Vector3(1.0f, 0.0f, 0.0f);
	public float wind_mag = 0.1f;
	public Boolean useWind = false;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		useWind = true;
		velUp = 1.0f + Math.Min(1.0f, Math.Max(-0.3f, 1.0f*random_normal()));
		wind_mag = 0.1f;
//		wind_force = wind_mag * wind_force;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("space")) {
			// Vector3 pos = transform.position;
			// pos.y += upSize;
			// transform.position = pos;
			Rigidbody r = gameObject.GetComponent<Rigidbody>();
			// r.AddForce(0, posUp, 0);
			// r.velocity = new Vector3(0, posUp, 0);
			r.position += new Vector3(0, posUp, 0);

			// racket.AddForce(0, upSize, 0);
		} else {
			// checkPosition();
		}
		Rigidbody rpong = myPong.GetComponent<Rigidbody>();
		if (useWind){
			rpong.AddForce(wind_mag * wind_force);
		}
	}

	void checkPosition() {
		if (originalPos == transform.position) return;
		if (Vector3.Distance(originalPos, transform.position) > 0.1f) {
			Vector3 to = originalPos - transform.position;
			print(to.magnitude);
			to.Normalize();
			to *= 0.05f;
			transform.position += to;
		} else {
			transform.position = originalPos;
		}
	}

//	void OnCollisionEnter(Collision col) {
//		if (col.gameObject == myPong) {
//			Rigidbody r = myPong.GetComponent<Rigidbody>();
//			// r.AddForce(0, -velUp, 0);
//			r.velocity = new Vector3(r.velocity.x, r.velocity.y + velUp, r.velocity.z);
//			// print(Time.frameCount + ": " + r.velocity);
//		}
//	}
	void OnCollisionEnter(Collision col) {
		Rigidbody racketR = gameObject.GetComponent<Rigidbody>();
		Matrix4x4 m = Matrix4x4.Rotate(racketR.rotation);

		Vector3 new_normal = m.MultiplyPoint3x4(init_normal);
		if (col.gameObject == myPong) {
			Rigidbody r = myPong.GetComponent<Rigidbody>();

			Vector3 force = velUp * new_normal;
			//print("rot matrix :" + m);
			//print("new_normal :" + new_normal);
			//print("Force :" + force);
			//print("before velocity :" + r.velocity);
			r.AddForce(force, ForceMode.Impulse);
			//print("after velocity :" + r.velocity);

//			r.velocity = new Vector3(r.velocity.x, r.velocity.y + velUp, r.velocity.z);
//			print(Time.frameCount + ": " + r.velocity);
		}
	}









	float random_normal(){
		//reuse this if you are generating many
		double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
		double u2 = 1.0-rand.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
			Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		double randNormal = 0.0f + 1.0f * randStdNormal;
		return (float)randNormal;
	}
}

