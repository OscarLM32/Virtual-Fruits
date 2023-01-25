using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Name: AudioController.cs
 * Brief Description: Controlador del audio, busca y reproduce sonidos.
 */
public class AudioController : MonoBehaviour
    {
    public enum Sonidos
        {
            Jump,
            ItemPickUp,
            EnemyHit,
            BackgroundMusic,
        }

    /* @Brief Description: Reproduce una sola vez el sonido que se le pasa como argumento
     * @Param sonido: sonido comprendido en el enum Sonidos.
     */
    public static void TocarSonido(Sonidos sonido, AudioSource fuenteDeAudio)
        {  
            fuenteDeAudio.PlayOneShot(GetAudioClip(sonido));
        }

        /* @Brief Description: Reproduce el sonido pasado como argumento en bucle y reduce el volumen del mismo.
         * @Param sonido: sonido comprendido en el enum Sonidos.
         */
        public static void TocarSonidoFondo(Sonidos sonido)
            {
                GameObject objetoSonido = new GameObject("SonidoFondo");
                AudioSource fuenteDeAudioFondo = objetoSonido.AddComponent<AudioSource>();
                fuenteDeAudioFondo.PlayOneShot(GetAudioClip(sonido));
                fuenteDeAudioFondo.loop = true;
                fuenteDeAudioFondo.volume = 0.4f;
            }

        /* @Brief Description: Encuentra la pista de audio correspondiente al sonido pasado por argumento.
         * @Param sonido: sonido comprendido en el enum Sonidos.
         */
        public static AudioClip GetAudioClip(Sonidos sonido)
            {
                foreach (GameAssets.ClipsDeAudio clipDeAudio in GameAssets.i.clipsDeAudioArray )
                {
                    if (clipDeAudio.tipoSonido == sonido)
                    {
                        return clipDeAudio.audio;
                    }
                }
                return null;
            }





    }


