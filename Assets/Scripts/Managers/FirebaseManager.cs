using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    // Register variables
    [Header("Register")]
    [SerializeField] private TMP_InputField usernameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterVerifyField;
    [SerializeField] private TMP_Text warningRegisterText;

    [Header("Login")]
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private TMP_Text warningLoginText;

    // TODO: Move to an UI Controller
    [Header("UserData")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text clickToLoginText;
    [SerializeField] private GameObject loginMenu;

    private DatabaseReference _dbReference;
    private FirebaseAuth _auth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
                // StartCoroutine(CreateUser("alice@alice.alice", "alice", "alicealice", "alicealice"));
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    protected void InitializeFirebase()
    {
        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;
    }

    public void Register() => StartCoroutine(CreateUser(emailRegisterField.text, usernameRegisterField.text, passwordRegisterField.text, passwordRegisterVerifyField.text));

    public void LogIn() => StartCoroutine(LoginUser(emailLoginField.text, passwordLoginField.text));

    public void LogOut() => StartCoroutine(LogOutUser());

    private IEnumerator CreateUser(string email, string username, string password, string passwordRepeat)
    {
        if (username == "")
        {
            warningRegisterText.text = "Ingrese un nombre de usuario";
            yield return null;
        }
        else if (password != passwordRepeat)
        {
            warningRegisterText.text = "Las contraseñas no coindicen";
            yield return null;
        }

        User newUser = new User(email, username, password);

        // Call the Firebase auth signin function passing the email and password
        Task<AuthResult> RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(newUser.email, newUser.password);

        // Wait until the task completes
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            // If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Error en el registro";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Ingrese un email";
                    break;
                case AuthError.MissingPassword:
                    message = "Ingrese una contraseña";
                    break;
                case AuthError.WeakPassword:
                    message = "La contraseña ingresada es débil";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "El email ingresado ya está en uso";
                    break;
            }
            Debug.Log(message);
            // Autologin
            StartCoroutine(LoginUser(newUser.email, newUser.password));
        }
        else
        {
            //User has now been created
            //Now get the result
            var user = RegisterTask.Result.User;

            if (user != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile{DisplayName = newUser.username};

                //Call the Firebase auth update user profile function passing the profile with the username
                Task ProfileTask = user.UpdateUserProfileAsync(profile);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    // warningRegisterText.text = "Username Set Failed!";
                }
                else
                {
                    //Username is now set
                    //Now return to login screen
                    // UIManager.instance.LoginScreen();                        
                    // warningRegisterText.text = "";
                }
            }
        }
    }

    private IEnumerator LoginUser(string email, string password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Error de inicio de sesión";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Ingrese un email";
                    break;
                case AuthError.MissingPassword:
                    message = "Ingrese una contraseña";
                    break;
                case AuthError.WrongPassword:
                    message = "Contraseña inválida";
                    break;
                case AuthError.InvalidEmail:
                    message = "Email inválido";
                    break;
                case AuthError.UserNotFound:
                    message = "El usuario no existe";
                    break;
            }
            Debug.Log(message);
            warningLoginText.text = message;
            warningLoginText.color = Color.red;
            warningLoginText.gameObject.SetActive(true);
        }
        else
        {
            //User is now logged in
            //Now get the result
            var user = LoginTask.Result.User;

            warningLoginText.text = "¡Bienvenido " + user.DisplayName + "!";
            warningLoginText.color = Color.green;
            warningLoginText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.5f);
            warningLoginText.text = "";
            warningLoginText.color = Color.red;
            warningLoginText.gameObject.SetActive(false);

            loginMenu.SetActive(false);
            clickToLoginText.gameObject.SetActive(false);
            usernameText.text = user.DisplayName;
            // Center horizontally usernameText
            usernameText.verticalAlignment = VerticalAlignmentOptions.Middle;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            // warningLoginText.text = "";
            // confirmLoginText.text = "Logged In";
            // StartCoroutine(LoadUserData());

            // Log out
            StartCoroutine(LogOutUser());

            // usernameField.text = User.DisplayName;
            // UIManager.instance.UserDataScreen(); // Change to user data UI
            // confirmLoginText.text = "";
            // ClearLoginFeilds();
            // ClearRegisterFeilds();
        }
    }

    private IEnumerator LogOutUser()
    {
        //Call the Firebase auth sign out function
        _auth.SignOut();

        //Wait until the task completes
        yield return new WaitUntil(predicate: () => _auth.CurrentUser == null);

        //Now return to login screen
        Debug.Log("Logged Out");

        // UIManager.instance.LoginScreen();
    }
}

public class User {
    public string email;
    public string username;
    public string password;

    public User() {
    }

    public User(string email, string username, string password) {
        this.email = email;
        this.username = username;
        this.password = password;
    }
}


