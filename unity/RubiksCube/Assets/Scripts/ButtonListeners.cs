using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListeners : MonoBehaviour
{
    private GameManager gameManager = null;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        addListeners();
    }

    private void addListeners()
    {
        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => gameManager.cubeAction(button.name));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
