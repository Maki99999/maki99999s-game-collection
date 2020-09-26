using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FalseTruth {
public abstract class Checkpoints : MonoBehaviour {

	public static void SetCheckpoint(float occultValueChange) {
		SetCheckpoint(PlayerPrefs.GetInt("Checkpoint") + 1, occultValueChange);
	}

	public static void SetCheckpoint(int point, float occultValueChange) {
		PlayerPrefs.SetInt("Checkpoint", point);
		PlayerPrefs.SetFloat("OccultLevel", PlayerPrefs.GetFloat("OccultLevel", 0) + occultValueChange);
	}

	public abstract void LoadCheckpoint();

}
}