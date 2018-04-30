using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashUI : MonoBehaviour
{
    public GameObject SignInDialog;
    public InputField NicknameField;
    public Button SignInSignOutButton;

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void SignInSignOut()
    {
        var authServices = GetComponent<AuthServices>();
        if(authServices != null)
        {
            if(AuthServices.isSignedIn)
            {
                authServices.SignOut();
            } else
            {
                SignInDialog.SetActive(true);
            }
        }
    }

    public void CloseSignInDialog()
    {
        SignInDialog.SetActive(false);
    }

    public void ToggleNicknameField()
    {
        var buttonText = SignInSignOutButton.transform.Find("Text").GetComponent<Text>();
        NicknameField.gameObject.SetActive(!NicknameField.gameObject.activeSelf);
        buttonText.text = NicknameField.gameObject.activeSelf ? "Sign Up" : "Sign In";
    }
}
    