using UnityEngine;
using System.Collections;

public class TestBinding : MonoBehaviour {

	// Use this for initialization
	public Color color;
	public Vector3 rot = Vector3.zero;
	
	//private Transform trans;
	
	void Start () {
		//trans = this.gameObject.transform;
		gameObject.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		//trans.Rotate( rot );
	}
}
