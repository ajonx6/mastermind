using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour {
    public const int GUESS_SIZE = 4;

    public GameObject[] columns;
    public GameObject answerColumn;
    public Button button;
    public TMP_Text buttonText;

    private int turn = 0;
    private Transform myguess;
    private Transform hints;

    private Color[] colors = new Color[] {new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f), new Color(0.373f, 0.122f, 1f), new Color(1f, 0.373f, 0.122f)};
    private Color[] answer =  new Color[GUESS_SIZE];
    private Color red = new Color(1f, 0f, 0f);
    private Color white = new Color(1f, 1f, 1f);
    
    private bool gameOver = false;

    void Start() {
        NextTurn();

        for (int i = 0; i < answer.Length; i++) {
            int r = Random.Range(0,  colors.Length);
            answer[i] = colors[r];
            // Debug.Log(r);
        }
    }

    void NextTurn() {
        if (turn >= 10) Lose();

        myguess = columns[turn].transform.GetChild(1);
        hints = columns[turn].transform.GetChild(3);
        foreach (Transform gt in myguess) {
            gt.gameObject.GetComponent<PickColor>().EnableCell();
        }
    }

    public bool AllColorsDone() {
        foreach (Transform gt in myguess) {
            if (gt.gameObject.GetComponent<PickColor>().currentColorIndex == -1) return false;
        }
        return true;
    }

    public bool CheckGuess() {
        List<Color> guessList = new List<Color>();
        for (int i = 0; i < GUESS_SIZE; i++) {
            guessList.Add(myguess.GetChild(i).GetComponent<PickColor>().currentColor);
        }

        List<Color> answerList = new List<Color>();
        for (int i = 0; i < GUESS_SIZE; i++) {
            answerList.Add(answer[i]);
        }

        int numWhites = 0;
        for (int i = guessList.Count - 1; i >= 0; i--) {
            if (guessList[i] == answer[i]) {
                numWhites++;
                guessList.RemoveAt(i);
                answerList.RemoveAt(i);
            }
        }

        int numReds = 0;
        for (int i = guessList.Count - 1; i >= 0; i--) {
            for (int j = answerList.Count - 1; j >= 0; j--) {
                if (guessList[i] == answerList[j]) {
                    numReds++;
                    guessList.RemoveAt(i);
                    answerList.RemoveAt(j);
                    break;
                }
            }
        }

        for (int i = 0; i < numWhites; i++) {
            hints.GetChild(i).GetComponent<SpriteRenderer>().color = white;
        }

        for (int i = numWhites; i < numReds + numWhites; i++) {
            hints.GetChild(i).GetComponent<SpriteRenderer>().color = red;
        }

        return numWhites == 4;
    }

    public void Win() {
        Debug.Log("YOU WIN!");
        GameEnd();
    }

    public void Lose() {
        Debug.Log("You lose...");
        GameEnd();
    }

    public void GameEnd() {
        button.onClick.RemoveListener(NextTurn);
        button.onClick.AddListener(Restart);
        buttonText.text = "New\nGame";
        gameOver = true;
        for (int i = 0; i < answer.Length; i++) {
            answerColumn.transform.GetChild(1).GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = answer[i];
        }
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SubmitGuess() {
        if (gameOver || !AllColorsDone()) return;
        if (CheckGuess()) Win();
        else {  
            foreach (Transform gt in myguess) {
                gt.gameObject.GetComponent<PickColor>().active = false;
            }
            turn++;
            NextTurn();
        }
    }
}