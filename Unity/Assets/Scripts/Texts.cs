using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Texts : MonoBehaviour
{
    private DatabaseReference databaseReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void SendMessage(string messageId, string messageText)
    {
        Message message = new Message(messageId, messageText);
        string json = JsonUtility.ToJson(message);

        if (databaseReference != null)
        {
            databaseReference.Child("messages").Push().SetRawJsonValueAsync(json);
        }
        else
        {
            Debug.LogError("Database reference is not initialized.");
        }
    }

    private class Message
    {
        public string messageId;
        public string messageText;
        public long timestamp;

        public Message(string messageId, string messageText)
        {
            this.messageId = messageId;
            this.messageText = messageText;
            this.timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    public void SendMessagesFromFile(string filePath)
    {
        string jsonContent = System.IO.File.ReadAllText(filePath);
        List<Message> messages = JsonUtility.FromJson<MessagesList>(jsonContent).messages;

        foreach (Message message in messages)
        {
            string json = JsonUtility.ToJson(message);
            databaseReference.Child("messages").Push().SetRawJsonValueAsync(json);
        }
    }

    public void OnSendMessagesButtonPressed()
    {
        string filePath = Application.dataPath + "/Data/texts.json";
        SendMessagesFromFile(filePath);
    }

    [System.Serializable]
    private class MessagesList
    {
        public List<Message> messages;
    }
}