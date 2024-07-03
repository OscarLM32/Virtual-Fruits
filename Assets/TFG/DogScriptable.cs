using UnityEngine;

[CreateAssetMenu(fileName = "SODog", menuName = "ScriptableObjects/TFG/DogScriptable")]
public class DogScriptable : ScriptableObject
{
    public string nombre;
    public int edad;
    public Juguete jugueteFavorito;
}

public enum Juguete
{
    PELOTA,
    PELUCHE,
    CUERDA,
    MORDEDOR
}