using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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

    public async Task CreateUser(string email, string username, string password, string passwordRepeat)
    {
        if (username == "")
        {
            throw new Exception("Ingrese un nombre de usuario");
        }
        else if (password != passwordRepeat)
        {
            throw new Exception("Las contraseñas no coindicen");
        }

        User newUser = new User(email, username, password);

        // Call the Firebase auth signin function passing the email and password
        Task<AuthResult> RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(newUser.email, newUser.password);
        await RegisterTask;

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
            throw new Exception(message);
        }

        // User has now been created
        // Now get the result
        var user = RegisterTask.Result.User;

        // Create a user profile and set the username
        UserProfile profile = new UserProfile{DisplayName = newUser.username};

        // Call the Firebase auth update user profile function passing the profile with the username
        Task ProfileTask = user.UpdateUserProfileAsync(profile);
        await ProfileTask;

        if (ProfileTask.Exception != null)
        {
            // If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
            throw new Exception("Error al crear el usuario");
        }
    }

    public async Task LoginUser(string email, string password, bool isAutoLogin = false)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
        //Wait until the task completes
        try {
            await LoginTask;
        } catch (Exception e) {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {e}");
            FirebaseException firebaseEx = e.GetBaseException() as FirebaseException;
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
                case AuthError.UserDisabled:
                    message = "El usuario fue baneado";
                    break;
            }
            Debug.Log(message);
            throw new Exception(message);
        }

        // User is now logged in
        // Now get the result
        var user = LoginTask.Result.User;
        CurrentUser = new LoggedUser(user.UserId, user.Email, user.DisplayName);

        Debug.LogFormat("User signed in successfully: {0} ({1})", user.UserId, user.DisplayName);
    }

    public void LogOutUser()
    {
        // Call the Firebase auth sign out function
        _auth.SignOut();

        CurrentUser = null;

        // Now return to login screen
        Debug.Log("Logged Out");
    }

    public async Task SetRankingEntry(string level, int score, int kills, int killsScore, long time, int timeScore)
    {
        if (CurrentUser == null)
        {
            throw new Exception("No hay un usuario logueado");
        }

        RankingEntry newEntry = new RankingEntry(CurrentUser.username, score, kills, killsScore, time, timeScore);
        string json = JsonUtility.ToJson(newEntry);

        Task DBTask = _dbReference.Child("ranking").Child(level).Child(CurrentUser.id).SetRawJsonValueAsync(json);

        try
        {
            await DBTask;
        }
        catch (Exception e)
        {
            throw new Exception("Error al guardar el puntaje: " + e.Message);
        }
    }

    public async Task<RankingEntry> GetRankingEntry(string level)
    {
        if (CurrentUser == null)
        {
            throw new Exception("No hay un usuario logueado");
        }

        Task<DataSnapshot> DBTask = _dbReference.Child("ranking").Child(level).Child(CurrentUser.id).GetValueAsync();

        try
        {
            await DBTask;
        }
        catch (Exception e)
        {
            throw new Exception("Error al cargar el puntaje: " + e.Message);
        }

        // Data has been retrieved
        DataSnapshot snapshot = DBTask.Result;
        RankingEntry entry = null;

        if (snapshot.Exists)
        {
            entry = new RankingEntry(
                snapshot.Child("username").Value.ToString(),
                int.Parse(snapshot.Child("score").Value.ToString()),
                int.Parse(snapshot.Child("kills").Value.ToString()),
                int.Parse(snapshot.Child("killsScore").Value.ToString()),
                long.Parse(snapshot.Child("time").Value.ToString()),
                int.Parse(snapshot.Child("timeScore").Value.ToString())
            );
        }

        return entry;
    }

    public async Task<List<RankingEntry>> GetRanking(string level, int page)
    {
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = _dbReference.Child("ranking").Child(level).OrderByChild("score").LimitToFirst(page).GetValueAsync();

        try
        {
            await DBTask;
        }
        catch (Exception e)
        {
            throw new Exception("Error al cargar el ranking: " + e.Message);
        }

        // Data has been retrieved
        DataSnapshot snapshot = DBTask.Result;
        List<RankingEntry> scoreboardEntries = new List<RankingEntry>();

        // Loop through every users UID
        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            RankingEntry newEntry = new RankingEntry(
                childSnapshot.Child("username").Value.ToString(),
                int.Parse(childSnapshot.Child("score").Value.ToString()),
                int.Parse(childSnapshot.Child("kills").Value.ToString()),
                int.Parse(childSnapshot.Child("killsScore").Value.ToString()),
                long.Parse(childSnapshot.Child("time").Value.ToString()),
                int.Parse(childSnapshot.Child("timeScore").Value.ToString())
            );

            scoreboardEntries.Add(newEntry);
        }

        return scoreboardEntries;
    }
}

public class RankingEntry
{
    public string username;
    public int score;
    public int kills;
    public int killsScore;
    public long time;
    public int timeScore;

    public RankingEntry(string username, int score, int kills, int killsScore, long time, int timeScore)
    {
        this.username = username;
        this.score = score;
        this.kills = kills;
        this.killsScore = killsScore;
        this.time = time;
        this.timeScore = timeScore;
    }
}

public class User {
    public string email;
    public string username;
    public string password;

    public User(string email, string username, string password) {
        this.email = email;
        this.username = username;
        this.password = password;
    }
}

public class LoggedUser
{
    public string id;
    public string email;
    public string username;

    public LoggedUser() {
    }

    public LoggedUser(string id, string email, string username) {
        this.id = id;
        this.email = email;
        this.username = username;
    }
}
