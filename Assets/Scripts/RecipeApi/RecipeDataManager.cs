using System;
using System.Collections.Generic;
using UnityEngine;
namespace RecipeApi
{
    [Serializable]
    public class Nutrient
    {
        public string label;
        public double quantity;
        public string unit;

        public Nutrient(string label, double quantity, string unit)
        {
            this.label = label;
            this.quantity = quantity;
            this.unit = unit;
        }
    }

    [Serializable]
    public class TotalNutrients
    {
        public Nutrient ENERC_KCAL;
        public Nutrient FAT;
        public Nutrient FASAT;
        public Nutrient FATRN;
        public Nutrient FAMS;
        public Nutrient FAPU;
        public Nutrient CHOCDF;
        public Nutrient CHOCDFnet;
        public Nutrient FIBTG;
        public Nutrient SUGAR;
        public Nutrient SUGARadded;
        public Nutrient PROCNT;
        public Nutrient CHOLE;
        public Nutrient NA;
        public Nutrient CA;
        public Nutrient MG;
        public Nutrient K;
        public Nutrient FE;
        public Nutrient ZN;
        public Nutrient P;
        public Nutrient VITA_RAE;
        public Nutrient VITC;
        public Nutrient THIA;
        public Nutrient RIBF;
        public Nutrient NIA;
        public Nutrient VITB6A;
        public Nutrient FOLDFE;
        public Nutrient FOLFD;
        public Nutrient FOLAC;
        public Nutrient VITB12;
        public Nutrient VITD;
        public Nutrient TOCPHA;
        public Nutrient VITK1;
        public Nutrient WATER;
    }

    [Serializable]
    public class TotalDaily
    {
        public Nutrient ENERC_KCAL;
        public Nutrient FAT;
        public Nutrient FASAT;
        public Nutrient CHOCDF;
        public Nutrient FIBTG;
        public Nutrient PROCNT;
        public Nutrient CHOLE;
        public Nutrient NA;
        public Nutrient CA;
        public Nutrient MG;
        public Nutrient K;
        public Nutrient FE;
        public Nutrient ZN;
        public Nutrient P;
        public Nutrient VITA_RAE;
        public Nutrient VITC;
        public Nutrient THIA;
        public Nutrient RIBF;
        public Nutrient NIA;
        public Nutrient VITB6A;
        public Nutrient FOLDFE;
        public Nutrient VITB12;
        public Nutrient VITD;
        public Nutrient TOCPHA;
        public Nutrient VITK1;
    }

    [Serializable]
    public class Digest
    {
        public string label;
        public string tag;
        public string schemaOrgTag;
        public double total;
        public bool hasRdi;
        public double daily;
        public string unit;
        public List<SubDigest> sub;
    }

    [Serializable]
    public class SubDigest
    {
        public string label;
        public string tag;
        public string schemaOrgTag;
        public double total;
        public bool hasRdi;
        public double daily;
        public string unit;
    }

    [Serializable]
    public class Hit
    {
        public Recipe recipe;
        public Links links;
    }

    [Serializable]
    public class Images
    {
        public ImageDetail thumbnail;
        public ImageDetail small;
        public ImageDetail regular;
        public ImageDetail large;
    }

    [Serializable]
    public class ImageDetail
    {
        public string url;
        public int width;
        public int height;
    }

    [Serializable]
    public class Ingredient
    {
        public string text;
        public double quantity;
        public string measure;
        public string food;
        public double weight;
        public string foodCategory;
        public string foodId;
        public string image;
    }

    [Serializable]
    public class Links
    {
        public LinkDetail next;
        public LinkDetail self;
    }

    [Serializable]
    public class LinkDetail
    {
        public string href;
        public string title;
    }

    [Serializable]
    public class Recipe
    {
        public string uri;
        public string label;
        public string image;
        public Images images;
        public Sprite sprite;
        public string source;
        public string url;
        public string shareAs;
        public double yield;
        public List<string> dietLabels;
        public List<string> healthLabels;
        public List<string> cautions;
        public List<string> ingredientLines;
        public List<Ingredient> ingredients;
        public double calories;
        public object TotalCo2Emissions;
        public string co2EmissionsClass;
        public double totalWeight;
        public double totalTime;
        public List<string> cuisineType;
        public List<string> mealType;
        public List<string> dishType;
        public TotalNutrients totalNutrients;
        public TotalDaily totalDaily;
        public List<Digest> digest;
        public List<string> tags;
    }

    [Serializable]
    public class RecipeDataManager
    {
        public int from;
        public int to;
        public int count;
        public Links links;
        public List<Hit> hits;
    }
}