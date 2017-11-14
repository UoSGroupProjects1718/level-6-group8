using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject Player;
    private Player playerStats;
    public Text Money;

	// Use this for initialization
	void Start ()
	{
        Player = GameObject.FindGameObjectWithTag("Player");
	    playerStats = Player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	   // Money.text = playerStats.Money.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
	}
}
