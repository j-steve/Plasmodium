using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Biome
{
    public static List<Biome> BIOMES = new List<Biome>
    {
        new Biome("Wood", "Brown", 8, 6, 8, 3, 1),
        new Biome("Leaf", "TealGreen", 2, 3, 5, 4, 1),
        new Biome("Moss", "Green", 4, 5, 6, -1, 1),
        new Biome("Stone", "Dark", 7, 7, 9, 2, 1)
    };


    public string Name { get; private set; }
    public Material Material { get; private set; }
    public int Moisture { get; private set; }
    public int Nutrients { get; private set; }
    public int Oxygen { get; private set; }
    public int Temperature { get; private set; }
    public int Deco { get; private set; }

    // Constructor to initialize the Biome
    public Biome(string name, string materialName, int moisture, int nutrients, int oxygen, int temperature, int deco)
    {
        Name = name;
        Material = LoadMaterial(materialName);
        Moisture = moisture;
        Nutrients = nutrients;
        Oxygen = oxygen;
        Temperature = temperature;
        Deco = deco;
    }


    // Method to load material from the Resources folder
    private Material LoadMaterial(string materialName)
    {
        Material loadedMaterial = Resources.Load<Material>("HexMaterials/" + materialName);
        if (loadedMaterial != null)
        {
            return loadedMaterial;
        }
        else
        {
            Debug.LogWarning("Material not found: " + materialName);
            return null;
        }
    }

}
