using System.Collections;
using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.Networking;

public class CallGemini : MonoBehaviour
{
    [SerializeField] private TMP_Text transcriptionText; // Reference to the TextMeshPro component
    [SerializeField] private TMP_Text responseText; // Reference to the TextMeshPro component
    [SerializeField] private string gasURL = "https://script.google.com/macros/s/AKfycbzrpd2CBP82_H_lDMQLxmF-VLqK5mzLgSj-3OcsUhMvapkDQ6Vfu_HGcSR16Y4ZXNuvXQ/exec";

    [SerializeField] private float checkInterval = 1.0f; // How often to check for updates
    private string lastCompleteSentence = "";

    private void Start()
    {
        StartCoroutine(CheckForUpdates());
    }

    private IEnumerator CheckForUpdates()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            // Get the current transcription from the TextMeshPro component
            string currentTranscription = transcriptionText.text;
            string[] sections = currentTranscription.Split(new[] { ";;" }, System.StringSplitOptions.None);

            if (sections.Length > 0)
            {
                string lastSection = sections[sections.Length - 1].Trim();
        
                // Ensure the last section is complete (not empty) and different from the last processed sentence
                if (!string.IsNullOrWhiteSpace(lastSection) && lastSection != lastCompleteSentence)
                {
                    lastCompleteSentence = lastSection; // Update the last complete sentence
                    StartCoroutine(SendDataToGAS(lastCompleteSentence)); // Call Gemini with the new sentence
                }
            }
        }
    }

    private IEnumerator SendDataToGAS(string transcription)
    {
        // Create a form and add the transcription as a parameter
        WWWForm form = new WWWForm();
        string prompt = "Summarize the following sentence: " + transcription;
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
