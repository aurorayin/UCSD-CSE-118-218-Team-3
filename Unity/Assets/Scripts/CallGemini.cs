using System.Collections;
using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.Networking;
using System.Collections.Generic;

public class CallGemini : MonoBehaviour
{
    [SerializeField] private TMP_Text transcriptionText; // Reference to the TextMeshPro component
    [SerializeField] private TMP_Text responseText; // Reference to the TextMeshPro component
    [SerializeField] private TMP_Text buttonStatus; // Reference to the activate/deactivate button

    private string gasURL = "https://script.google.com/macros/s/AKfycbzKM66sePA1NH_c8D0w14sGARjxM6ZRC94yatdIoK9ZNml-_LBx2ALtxuoVXLxtI0K4rw/exec";
    //[SerializeField] private string gasURL = "https://script.google.com/macros/s/AKfycbwxXMvkCBBWyvgjbt5iUWzr5jqVvvQvOH3e4qHgC8CJR2bmcJpjAMXXkFRjpNrCYY7J7g/exec";
    private string prompt = "We are building an hearing assistant application, providing summarizations for real-time transcription of conversations, in order to help the elderly to be able to have clear and meaningful conversations with other people. Based on this, summarize the following dialogue into a simple, clear and short one-sentence or two-sentence memo: ";
    private string lastProcessedTranscription = ""; // Last processed transcription
    private List<string> unprocessedTranscriptions = new List<string>(); // Store unprocessed transcriptions

    private bool isCurrentlyActivated = true; // Tracks the current button state

    private void Update()
    {
        // Determine the current state based on the button's displayed text
        // If button shows "Deactivate," it means the current state is "Activated"
        bool newActivationState = buttonStatus != null && buttonStatus.text == "Deactivate";

        // If state has changed
        if (newActivationState != isCurrentlyActivated)
        {
            if (!newActivationState) // if state has changed to Deactivate
            {
                // Process all unprocessed transcriptions
                if (unprocessedTranscriptions.Count > 0)
                {
                    StartCoroutine(SendBatchToGAS(unprocessedTranscriptions));
                    unprocessedTranscriptions.Clear(); // Clear the list after processing
                }
            }
            //transcriptionText.text += "\nstate changed\n";

            // Update current activation state
            isCurrentlyActivated = newActivationState;
        }

        // If currently activated, collect transcriptions
        if (isCurrentlyActivated)
        {
            string currentTranscription = transcriptionText.text;

            if (!string.IsNullOrEmpty(currentTranscription) && currentTranscription != lastProcessedTranscription)
            {
                lastProcessedTranscription = currentTranscription;
                unprocessedTranscriptions.Add(currentTranscription); // Add to the list of unprocessed transcriptions
            }
            //transcriptionText.text += "\nis currently activated\n";
        }
    }


    // Send a batch of transcriptions to Gemini
    private IEnumerator SendBatchToGAS(List<string> transcriptions)
    {
        // Combine all transcriptions into one string
        string combinedTranscriptions = string.Join("\n", transcriptions);
        
        prompt += combinedTranscriptions;
        // Create a form and add the transcription as a parameter
        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);

        UnityWebRequest www = UnityWebRequest.Post(gasURL, form);

        yield return www.SendWebRequest();

        string response;

        if (www.result == UnityWebRequest.Result.Success)
        {
            response = www.downloadHandler.text;
        }
        else
        {
            response = "There was an error!";
        }
        transcriptionText.text += "\n;;Gemini responded:\n";
        transcriptionText.text += response;
        transcriptionText.text += ";;";
        Debug.Log(response);
    }
}