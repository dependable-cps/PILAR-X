using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using RecipeApi;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;


public class RecipeSearch : MonoBehaviour
{
    private string query;
    public string appId = "a2bcf307";
    public string appKey = "0fcc6f856893179ebf25b8478d6fb7bb";
    public RecipeDataManager recipeResponse;
    public static Action OnRecipeSearchCompleted;
    private string url;
    private int maxIngredients = 4;
    public void OnSearchRecipe(string ingredients)
    {
        if (ingredients != null) query = ingredients;
        // Limit the number of ingredients in the query to avoid no results
        string[] ingredientArray = query.Split(',');
        // Limit to a reasonable number
        // Split the ingredients string into an array

        // Create a random object
        Random random = new Random();
        // Randomly shuffle and take 'maxIngredients' elements
        string[] randomIngredients = ingredientArray.OrderBy(x => random.Next()).Take(maxIngredients).ToArray();

        // Join them with a space for the API query
        string limitedIngredients = string.Join(" ", randomIngredients);

        // Get user-selected diet and health labels
        string dietLabel = ReferenceManager.Instance.dietDropdown.options[ReferenceManager.Instance.dietDropdown.value].text.ToLower();
        string healthLabel = ReferenceManager.Instance.healthDropdown.options[ReferenceManager.Instance.healthDropdown.value].text.ToLower();

        // Get calorie range values
        string minCalories = ReferenceManager.Instance.doubleSlider._sliderMin.Value.ToString();
        string maxCalories = ReferenceManager.Instance.doubleSlider._sliderMax.Value.ToString();
        string calories = string.Empty;

        // URL-encode the ingredients and labels to handle spaces and special characters
        string encodedQuery = UnityWebRequest.EscapeURL(limitedIngredients.Replace(",", " ")); // Only use the limited set of ingredients
        string encodedDietLabel = !string.IsNullOrEmpty(dietLabel) && dietLabel != "any" ? UnityWebRequest.EscapeURL(dietLabel) : string.Empty;
        string encodedHealthLabel = !string.IsNullOrEmpty(healthLabel) && healthLabel != "any" ? UnityWebRequest.EscapeURL(healthLabel) : string.Empty;

        // Construct the calories range string
        if (!string.IsNullOrEmpty(minCalories) && !string.IsNullOrEmpty(maxCalories))
        {
            calories = $"{minCalories}-{maxCalories}"; // Correct format for the API
        }

        // Construct the API URL
        url = $"https://api.edamam.com/search?q={encodedQuery}&app_id={appId}&app_key={appKey}";

        // Add diet label if applicable
        if (!string.IsNullOrEmpty(encodedDietLabel))
        {
            url += $"&diet={encodedDietLabel}";
        }

        // Add health label if applicable
        if (!string.IsNullOrEmpty(encodedHealthLabel))
        {
            url += $"&health={encodedHealthLabel}";
        }

        // Add calorie filter if both min and max values are provided
        if (!string.IsNullOrEmpty(calories))
        {
            url += $"&calories={calories}";
        }

        // Start the coroutine to get recipes from the API
        StartCoroutine(GetRecipes());
    }




    IEnumerator GetRecipes()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.error);
            OnSearchRecipe(null);
        }
        else
        {
            // You can then parse the JSON response and update the UI accordingly
            recipeResponse = JsonConvert.DeserializeObject<RecipeDataManager>(request.downloadHandler.text);
            if (recipeResponse.count == 0)
            {
                OnSearchRecipe(null);
            }
            else
            {
                OnRecipeSearchCompleted?.Invoke();
            }
           
        }
    }

}
