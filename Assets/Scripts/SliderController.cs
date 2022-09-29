using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerName;
    public InputController Player;

    public string playerNames;

    void Start()
    {
        string[] names = new string[] { "Trapper", "Wraith", "Shape", "Nightmare", "Executioner", "Nemesis", "Blight", "Spirit", "Demo", "Cannibal", "Legion", "Plague", "Mastermind", "Oni", "Nurse", "Huntress", "Twins", "Onryo", "Dredge", "Ghostface", "Hillbilly", "Hag", "Doctor", "Clown", "Deathslinger", "Trickster", "Artist", "Cenobite", "Pig" };
        string randomName = names[Random.Range(0, names.Length)];
        playerName.text = randomName;
        Player = gameObject.GetComponentInParent<InputController>();
        playerHealth = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        playerHealth.text = Player.health.ToString();
    }
}
