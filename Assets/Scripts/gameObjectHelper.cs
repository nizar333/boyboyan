﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gameObjectHelper : MonoBehaviour {
	public static void SendMessageToAll (string methodName, params object[] parameters)
	{
		HashSet<GameObject> objectsToCall;
		objectsToCall = FindGameObjectsWithComponent (typeof(MonoBehaviour));

		object callParameter = (parameters != null && parameters.Length == 1) ? parameters [0] : parameters;
		foreach (GameObject gameObject in objectsToCall) {
			gameObject.SendMessage (methodName, callParameter, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static HashSet<GameObject> FindGameObjectsWithComponent (Type type)
	{
		HashSet<GameObject> objectsWithComponent = new HashSet<GameObject> ();

		Component[] targetComponents = (Component[])GameObject.FindObjectsOfType (type);
		for (int index = 0; index < targetComponents.Length; index++) {
			objectsWithComponent.Add (targetComponents [index].gameObject);
		}
		return objectsWithComponent;
	}
}
