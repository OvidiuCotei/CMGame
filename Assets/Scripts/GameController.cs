using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;
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
    private string firstGuessPuzzle;
    private string secondGuessPuzzle;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Fruits");
    }

    private void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
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

        if(firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.2f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            //btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            //btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckIfGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }

    private void CheckIfGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game Finished");
            Debug.Log("It took ypu " + countGuesses + " many guess(es) to finish the game");
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
}