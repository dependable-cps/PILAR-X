using TMPro;
using TS.DoubleSlider;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance;
    public RecipeSearch recipeSearch;
    public UiManager uiManager;
    public GameObject recipePf;
    public Transform recipeParent;
    public  ScrollRect scrollRect;
    public ChatGPTTester chatGPTTester;
    [FormerlySerializedAs("dummyTester")]
    public FeatureTester featureTester;
    public TMP_Dropdown dietDropdown;
    public TMP_Dropdown healthDropdown;
    public RecipeFilterUI recipeFilterUI;
    public DoubleSlider doubleSlider;
    public GameObject llmPanel;
    public GameObject traditionalPanel;
    public TMP_Text queryText;
    public Image recipeImage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
