using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallColour
    {
        Yellow,
        Red,
        Green,
        Blue,
        Purple
    }

    [SerializeField]
    BallColour colour;

    public BallColour GetBallColour() { return colour; }

	void Start ()
    {

	}
	
	void Update ()
    {
		
	}
}
