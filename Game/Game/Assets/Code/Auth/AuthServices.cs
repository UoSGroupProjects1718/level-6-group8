using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class AuthServices : MonoBehaviour
{
    public Toggle SignUpToggle;
    public InputField UserField;
    public InputField PasswordField;

    private FirebaseAuth auth;
	// Use this for initialization
	void Start () {
	    auth = FirebaseAuth.DefaultInstance;
	    auth.StateChanged += AuthStateChanged;
	    AuthStateChanged(this, null);
	}

    void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool IsEmailValid(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("Email field is empty.");
            return false;
        }

        Regex _regex = CreateEmailRegex();

        // Use RegEx implementation if it has been created, otherwise use a non RegEx version.
        if (_regex != null)
        {
            return _regex.Match(email).Length > 0;
        }

        return false;
    }

    Regex CreateEmailRegex()
    {
        const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
        return new Regex(pattern, options);
    }

    public void SignInOrSignUp()
    {
        if (IsEmailValid(UserField.text))
        {
            if (SignUpToggle.isOn)
            {
                SignUp();
            }
            else
            {
                SignIn();
            }
            CloseSignupDialog();
        }
        else
        {
            Debug.LogError("Invalid email format.");
        }
    }

    void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(UserField.text, PasswordField.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        });

    }

    void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(UserField.text, PasswordField.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Sign up was cancelled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Sign in encountered an error: " + task.Exception);
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase User created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    void CloseSignupDialog()
    {
        gameObject.GetComponent<SplashUI>().CloseSignInDialog();
    }
}
