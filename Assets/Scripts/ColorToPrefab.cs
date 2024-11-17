/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : ColorToPrefab.cs
/// Description : This struct represents a mapping between a color and a prefab.
///               It is used to associate specific colors with corresponding prefab objects.
/// Author : Kazuo Reis de Andrade
/// </summary>

using UnityEngine;

[System.Serializable]
public struct ColorToPrefab
{
    public Color color;
    public GameObject prefab;
}