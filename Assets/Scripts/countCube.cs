using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countCube : MonoBehaviour {

	int count = 0;
	public TextMesh textCount;


	void Update(){
		textCount.text = count.ToString();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "cube" || other.tag == "cube1" || other.tag == "cube2" || other.tag == "cube3" || other.tag == "cube4") 
		{
		  count++;
		}

	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "cube" || other.tag == "cube1" || other.tag == "cube2" || other.tag == "cube3" || other.tag == "cube4") 
		{
			count--;
		}

		if (other.tag == "cubee") 
		{
			count--;
		}
	}
}
