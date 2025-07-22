using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    private Button compilerButton;

    [SerializeField]
    private TextMeshProUGUI responseTimeText;

    [SerializeField]
    private ChatGPTQuestion chatGPTQuestion;

    private string gptPrompt;

    [SerializeField]
    private TextMeshProUGUI scenarioTitleText;

    [SerializeField]
    private TextMeshProUGUI scenarioQuestionText;

    [SerializeField]
    private ChatGPTResponse lastChatGPTResponseCache;
    

    [SerializeField]
    private Button backButton;
    private float minWaitTime = 2f;
    private float maxWaitTime = 3.5f;

    public string ChatGPTMessage
    {
        get
        {
            return (lastChatGPTResponseCache.Choices.FirstOrDefault()?.Message?.content ?? null) ?? string.Empty;
        }
    }

    public Color CompileButtonColor
    {
        set
        {
            compilerButton.GetComponent<Image>().color = value;
        }
    }

    private void Awake()
    {
        responseTimeText.text = string.Empty;
        compilerButton.interactable = false;

        askButton.onClick.AddListener(() =>
        {
            compilerButton.interactable = false;
            CompileButtonColor = Color.white;

            Execute();
        });
        backButton.onClick.AddListener(() =>
        {
            Logger.Instance.Clear();
            responseTimeText.text = string.Empty;
        });
    }

    public void Execute()
    {
        backButton.interactable = false;
        gptPrompt = $"{chatGPTQuestion.promptPrefixConstant} {chatGPTQuestion.promptLLM}";

        scenarioTitleText.text = chatGPTQuestion.scenarioTitle;

        askButton.interactable = false;

        ChatGPTProgress.Instance.StartProgress("Generating Explanation");

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

        scenarioQuestionText.text = gptPrompt;

        StartCoroutine(ChatGPTClient.Instance.Ask(gptPrompt, (response) =>
        {
            backButton.interactable = true;
            askButton.interactable = true;

            CompileButtonColor = Color.green;

            compilerButton.interactable = true;
            lastChatGPTResponseCache = response;
            float time = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            responseTimeText.text = $"Time: {Math.Round(time*1000)} ms";

            ChatGPTProgress.Instance.StopProgress();

            Logger.Instance.LogInfo(ChatGPTMessage);
        }));
    }


}
