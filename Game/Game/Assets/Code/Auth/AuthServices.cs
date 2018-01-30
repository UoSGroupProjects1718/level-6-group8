using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class AuthServices : MonoBehaviour
{
    public Toggle SignUpToggle;
    public InputField NickField;
    public InputField EmailField;
    public InputField PasswordField;
    public Text SignedInText;
    public Button SignInDialogButton;
    public static bool isSignedIn;

    private FirebaseAuth auth;
    private FirebaseUser user;

    // Use this for initialization
    void Start () {
	    auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        UpdateUIComponents();
	}

    public void SignOut()
    {
        if(isSignedIn)
        {
            auth.SignOut();
        } else
        {
            Debug.LogError("Can't log out - Not signed in!");
        }
    }

    void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                isSignedIn = false;
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignedIn = true;
            }
        }
		UpdateUIComponents();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void UpdateUIComponents()
    {
        UpdateSignedInText();
        UpdateSignInDialogButton();
    }

    void UpdateSignedInText()
    {
        SignedInText.text = isSignedIn ? auth.CurrentUser.DisplayName + " Signed in" : "Nobody signed in.";
    }

    void UpdateSignInDialogButton()
    {
        var buttonText = SignInDialogButton.gameObject.transform.GetChild(0).GetComponent<Text>();
        if(buttonText != null)
        {
            if (isSignedIn)
            {
                buttonText.text = "Sign Out";
            } else {
                buttonText.text = "Sign In/Register";
            }
        }
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
        if (IsEmailValid(EmailField.text))
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

    string SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(EmailField.text, PasswordField.text).ContinueWith(task => {
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
        Debug.Log("User with ID " + auth.CurrentUser.UserId + " is signed in.");
        return auth.CurrentUser.UserId;
    }

    string SignIn(string username, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(username, password).ContinueWith(task => {
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
        Debug.Log("User with ID " + auth.CurrentUser.UserId + " is signed in.");
        return auth.CurrentUser.UserId;
    }

    string SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(EmailField.text, PasswordField.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Sign up was cancelled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Sign up encountered an error: " + task.Exception);
            }

            FirebaseUser newUser = task.Result;
            GenerateUserProfile(newUser, NickField.text);
            Debug.LogFormat("Firebase User created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
        Debug.Log("User with ID " + auth.CurrentUser.UserId + " is signed in.");
        return auth.CurrentUser.UserId;
    }

    private void GenerateUserProfile(FirebaseUser newUser, string nickname)
    {
        if(newUser != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = nickname
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                if(task.IsCompleted)
                {
                    Debug.Log("User profile updated successfully.");
                    DBManager db = new DBManager();
                    db.WriteNewUser(newUser);
                }
                UpdateUIComponents();
            });
        }
    }

    void CloseSignupDialog()
    {
        gameObject.GetComponent<SplashUI>().CloseSignInDialog();
    }
}
