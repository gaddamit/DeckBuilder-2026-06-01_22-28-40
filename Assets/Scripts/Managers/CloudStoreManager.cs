using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

[System.Serializable]
public class DeckContainer
{
    public List<string> cards; // The inner array
}

[System.Serializable]
public class UserProfile
{
    public string user_id;
    public List<DeckContainer> decks; // Maps directly to your JSON "decks" array
}

public class CloudStoreManager : MonoBehaviour
{
    [SerializeField] private const string _url = "https://api.jsonbin.io/v3/b/";
    [SerializeField] private CloudStore _cloudStore;

    void Start()
    {
        
    }

    public string CreateUserPayload(string userID)
    {
        string jsonPayload = "{\n" +
                          "    \"user_id\": \"PLAYER_ID\",\n" +
                          "    \"decks\": []\n" +
                          "}";
        jsonPayload = jsonPayload.Replace("PLAYER_ID", userID);
        return jsonPayload;
    }
    
    // Used when creating a new user, replaces the User Profile on the cloud
    public IEnumerator CreateBinData(string userID, Action<bool> onComplete)
    {
        string url = _url + $"{_cloudStore.BinID}";
        string jsonPayload = CreateUserPayload(userID);
        byte[] raw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(raw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("X-Master-Key", _cloudStore.MasterKey);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("X-Bin-Versioning", "false");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Created user");
                onComplete?.Invoke(true);
            }
            else
            {
                Debug.Log($"Error: {webRequest.error}");
                onComplete?.Invoke(false);
            }
        }
    }

    // Fetches the User Profile on the cloud store
    public IEnumerator GetBinData(Action<UserProfile> onComplete)
    {
        string url = _url + $"{_cloudStore.BinID}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("X-Master-Key", _cloudStore.MasterKey);
            webRequest.SetRequestHeader("X-Bin-Meta", "false");

            yield return webRequest.SendWebRequest();

            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log($"Result:{jsonResult}");
                UserProfile profile = JsonUtility.FromJson<UserProfile>(jsonResult);
                onComplete?.Invoke(profile);
            }
            else
            {
                Debug.LogError($"JSON bin download failed:{webRequest.error}");
                onComplete?.Invoke(null);
            }

        }
    }

    // Used for updating the User's decks
    public IEnumerator UpdateBinData(List<string> cardIDs, Action<bool> onComplete) 
    {
        string url = _url + $"{_cloudStore.BinID}";

        // Create a request to get the current JSON value of User Profile
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("X-Master-Key", _cloudStore.MasterKey);
            webRequest.SetRequestHeader("X-Bin-Meta", "false");

            yield return webRequest.SendWebRequest();

            if(webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error: {webRequest.error}");
                onComplete?.Invoke(false);
            }
            
            // Convert result to User Profile Serializable and append new deck
            string jsonResult = webRequest.downloadHandler.text;
            UserProfile profile = JsonUtility.FromJson<UserProfile>(jsonResult);
            DeckContainer deck = new DeckContainer();
            deck.cards = cardIDs;
            profile.decks.Add(deck);

            string jsonPayload = JsonUtility.ToJson(profile);
            byte[] raw = Encoding.UTF8.GetBytes(jsonPayload);

            // Send updated User Profile to cloud store
            using (UnityWebRequest putRequest = new UnityWebRequest(url, "PUT"))
            {
                putRequest.uploadHandler = new UploadHandlerRaw(raw);
                putRequest.downloadHandler = new DownloadHandlerBuffer();

                putRequest.SetRequestHeader("X-Master-Key", _cloudStore.MasterKey);
                putRequest.SetRequestHeader("Content-Type", "application/json");
                putRequest.SetRequestHeader("X-Bin-Versioning", "false");

                yield return putRequest.SendWebRequest();

                if (putRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Update user profile");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.Log($"Error: {putRequest.error}");
                    onComplete?.Invoke(false);
                }
            }

        }        
    }
}
