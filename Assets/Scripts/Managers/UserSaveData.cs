using System;
using UnityEngine;

public static class UserSaveData
{
    private static string _userID = "";
    public static string UserID => _userID;
    public static bool HasPreviousUser()
    {
        return PlayerPrefs.HasKey("user_id");
    }

    public static void RemoveUser()
    {
        if(HasPreviousUser())
        {
            try
            {
                PlayerPrefs.DeleteKey("user_id");
                PlayerPrefs.Save();
                Debug.Log($"User delete: {_userID}");
            }
            catch (PlayerPrefsException e)
            {
                Debug.Log($"Failed to delete user: {_userID}");
            }
        }
    }

    public static string? CreateUser()
    {
       string newID = Guid.NewGuid().ToString();

        try
        {
            PlayerPrefs.SetString("user_id", newID);
            PlayerPrefs.Save();
            _userID = newID;
            Debug.Log($"New user ID: {newID}");
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError($"Failed to create user: {e.Message}");
            return string.Empty;
        }
        return newID;
    }

    public static void LoadPreviousUser()
    {
        _userID = PlayerPrefs.GetString("user_id");
        Debug.Log($"New user ID: {_userID}");
    }
}
