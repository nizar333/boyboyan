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
		if (other.tag == "cube") 
		{
		  count++;
		}

	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "cube") 
		{
			count--;
		}

		if (other.tag == "cubee") 
		{
			count--;
		}
	}
}
