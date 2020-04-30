using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    Player player;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
        text.text = player.GetHealth().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = player.GetHealth().ToString();
    }
}
