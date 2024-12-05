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

    [SerializeField] private string gasURL = "https://script.google.com/macros/s/AKfycbzrpd2CBP82_H_lDMQLxmF-VLqK5mzLgSj-3OcsUhMvapkDQ6Vfu_HGcSR16Y4ZXNuvXQ/exec";

    [SerializeField] private float checkInterval = 1.0f; // How often to check for updates

    //private void Start()
    //{
    //    StartCoroutine(CheckForUpdates());
    //}
    private string lastProcessedTranscription = ""; // Last processed transcription
    private List<string> unprocessedTranscriptions = new List<string>(); // Store unprocessed transcriptions

    private bool isCurrentlyActivated = true; // Tracks the current button state

    private void Update()
    {
        // Determine the current state based on the button's displayed text
        // If button shows "Deactivate," it means the current state is "Activated"
        bool newActivationState = buttonStatus != null && buttonStatus.text == "Deactivate";

        //transcriptionText.text += buttonStatus.text;
        //transcriptionText.text += newActivationState;
        //transcriptionText.text += isCurrentlyActivated;
        //transcriptionText.text += "\n";
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

    //private IEnumerator CheckForUpdates()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(checkInterval);

    //        // Get the current transcription from the TextMeshPro component
    //        string currentTranscription = transcriptionText.text;
    //        string[] sections = currentTranscription.Split(new[] { ";;" }, System.StringSplitOptions.None);

    //        if (sections.Length > 0)
    //        {
    //            string lastSection = sections[sections.Length - 1].Trim();

    //            // Ensure the last section is complete (not empty) and different from the last processed sentence
    //            if (!string.IsNullOrWhiteSpace(lastSection) && lastSection != lastCompleteSentence)
    //            {
    //                lastCompleteSentence = lastSection; // Update the last complete sentence
    //                StartCoroutine(SendDataToGAS(lastCompleteSentence)); // Call Gemini with the new sentence
    //            }
    //        }
    //    }
    //}

    // Send a batch of transcriptions to Gemini
    private IEnumerator SendBatchToGAS(List<string> transcriptions)
    {
        // Combine all transcriptions into one string
        string combinedTranscriptions = string.Join("\n", transcriptions);
        string prompt = "Summarize the following text: " + combinedTranscriptions;
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
