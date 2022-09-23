using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI playerHealth;
    public InputController Player;

    public string playerNames;
    public TextMeshProUGUI playerName;

    // Start is called before the first frame update
    void Start()
    {
        string[] names = new string[] { "Trapper", "Wraith", "Shape", "Nightmare", "Executioner", "Nemesis", "Blight", "Spirit", "Demo", "Cannibal", "Legion", "Plague", "Mastermind", "Oni" };
        string randomName = names[Random.Range(0, names.Length)];
        Debug.Log(randomName);
        playerName.text = randomName;
        Player = gameObject.GetComponentInParent<InputController>();
        playerHealth = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.text = Player.health.ToString();
    }
}
