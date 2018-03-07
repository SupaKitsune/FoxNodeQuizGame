using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour {
	public RoundData[] allRoundData;
	public int currentRound = 0;

	void Start () {
		DontDestroyOnLoad (gameObject);
		SceneManager.LoadScene ("MenuScreen");
	}

	public RoundData GetCurrentRoundData () {
		return allRoundData[currentRound];
	}
}
