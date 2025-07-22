using System;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YOLOv8WithOpenCVForUnityExample;
public class UiManager : MonoBehaviour
{
    [SerializeField] private UIView detectionView;
    [SerializeField] private UIView gptApiView;
    [SerializeField] private UIView recipeSearchView;
    [SerializeField] private UIView menuView;
    [SerializeField] private UIView loadingView;
    [SerializeField] private UIView approachView;
    [SerializeField] private Button searchRecipeButton;
    [SerializeField] private TextMeshProUGUI detectionResultText;
    private UIView[] _allViews;
    private UIView _currentView;
    private string _currentIngredient;
    private void OnEnable()
    {
        YOLOv8ObjectDetectionExample.OnResultGenerated += OnGetDetectionResult;
        PopulateRecipeData.OnQueryComplete += QueryTaken;

    }
    private void QueryTaken()
    {
        ShowView(approachView);
    }
    public void OnLLMButtonClicked()
    {
        ShowView(gptApiView);
        ReferenceManager.Instance.llmPanel.SetActive(true);
        ReferenceManager.Instance.traditionalPanel.SetActive(false);
        ReferenceManager.Instance.chatGPTTester.Execute();
    }
    public void OnTraditionalButtonClicked()
    {
        ShowView(gptApiView);
        ReferenceManager.Instance.llmPanel.SetActive(false);
        ReferenceManager.Instance.traditionalPanel.SetActive(true);
        ReferenceManager.Instance.featureTester.Execute();
    }
    public void OnBothButtonClicked()
    {
        ShowView(gptApiView);
        ReferenceManager.Instance.llmPanel.SetActive(true);
        ReferenceManager.Instance.traditionalPanel.SetActive(true);
        ReferenceManager.Instance.chatGPTTester.Execute();
        ReferenceManager.Instance.featureTester.Execute();
    }
    public void OnAllRecipeDataFound()
    {
        ShowView(recipeSearchView);
    }
    private void OnGetDetectionResult(string result)
    {
        ToggleSearchButtonInteraction(true, result);
    }
    private void Awake()
    {
        _currentIngredient = "";
        _currentView = menuView;
        _allViews = new[]
        {
            menuView, detectionView,
            recipeSearchView,approachView,gptApiView,
            loadingView
        };
    }
    public void ShowView(UIView viewToShow)
    {
        foreach (UIView view in _allViews)
        {
            if (view.IsVisible)
            {
                view.Hide();
            }
        }
        if (viewToShow == detectionView)
        {
            ToggleSearchButtonInteraction(false, "");
        }
        viewToShow.Show();
        _currentView = viewToShow;
    }
    public void ActivatePreviousView()
    {
        int currentIndex = Array.IndexOf(_allViews, _currentView);
        int previousIndex = (currentIndex > 0) ? currentIndex - 1 : _allViews.Length - 1;
        ShowView(_allViews[previousIndex]);

    }
    private void ToggleSearchButtonInteraction(bool isEnable, string result)
    {
        _currentIngredient = result;
        detectionResultText.text = result;
        searchRecipeButton.interactable = isEnable && result != "";
    }
    public void OnSearchButtonClicked()
    {
        ReferenceManager.Instance.recipeFilterUI.Populate();
        ReferenceManager.Instance.recipeSearch.OnSearchRecipe(_currentIngredient);
        ShowView(loadingView);
    }
    public void OnFilterButtonClicked()
    {
        ReferenceManager.Instance.recipeSearch.OnSearchRecipe(_currentIngredient);
        ShowView(loadingView);
    }
    private void OnDisable()
    {
        YOLOv8ObjectDetectionExample.OnResultGenerated -= OnGetDetectionResult;
        PopulateRecipeData.OnQueryComplete -= QueryTaken;
    }
}
