using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashUI : MonoBehaviour
{
    public GameObject SignInDialog;
    public InputField NicknameField;

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void OpenSignInDialog()
    {
        SignInDialog.SetActive(true);
    }

    public void CloseSignInDialog()
    {
        SignInDialog.SetActive(false);
    }

    public void ToggleNicknameField()
    {
        NicknameField.gameObject.SetActive(!NicknameField.gameObject.activeSelf);
    }
}
