using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [Header("User data")]
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text clickToLoginText;

    [Header("Menus")]
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject registerMenu;
    [SerializeField] private GameObject loggedInMenu;

    [Header("Login Menu")]
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private TMP_Text warningLoginText;

    [Header("Register Menu")]
    [SerializeField] private TMP_InputField usernameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterVerifyField;
    [SerializeField] private TMP_Text warningRegisterText;

    [Header("LoggedIn Menu")]
    [SerializeField] private TMP_Text loggedInMenuTitle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleUserMenu(bool isActive)
    {
        GameObject menu;
        if (DatabaseManager.instance.CurrentUser == null)
            menu = loginMenu;
        else
            menu = loggedInMenu;
        menu.SetActive(isActive);
    }

    private void SetActiveUserState(bool state)
    {
        clickToLoginText.gameObject.SetActive(!state);
        if (state)
        {
            usernameText.text = DatabaseManager.instance.CurrentUser.username;
            usernameText.verticalAlignment = VerticalAlignmentOptions.Middle;
        }
        else
        {
            usernameText.text = "Invitado";
            usernameText.verticalAlignment = VerticalAlignmentOptions.Top;
        }
    }

    public IEnumerator LogInEffect(string username)
    {
        warningLoginText.text = "¡Bienvenido " + username + "!";
        warningLoginText.color = Color.green;
        warningLoginText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        HideWarningText();

        loginMenu.SetActive(false);
        loggedInMenuTitle.text = "¡Hola " + username + "!";

        SetActiveUserState(true);
    }

    public void LogOutEffect()
    {
        SetActiveUserState(false);
        loggedInMenu.SetActive(false);
    }

    public void SetWarningText(string text)
    {
        warningLoginText.text = text;
        warningLoginText.color = Color.red;
        warningLoginText.gameObject.SetActive(true);
    }

    public void HideWarningText()
    {
        warningLoginText.text = "";
        warningLoginText.color = Color.red;
        warningLoginText.gameObject.SetActive(false);
    }

    public void OpenUserMenu() => ToggleUserMenu(true);

    public void CloseUserMenu() => ToggleUserMenu(false);

    public void Register() => StartCoroutine(DatabaseManager.instance.CreateUser(emailRegisterField.text, usernameRegisterField.text, passwordRegisterField.text, passwordRegisterVerifyField.text));

    public void LogIn() => StartCoroutine(DatabaseManager.instance.LoginUser(emailLoginField.text, passwordLoginField.text));

    public void LogOut() => StartCoroutine(DatabaseManager.instance.LogOutUser());
}
