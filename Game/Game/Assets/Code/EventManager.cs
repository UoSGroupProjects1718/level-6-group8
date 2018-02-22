using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Enter_Factory,
    Ingredient_Selected,
    Factory_Started,
    Level_Solved,
}

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance { get { return instance; } }

    private Queue<EventType> events;

    private void Update()
    {
        if (events.Count > 0)
        {
            HandleEvents();
        }
    }

    public void AddEvent(EventType _event)
    {
        events.Enqueue(_event);
    }

    void HandleEvents()
    {
        EventType _event = events.Peek();
        events.Dequeue();

        // If it's a tutorial level, send the event to the Tutorial
        if (LevelController.Instance.LevelFactory.IsTutorial)
        {
            LevelController.Instance.LevelFactory.Tutorial.Progress(_event);
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
                }
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
