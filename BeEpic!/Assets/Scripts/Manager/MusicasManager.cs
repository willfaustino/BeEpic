using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicasManager : MonoBehaviour
{
    public Sprite imageLoop;
    public Sprite imageLoopPressionado;
    public Sprite imagePlay;
    public Sprite imagePause;
    // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
    public List<BotoesPlay> listaBotoesPlay = new List<BotoesPlay>();
    
    // Toca a música
    // O ANAMusic é o plugin que eu uso para fazer o sons tocar em background e com a tela bloqueada
    public void Play(int musicID, Button btnPlay){
		if (ANAMusic.isPlaying(musicID))
		{
            btnPlay.GetComponent<Image>().sprite = imagePlay;
            ANAMusic.pause(musicID);
		}
		else
		{
			btnPlay.GetComponent<Image>().sprite = imagePause;
            ANAMusic.play(musicID, MusicaConcluida);
		}
    }

    // Para a música e faz ela voltar para o início
    public void Stop(int musicID, Button btnPlay)
	{
        btnPlay.GetComponent<Image>().sprite = imagePlay;
		ANAMusic.pause(musicID);
		ANAMusic.seekTo(musicID, 0);
	}

    // Seta a música para fazer o loop
    public void Loop(int musicID, Button btnLoop)
	{   
		if (ANAMusic.isLooping(musicID))
		{
            btnLoop.GetComponent<Image>().sprite = imageLoop;
			ANAMusic.setLooping(musicID, false);
		}
		else
		{
            btnLoop.GetComponent<Image>().sprite = imageLoopPressionado;
			ANAMusic.setLooping(musicID, true);
		}
	}

    // Deleta a música da lista
    public void Deletar(int musicID, GameObject volume)
    {
        ANAMusic.pause(musicID);
        ANAMusic.seekTo(musicID, 0);
        ANAMusic.release(musicID);
        Destroy(volume);
    }

    // Altera o volume
    public void VolumeChange(int musicID, float sliderVolume)
    {
        ANAMusic.setVolume(musicID, sliderVolume);
    }

    public void Loaded(int musicID)
	{
        ANAMusic.play(musicID, MusicaConcluida);
    }

    // Quando a música terminar, volta o botão de pause para play
    public void MusicaConcluida(int musicID){
        foreach (BotoesPlay b in listaBotoesPlay)
        {
            if(b.idMusica == musicID){
                b.btnPlay.GetComponent<Image>().sprite = imagePlay;
            }
        }
    }
}
