using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject btn;
    [SerializeField] private int maxBtn = 8;

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        for(int i = 0; i < maxBtn; i++)
        {
            GameObject button = Instantiate(btn, puzzleField, false);
            button.name = "" + i;
        }
    }
}