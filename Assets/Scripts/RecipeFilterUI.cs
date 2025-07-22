using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RecipeFilterUI : MonoBehaviour
{
    // List of diet and health labels
   [SerializeField] private List<string> dietLabels = new List<string>()
    {
        "Any",
        "Balanced",
        "High-Protein",
        "Low-Carb",
        "Low-Fat",
        "High-Fiber"
    };
   
   [SerializeField] List<string> healthLabels = new List<string>()
    {
        "Any",
        "Vegan",
        "Vegetarian",
        "Gluten-Free",
        "Dairy-Free",
        "Peanut-Free",
        "Tree-Nut-Free",
        "Soy-Free",
        "Fish-Free",
        "Shellfish-Free",
        "Paleo",
        "Keto",
        "Kosher"
    };
    [ContextMenu("Populate")]
    public void Populate()
    {
        // Populate the dietDropdown options
        PopulateDropdown(ReferenceManager.Instance.dietDropdown, dietLabels);

        // Populate the healthDropdown options
        PopulateDropdown(ReferenceManager.Instance.healthDropdown, healthLabels);
        
        ReferenceManager.Instance.doubleSlider.Populate();
    }

    void PopulateDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        // Clear any existing options
        dropdown.ClearOptions();

        // Add the new options from the list
        dropdown.AddOptions(options);
    }
}
