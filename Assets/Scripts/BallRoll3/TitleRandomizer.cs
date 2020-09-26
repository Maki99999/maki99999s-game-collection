using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallRoll3 {
public class TitleRandomizer : MonoBehaviour {

	static string[] names = new string[] {	"Roll Ball", "Roll Ball 3", "Roll Sphere", "Roll Sphere 3", 
									"Rolling Ball", "Rolling Sphere", "Rolling Ball 3", 
									"Rolling Sphere 3", "Ball Roll 3", "Ball Roll", "Sphere Roll", "Sphere Roll 3", 
									"Ball Rolling 3", "Ball Rolling", "Sphere Rolling", "Sphere Rolling 3"};

	void Start () {
		GetComponent<Text>().text = GetRandomName();
	}

	public static string GetRandomName() {
		int randomValue = Random.Range(0, names.Length - 1);
		return names[randomValue];
	}
}
}