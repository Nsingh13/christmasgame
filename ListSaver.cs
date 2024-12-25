using System.Collections.Generic;
using UnityEngine;

public class ListSaver : MonoBehaviour
{
    [System.Serializable]
    public class ListWrapper<T>
    {
        public List<T> list;

        public ListWrapper(List<T> list)
        {
            this.list = list;
        }
    }

    // Save the list to PlayerPrefs
    public void SaveList(List<string> myList, string key)
    {
        string json = JsonUtility.ToJson(new ListWrapper<string>(myList)); // Convert list to JSON
        PlayerPrefs.SetString(key, json); // Save JSON string in PlayerPrefs
        PlayerPrefs.Save(); // Ensure it's written to disk
        Debug.Log($"List saved under key: {key}");
    }

    // Load the list from PlayerPrefs
    public List<string> LoadList(string key)
    {
        if (PlayerPrefs.HasKey(key)) // Check if key exists
        {
            string json = PlayerPrefs.GetString(key); // Retrieve JSON string
            return JsonUtility.FromJson<ListWrapper<string>>(json).list; // Convert JSON back to list
        }

        Debug.LogWarning($"No list found under key: {key}");
        return new List<string>(); // Return an empty list if key doesn't exist
    }

    // Example usage
    private void Start()
    {

    }
}
