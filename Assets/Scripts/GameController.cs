using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Text gameFinishedInfotText;
    [SerializeField] private Sprite bgImage;
    [SerializeField] AudioClip succesClip;
    [SerializeField] AudioClip wrongClip;
    [SerializeField] private Button restartButton;
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    private bool firstGuess;
    private bool secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int firstGuessIndex;
    private int secondGuessIndex;
    private int score;
    private int comboCount;
    private int maxComboBonus = 20;
    private int baseScorePerMatch = 10;
    private int comboBonus = 10;
    private string firstGuessPuzzle;
    private string secondGuessPuzzle;
    private bool lastMatchWasCorrect;
    private AudioSource AS;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Fruits");
        AS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        score = 0;
        comboCount = 0;
        lastMatchWasCorrect = false;
        UpdateScoreText();

        if(restartButton != null)
        {
            restartButton.gameObject.SetActive(false);

            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                RestartGame();
                restartButton.gameObject.SetActive(false);
            });
        }
    }

    private void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    private void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0;i < looper; i++)
        {
            if(index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    private void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() =>
            {
                PickAPuzzle();
            });
        }
    }

    public void PickAPuzzle()
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(CheckIfPuzzlesMatch());
        }
    }

    private IEnumerator CheckIfPuzzlesMatch()
    {
        yield return new WaitForSeconds(0.2f);

        if(firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {
            yield return new WaitForSeconds(0.2f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            //btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            //btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            if (lastMatchWasCorrect)
            {
                score += baseScorePerMatch + comboBonus;
                comboCount++;
                comboText.text = "You made Combo +" + comboBonus;
            }
            else
            {
                score += baseScorePerMatch;
                comboCount = 1;
                comboText.text = string.Empty;
            }

            lastMatchWasCorrect = true;
            AS.PlayOneShot(succesClip, 1);
            UpdateScoreText();

            CheckIfGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
            lastMatchWasCorrect = false;
            comboCount = 0;
            comboText.text = string.Empty;
            AS.PlayOneShot(wrongClip, 1);
        }

        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }

    private void CheckIfGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            gameFinishedInfotText.text = $"GAME FINISHED \n It took you {countGuesses} many guess(es) to finish game \n Yout Final Score is: {score}";

            if (restartButton != null)
            {
                restartButton.gameObject.SetActive(true);
            }
        }
    }

    private void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite tmp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = tmp;
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}