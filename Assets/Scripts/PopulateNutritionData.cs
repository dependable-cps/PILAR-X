using System;
using RecipeApi;
using TMPro;
using UnityEngine;

public class PopulateNutritionData : MonoBehaviour
{
    [SerializeField] private Digest _digest;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI amount;

    public void Populate(Digest digest, bool isMid)
    {
        _digest = digest;
        name.text = _digest.label;
        amount.text = Math.Round(_digest.total).ToString();
    }
}
