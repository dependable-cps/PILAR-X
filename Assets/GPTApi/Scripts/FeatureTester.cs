using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FeatureTester : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI responseTimeText;

    [SerializeField]
    private Button backButton;

    private float minWaitTime = 8f;
    private float maxWaitTime = 10f;

    [SerializeField]
    private ChatGPTQuestion chatGPTQuestion;

    private string gptPrompt;

    [SerializeField]
    private TextMeshProUGUI scenarioTitleText;


    [SerializeField]
    private ChatGPTResponse lastChatGPTResponseCache;

    public string ChatGPTMessage
    {
        get
        {
            return (lastChatGPTResponseCache.Choices.FirstOrDefault()?.Message?.content ?? null) ?? string.Empty;
        }
    }


    private void Awake()
    {
        responseTimeText.text = string.Empty;
        backButton.onClick.AddListener(() =>
        {
            FeatureLogger.Instance.Clear();
            responseTimeText.text = string.Empty;
        });
    }

    public void Execute()
    {
        // backButton.interactable = false;
        gptPrompt = $"{chatGPTQuestion.promptPrefixConstant} {chatGPTQuestion.promptTraditional}";

        scenarioTitleText.text = chatGPTQuestion.scenarioTitle;


        FeatureProgress.Instance.StartProgress("Generating Explanation");

        // handle replacements
        Array.ForEach(chatGPTQuestion.replacements, r =>
        {
            gptPrompt = gptPrompt.Replace("{" + $"{r.replacementType}" + "}", r.value);
        });

        // handle reminders
        if (chatGPTQuestion.reminders.Length > 0)
        {
            gptPrompt += $", {string.Join(',', chatGPTQuestion.reminders)}";
        }

        StartCoroutine(ChatGPTClient.Instance.Ask(gptPrompt, (response) =>
        {
            // backButton.interactable = true;
            lastChatGPTResponseCache = response;
            StartCoroutine(Result((float)response.ResponseTotalTime));

        }));
    }
    IEnumerator Result(float takenTime)
    {
        float time = Mathf.Abs(Random.Range(minWaitTime, maxWaitTime) - takenTime / 1000);
        yield return new WaitForSeconds(time);
        responseTimeText.text = $"Time: {Math.Round(time * 1000)} ms";

        FeatureProgress.Instance.StopProgress();

        FeatureLogger.Instance.LogInfo(ChatGPTMessage);

    }


}
