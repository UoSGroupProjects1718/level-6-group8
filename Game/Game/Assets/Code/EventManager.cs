using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* When adding new elements to this enum, please append them 
onto the end, otherwise everything in the inspector gets shifted */

public enum EventType
{
    Enter_Factory,
    Ingredient_Selected,
    Machine_Placed,
    Machine_Rotated,
    Machine_Deleted,
    Factory_Started,
    Level_Solved,
    Area_Unlocked,
    Machine_Selected,
    Cookbook_PageTurn,

    //
    Grinder_Execute,
    Brewer_Execute,
}

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance { get { return instance; } }

    private Queue<EventType> events;

    private void Update()
    {
        while (events.Count > 0)
        {
            HandleEvents();
        }
    }

    /// <summary>
    /// Adds an event to the queue
    /// </summary>
    /// <param name="_event"></param>
    public void AddEvent(EventType _event)
    {
        events.Enqueue(_event);
    }

    /// <summary>
    /// Handles all events for the frame
    /// </summary>
    void HandleEvents()
    {
        EventType _event = events.Peek();
        events.Dequeue();

        // If we're in the Level scene
        if (SceneManager.GetActiveScene().name == "Game")
        {
            // If it's a tutorial level, send the event to the Tutorial
            if (LevelController.Instance.LevelFactory.IsTutorial)
            {
                LevelController.Instance.LevelFactory.Tutorial.Progress(_event);
            }
        }

        // Event specific behaviour
        switch (_event)
        {
            case EventType.Level_Solved:

                // Is this is not a a tutorial level
                if (!LevelController.Instance.LevelFactory.IsTutorial)
                {
                    // Display the LevelComplete screen
                    LevelController.Instance.OnLevelComplete();

                    // Play sound
                    AudioManager.Instance.PlaySound(EventType.Level_Solved);
                }
                break;

            case EventType.Area_Unlocked:
            case EventType.Enter_Factory:
            case EventType.Ingredient_Selected:
            case EventType.Machine_Placed:
            case EventType.Machine_Deleted:
            case EventType.Machine_Rotated:
            case EventType.Machine_Selected:
            case EventType.Cookbook_PageTurn:

            case EventType.Grinder_Execute:
            case EventType.Brewer_Execute:
                AudioManager.Instance.PlaySound(_event);
                break;
        }
    }

	void Start ()
    {
        //! Singleton
        if (instance == null) instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        events = new Queue<EventType>();
        DontDestroyOnLoad(this.gameObject);
	}
}
