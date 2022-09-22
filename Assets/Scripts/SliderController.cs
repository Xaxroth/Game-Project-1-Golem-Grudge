using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI playerHealth;
    public InputController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.GetComponentInParent<InputController>();
        playerHealth = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.text = Player.health.ToString();
    }
}
