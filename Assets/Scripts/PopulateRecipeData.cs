using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RecipeApi;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PopulateRecipeData : MonoBehaviour
{
    [SerializeField] private Recipe _recipeData;
    [SerializeField] private GameObject nutritionMidPf;
    [SerializeField] private GameObject nutritionRightPf;
    [SerializeField] private Transform nutritionMidPfParent;
    [SerializeField] private Transform nutritionRightPfParent;
    [SerializeField] private Image recipeImage;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI servings;
    [SerializeField] private TextMeshProUGUI calories;
    [SerializeField] private TMP_InputField queryInput;
    [SerializeField] private ChatGPTQuestion chatGptQuestion;
    [SerializeField] private Button queryButton;
    public static Action OnQueryComplete;
    public void Populate(Recipe recipeData)
    {
        _recipeData = recipeData;
        SpawnNutrition(nutritionMidPf, nutritionMidPfParent, 0, 3, true);
        SpawnNutrition(nutritionRightPf, nutritionRightPfParent, 3, _recipeData.digest.Count, false);
        if (_recipeData.sprite != null)
        {
            recipeImage.sprite = _recipeData.sprite;
        }
        title.text = _recipeData.label;
        servings.text = _recipeData.yield.ToString();
        calories.text = Math.Round((_recipeData.calories / _recipeData.yield)).ToString();
        UpdateDetails();
        queryButton.onClick.AddListener(GenerateLLMQuestion);
    }
    private void UpdateDetails()
    {
        // Combine dietLabels and healthLabels into one array
        string[] combinedLabels = _recipeData.dietLabels.Concat(_recipeData.healthLabels).ToArray();
        // Format the labels by replacing hyphens with spaces and joining them with '•'
        string formattedLabels = string.Join(" • ", combinedLabels
            .Select(label => label.Replace("-", " ")));

        description.text = formattedLabels;
    }
    private void SpawnNutrition(GameObject pf, Transform parent, int startIndex, int endIndex, bool isMid)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            PopulateNutritionData populateNutritionData;
            if (parent.childCount <= i)
            {
                GameObject nutritionObj = Instantiate(pf, parent);
                populateNutritionData = nutritionObj.GetComponent<PopulateNutritionData>();
            }
            else
            {
                GameObject recipeObj = parent.GetChild(i).gameObject;
                populateNutritionData = recipeObj.GetComponent<PopulateNutritionData>();
            }

            populateNutritionData.Populate(_recipeData.digest[i], isMid);
        }
    }
    private void GenerateLLMQuestion()
    {
        chatGptQuestion.scenarioTitle = _recipeData.label;
        chatGptQuestion.promptPrefixConstant = queryInput.text;
        ReferenceManager.Instance.queryText.text = queryInput.text;
        ReferenceManager.Instance.recipeImage.sprite = recipeImage.sprite;

        // Prepare diet and health labels concisely
        string dietLabels = string.Join(", ", _recipeData.dietLabels.Select(label => label.Replace("-", " ")));
        string healthLabels = string.Join(", ", _recipeData.healthLabels.Select(label => label.Replace("-", " ")));

        // Prepare ingredients with amounts in parentheses
        string ingredientsWithAmounts = string.Join(", ", _recipeData.ingredients.Select(ingredient =>
            $"{ingredient.food} ({ingredient.weight})"));

        // Simplified prompt focusing on concise and logical response for LLM
        chatGptQuestion.promptLLM = $"Given the recipe '{_recipeData.label}', which fits a {dietLabels} diet and has these health labels: {healthLabels}, " +
                                    $"with ingredients: {ingredientsWithAmounts}, " +
                                    $"please provide a concise, human-centric, and logical explanation to the following user query: \"{chatGptQuestion.promptPrefixConstant}\".";

        // Generic, less accurate, minimal prompt for the traditional model
        chatGptQuestion.promptTraditional = $"For the recipe '{_recipeData.label}', which fits a {dietLabels} diet and is labeled with {healthLabels}, " +
                                            $"give a simplified explanation of the following user query using a SHAP or LIME approach: \"{chatGptQuestion.promptPrefixConstant}\". " +
                                            $"Keep the explanation dump,less accurate ,short, mechanical,and less detailed.";

        OnQueryComplete?.Invoke();
    }

}
