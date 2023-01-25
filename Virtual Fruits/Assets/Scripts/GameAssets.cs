using UnityEngine;

/*
 * Name: GameAssets.cs
 * Brief Description: Clase que cumple la funci�n que hacer referencia a game assets para poder acceder a ellos desde otros scripts.
 */
public class GameAssets : MonoBehaviour
{

    private static GameAssets _i;

    /* @Brief Description: Devuelve una instancia st�tica de un prefab llamado GameAssets en la carpeta Resources.
     */
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    /* Name: ClipsDeAdudio
     * Brief Description: Clase serializada que almacena pistas de audio con su tipo de sonido correspondiente de el enumerador Sonidos.
     */
    [System.Serializable]
    public class ClipsDeAudio
    {
        public AudioController.Sonidos tipoSonido;
        public AudioClip audio;
    }

    public ClipsDeAudio[] clipsDeAudioArray;

    [Header("Stinger prefab")]
    public GameObject stinger;

}
