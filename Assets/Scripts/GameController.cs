using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public GameObject questionDisplayer;
	public GameObject gameoverDisplayer;
	public Text questionDisplay;
	public Text scoreText;
	public Text timeText;
	public BasicObjectPool answerButtonPool;
	public Transform answerButtonParentTransform;
	public int playerScore;

	private DataController dataController;
	private RoundData roundData;
	private QuestionData[] question;
	private List<GameObject> answerButtonObject = new List<GameObject> ();
	private bool roundIsActive;
	private int questionIndex;
	private float timeRemaining;

	void Start () {
		playerScore = 0;
		roundIsActive = true;

		dataController = FindObjectOfType<DataController> ();
		roundData = dataController.GetCurrentRoundData ();
		question = roundData.questions;
		timeRemaining = roundData.timeLimitInSeconds;

		ShowQuestions ();
	}

	private void ShowQuestions (){
		RemoveAnswerButtons ();

		QuestionData currentQuestion = question [questionIndex];
		questionDisplay.text = currentQuestion.questionText;

		//GameObject buttonObject = null;
		foreach(AnswerData answer in currentQuestion.answers){
			GameObject buttonObject = answerButtonPool.GetObject ();
			buttonObject.transform.SetParent (answerButtonParentTransform);

			AnswerButton answerButton = buttonObject.GetComponent<AnswerButton> ();
			answerButton.Setup (answer);

			answerButtonObject.Add (buttonObject);
		}
	}

	private void RemoveAnswerButtons (){
		while (answerButtonObject.Count > 0) {
			answerButtonPool.ReturnObject (answerButtonObject [0]);
			answerButtonObject.RemoveAt (0);
		}
	}

	public void AnswerClicked(bool isCorrect){
		if (isCorrect) {
			playerScore += roundData.pointsAddedForCorrectAnswer;
			scoreText.text = "Score: " + playerScore.ToString();
		}

		if (question.Length > questionIndex + 1) {
			questionIndex++;
			ShowQuestions ();
		} else {
			EndRound ();
		}
	}

	public void EndRound () {
		roundIsActive = false;
		questionDisplayer.SetActive (false);
		gameoverDisplayer.SetActive (true);
	}

	public void StartOver () {
		SceneManager.LoadScene ("MenuScreen");
	}

	private void UpdateTime () {
		timeText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
	}

	void Update () {
		if (roundIsActive) {
			timeRemaining -= Time.deltaTime;
			UpdateTime ();
			if (timeRemaining <= 0) {
				EndRound ();
			}
		}
	}
}
