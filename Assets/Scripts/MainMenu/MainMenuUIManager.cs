using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System;

public class MainMenuUIManager : MonoBehaviour
{
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
    [SerializeField] private TMP_Text warningRegisterText;

    [Header("LoggedIn Menu")]
    [SerializeField] private TMP_Text loggedInMenuTitle;

    [Header("Options Menu")]
    [SerializeField] private GameObject optionsMenu;

    [Header("Game Title")]
    [SerializeField] private GameObject gameTitle;

    void Start()
    {
        if (DatabaseManager.instance.CurrentUser == null)
            SetActiveUserState(false);
        else
            SetActiveUserState(true);
    }

    private void ToggleUserMenu(bool isActive)
    {
        GameObject menu;
        if (DatabaseManager.instance.CurrentUser == null)
            menu = loginMenu;
        else
            menu = loggedInMenu;
        menu.SetActive(isActive);
        gameTitle.SetActive(!isActive);
    }

    private void SetActiveUserState(bool state)
    {
        clickToLoginText.gameObject.SetActive(!state);
        if (state)
        {
            string username = DatabaseManager.instance.CurrentUser.username;
            usernameText.text = username;
            usernameText.verticalAlignment = VerticalAlignmentOptions.Middle;
            loggedInMenuTitle.text = "¡Hola " + username + "!";
        }
        else
        {
            usernameText.text = "Invitado";
            usernameText.verticalAlignment = VerticalAlignmentOptions.Top;
        }
    }

    public async Task LogInEffect(string username)
    {
        warningLoginText.text = "¡Bienvenido " + username + "!";
        warningLoginText.color = Color.green;
        warningLoginText.gameObject.SetActive(true);

        await Task.Delay(1500);

        Debug.Log(warningLoginText.text);

        HideLoginWarningText();

        loginMenu.SetActive(false);
        gameTitle.SetActive(true);

        SetActiveUserState(true);
    }

    public async Task AutoLogInEffect(string username)
    {
        warningRegisterText.text = "¡Bienvenido " + username + "!";
        warningRegisterText.color = Color.green;
        warningRegisterText.gameObject.SetActive(true);

        await Task.Delay(1500);

        HideRegisterWarningText();

        registerMenu.SetActive(false);
        gameTitle.SetActive(true);
        loggedInMenuTitle.text = "¡Hola " + username + "!";

        SetActiveUserState(true);
    }

    public void LogOutEffect()
    {
        SetActiveUserState(false);
        loggedInMenu.SetActive(false);
        gameTitle.SetActive(true);
    }

    public void SetRegisterWarningText(string text)
    {
        warningRegisterText.text = text;
        warningRegisterText.color = Color.red;
        warningRegisterText.gameObject.SetActive(true);
    }

    public void SetLoginWarningText(string text)
    {
        warningLoginText.text = text;
        warningLoginText.color = Color.red;
        warningLoginText.gameObject.SetActive(true);
    }

    public void HideLoginWarningText()
    {
        warningLoginText.text = "";
        warningLoginText.color = Color.red;
        warningLoginText.gameObject.SetActive(false);
    }

    public void HideRegisterWarningText()
    {
        warningRegisterText.text = "";
        warningRegisterText.color = Color.red;
        warningRegisterText.gameObject.SetActive(false);
    }

    public void OpenUserMenu() => ToggleUserMenu(true);

    public void CloseUserMenu() => ToggleUserMenu(false);

    public void OpenOptionsMenu()
    {
        gameTitle.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        gameTitle.SetActive(true);
    }

    public void SwitchLoginWithRegisterMenu()
    {
        registerMenu.SetActive(true);
        loginMenu.SetActive(false);
    }

    public void SwitchRegisterWithLoginMenu()
    {
        loginMenu.SetActive(true);
        registerMenu.SetActive(false);
    }

    public void CloseRegisterMenu()
    {
        registerMenu.SetActive(false);
        gameTitle.SetActive(true);
    }

    public async void Register()
    {
        Task registerUserTask = DatabaseManager.instance.CreateUser(emailRegisterField.text, usernameRegisterField.text, passwordRegisterField.text, passwordRegisterField.text);
        try {
            await registerUserTask;
        } catch (Exception e) {
            SetRegisterWarningText(e.Message);
            return;
        }

        await DatabaseManager.instance.LoginUser(emailRegisterField.text, passwordRegisterField.text);

        await AutoLogInEffect(DatabaseManager.instance.CurrentUser.username);
    }

    public async void LogIn()
    {
        Task loginUserTask = DatabaseManager.instance.LoginUser(emailLoginField.text, passwordLoginField.text);

        try {
            await loginUserTask;
        } catch (Exception e) {
            SetLoginWarningText(e.Message);
            return;
        }
            
        await LogInEffect(DatabaseManager.instance.CurrentUser.username);
    }

    public void LogOut()
    {
        DatabaseManager.instance.LogOutUser();

        LogOutEffect();
    }

}
