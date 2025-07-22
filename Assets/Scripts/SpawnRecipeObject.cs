using System.Collections;
using System.Collections.Generic;
using RecipeApi;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpawnRecipeObject : MonoBehaviour
{
    [SerializeField] private RecipeDataManager _recipeResponse;
    private void OnEnable()
    {
        RecipeSearch.OnRecipeSearchCompleted += OnRecipeSearchComplete;
    }

    private void OnRecipeSearchComplete()
    {
        _recipeResponse = ReferenceManager.Instance.recipeSearch.recipeResponse;
        StartCoroutine(SpawnRecipeCoroutine());
    }

    private IEnumerator SpawnRecipeCoroutine()
    {
        List<UnityWebRequestAsyncOperation> downloadOperations = new List<UnityWebRequestAsyncOperation>();

        for (int index = 0; index < _recipeResponse.hits.Count; index++)
        {
            Hit hit = _recipeResponse.hits[index];
            PopulateRecipeData populateRecipeData;
            if (ReferenceManager.Instance.recipeParent.childCount <= index)
            {
                // Instantiate the recipe object
                GameObject recipeObj = Instantiate(ReferenceManager.Instance.recipePf, ReferenceManager.Instance.recipeParent);
                populateRecipeData = recipeObj.GetComponent<PopulateRecipeData>();
            }
            else
            {
                GameObject recipeObj = ReferenceManager.Instance.recipeParent.GetChild(index).gameObject;
                populateRecipeData = recipeObj.GetComponent<PopulateRecipeData>();
            }

            // Start the download and add the operation to the list
            UnityWebRequestAsyncOperation operation = StartDownloadImage(hit.recipe, populateRecipeData);
            downloadOperations.Add(operation);
        }
        if (_recipeResponse.hits.Count < ReferenceManager.Instance.recipeParent.childCount)
        {
            for (int index = ReferenceManager.Instance.recipeParent.childCount - 1; index > ReferenceManager.Instance.recipeParent.childCount - (ReferenceManager.Instance.recipeParent.childCount - _recipeResponse.hits.Count); index--)
            {
                Destroy(ReferenceManager.Instance.recipeParent.GetChild(index).gameObject);
            }
        }

        // Wait for all download operations to complete
        foreach (UnityWebRequestAsyncOperation operation in downloadOperations)
        {
            yield return operation;
        }

        // Call the callback after all recipes are spawned and populated
        OnAllRecipesSpawned();
    }

    private UnityWebRequestAsyncOperation StartDownloadImage(Recipe recipe, PopulateRecipeData populateRecipeData)
    {
        // Create a UnityWebRequest to download the image
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(recipe.image);
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        // Attach a callback to handle the response
        operation.completed += (asyncOp) =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + request.error);
            }
            else
            {
                // Get the downloaded texture
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                // Create a new sprite from the texture
                recipe.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Populate recipe data after the image has downloaded
                populateRecipeData.Populate(recipe);
            }
        };

        return operation;
    }

    private void OnAllRecipesSpawned()
    {
        ReferenceManager.Instance.uiManager.OnAllRecipeDataFound();
        ReferenceManager.Instance.scrollRect.verticalNormalizedPosition = 1f;
    }

    private void OnDisable()
    {
        RecipeSearch.OnRecipeSearchCompleted -= OnRecipeSearchComplete;
    }
}
