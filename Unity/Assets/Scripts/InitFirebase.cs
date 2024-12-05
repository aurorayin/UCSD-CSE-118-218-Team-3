using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

public class InitFirebase : MonoBehaviour
{   
    private Firebase.FirebaseApp app;
    private DatabaseReference mDatabaseRef;
    private bool firebaseReady = false;

    private bool hasWritten = false;
    void Awake()
    {   
        Debug.Log("Initializing Firebase");

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                Debug.Log("Firebase is ready to use");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                firebaseReady = true;
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    // Start is called before the first frame update
    void Start()
    {    
    }

    // Update is called once per frame
    void Update()
    {
        if (firebaseReady && !hasWritten) {
            mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            writeNewMemo(3, "Hello, Firebase!");
            hasWritten = true;
        }
    }

    private void writeNewMemo(int id, string content) {
        string json = JsonUtility.ToJson(content);
        Debug.Log("Writing to Firebase: " + content);

        mDatabaseRef.Child("memos").Child(id.ToString()).SetValueAsync(content);
    }
}