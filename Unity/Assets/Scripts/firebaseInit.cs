using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
}