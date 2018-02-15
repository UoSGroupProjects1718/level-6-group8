using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
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

        switch (_event)
        {
            case EventType.Level_Solved:

                // Is this a tutorial level?
                if (LevelController.Instance.LevelFactory.IsTutorial)
                {
                    // Progress further in the tutorial
                    LevelController.Instance.LevelFactory.Tutorial.Progress();
                }

                // If not
                else
                {
                    // Level is complete!
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
