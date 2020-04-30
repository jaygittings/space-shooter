using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    State state;
    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        state = FindObjectOfType<State>();
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = state.GetScore().ToString();
    }
}
