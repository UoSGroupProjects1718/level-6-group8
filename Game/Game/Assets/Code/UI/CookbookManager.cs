using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookManager : MonoBehaviour {
    [SerializeField]
    GameObject default_left;
    [SerializeField]
    GameObject default_right;

    [SerializeField]
    GameObject potion_left;
    [SerializeField]
    GameObject potion_right;

    public void detailedView(Potion pot)
    {
        default_left.SetActive(false);
        default_right.SetActive(false);

        UI_PotionPanel i = potion_right.GetComponent<UI_PotionPanel>();
        i.fillPanel(pot);

        potion_left.SetActive(true);
        potion_right.SetActive(true);
    }

    public void SelectionView()
    { 
        potion_left.SetActive(false);
        potion_right.SetActive(false);

        UI_PotionPanel i = potion_right.GetComponent<UI_PotionPanel>();
        i.clearPanel();

        default_left.SetActive(true);
        default_right.SetActive(true);
    }
}
