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
    // Create variable instance
    public static DatabaseManager instance;

    // Awake is called before Start
    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public LoggedUser CurrentUser { get; private set; }
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

    public IEnumerator CreateUser(string email, string username, string password, string passwordRepeat)
    {
        if (username == "")
        {
            MainMenuUIManager.instance.SetWarningText("Ingrese un nombre de usuario");
            yield return null;
        }
        else if (password != passwordRepeat)
        {
            MainMenuUIManager.instance.SetWarningText("Las contraseñas no coindicen");
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

    public IEnumerator LoginUser(string email, string password)
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

            MainMenuUIManager.instance.SetWarningText(message);


        }
        else
        {
            //User is now logged in
            //Now get the result
            var user = LoginTask.Result.User;
            CurrentUser = new LoggedUser(user.Email, user.DisplayName);

            StartCoroutine(MainMenuUIManager.instance.LogInEffect(user.DisplayName));

            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            // warningLoginText.text = "";
            // confirmLoginText.text = "Logged In";
            // StartCoroutine(LoadUserData());

            // Log out

            // usernameField.text = User.DisplayName;
            // UIManager.instance.UserDataScreen(); // Change to user data UI
            // confirmLoginText.text = "";
            // ClearLoginFeilds();
            // ClearRegisterFeilds();
        }
    }

    public IEnumerator LogOutUser()
    {
        //Call the Firebase auth sign out function
        _auth.SignOut();

        CurrentUser = null;

        //Wait until the task completes
        yield return new WaitUntil(predicate: () => _auth.CurrentUser == null);

        //Now return to login screen
        Debug.Log("Logged Out");

        MainMenuUIManager.instance.LogOutEffect();

        // UIManager.instance.LoginScreen();
    }
}

public class User : LoggedUser {
    public string password;

    public User() {
    }

    public User(string email, string username, string password): base(email, username) {
        this.password = password;
    }
}

public class LoggedUser
{
    public string email;
    public string username;

    public LoggedUser() {
    }

    public LoggedUser(string email, string username) {
        this.email = email;
        this.username = username;
    }
}


