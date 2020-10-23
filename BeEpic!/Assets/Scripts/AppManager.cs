using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AppManager : MonoBehaviour {

    public GameObject controleVolume; // Prefab do controle de volume
    
    [Header("Imagens")]
    public Sprite imagePlay;
    public Sprite imagePause;
    public Sprite imageLoop;
    public Sprite imageLoopPressionado;

    [Header("Botões Sons")]
    public GameObject btnSomPrefab;

    [Header("Categorias")]
    public TextMeshProUGUI nmCategoria;
    public Button btnCategoriaAmbiente;
    public Button btnCategoriaCombate;
    public Button btnCategoriaCriaturas;
    public Button btnCategoriaMagia;
    public Button btnCategoriaModerna;
    public Button btnCategoriaMusicas;
    public Button btnCategoriaNatureza;
    public Button btnCategoriaObjetos;
    public Button btnCategoriaPersonagens;
    public Button btnFavoritos;
    public Button btnInfo;

    [Header("Scrolls")]
    public GameObject contentSons;
    public GameObject contentVolumes;

    [Header("Favoritos")]
    public CanvasGroup canvasFavoritos;
    public GameObject contentFavoritos;
    public Button btnFavoritosSalvar;
    public Button btnFavoritosDeletar;
    public CanvasGroup popupFavoritos;
    public TMP_InputField txtNmFavorito;
    public Button btnPopupFavoritosConfirmar;
    public Button btnPopupFavoritosCancelar;
    public CanvasGroup popupFavoritosDeletar;
    public TextMeshProUGUI txtMensagemDeletar;
    public Button btnPopupFavoritosDeletarConfirmar;
    public Button btnPopupFavoritosDeletarCancelar;
    int idFavoritoSelecionado;
    string nomeFavoritoSelecionado;

    [Header("PopUp Erro")]
    public CanvasGroup popUpErro;
    public TextMeshProUGUI txtMensagemErro;
    public Button btnOkErro;

    [Header("PopUp Apoiar")]
    public CanvasGroup popUpApoiar;
    public Button btnSairApoiar;
    public Button btnApoiar;
    public Button btnInstagram;
    public Button btnYoutube;
    public Button btnTwitch;

    [Header("PopUp Config")]
    public CanvasGroup popUpConfig;

    // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
    List<BotoesPlay> botoesPlay = new List<BotoesPlay>();

    // Lista de Sons
    string[] musicas;
    string[] ambiente;
    string[] combate;
    string[] criaturas;
    string[] magia;
    string[] moderna;
    string[] natureza;
    string[] objetos;
    string[] personagens;
    string[] favoritos;

    void Start()
    {

        BetterStreamingAssets.Initialize();
        musicas = BetterStreamingAssets.GetFiles("Musicas", "*.ogg");
        ambiente = BetterStreamingAssets.GetFiles("Ambiente", "*.ogg");
        combate = BetterStreamingAssets.GetFiles("Combate", "*.ogg");
        criaturas = BetterStreamingAssets.GetFiles("Criaturas", "*.ogg");
        magia = BetterStreamingAssets.GetFiles("Magia", "*.ogg");
        moderna = BetterStreamingAssets.GetFiles("Moderna", "*.ogg");
        natureza = BetterStreamingAssets.GetFiles("Natureza", "*.ogg");
        objetos = BetterStreamingAssets.GetFiles("Objetos", "*.ogg");
        personagens = BetterStreamingAssets.GetFiles("Personagens", "*.ogg");
        
        Array.Sort(musicas);
        Array.Sort(ambiente);
        Array.Sort(combate);
        Array.Sort(criaturas);
        Array.Sort(magia);
        Array.Sort(moderna);
        Array.Sort(natureza);
        Array.Sort(objetos);
        Array.Sort(personagens);

        btnCategoriaMusicas.onClick.AddListener(PopularGridBotoesMusica);
        btnCategoriaAmbiente.onClick.AddListener(PopularGridBotoesAmbiente);
        btnCategoriaCombate.onClick.AddListener(PopularGridBotoesCombate);
        btnCategoriaCriaturas.onClick.AddListener(PopularGridBotoesMonstros);
        btnCategoriaMagia.onClick.AddListener(PopularGridBotoesMagias);
        btnCategoriaModerna.onClick.AddListener(PopularGridBotoesModerna);
        btnCategoriaNatureza.onClick.AddListener(PopularGridBotoesNatureza);
        btnCategoriaObjetos.onClick.AddListener(PopularGridBotoesObjetos);
        btnCategoriaPersonagens.onClick.AddListener(PopularGridBotoesPersonagens);
        btnFavoritos.onClick.AddListener(PopularGridBotoesFavoritos);
        btnFavoritosSalvar.onClick.AddListener(ValidaFavoritos);
        btnFavoritosDeletar.onClick.AddListener(ValidaDeletar);
        btnInfo.onClick.AddListener(LoadPopUpInfo);
        btnPopupFavoritosConfirmar.onClick.AddListener(SalvarFavoritos);
        btnPopupFavoritosCancelar.onClick.AddListener(() => EsconderTela(popupFavoritos));
        btnPopupFavoritosDeletarConfirmar.onClick.AddListener(DeletarFavoritos);
        btnPopupFavoritosDeletarCancelar.onClick.AddListener(() => EsconderTela(popupFavoritosDeletar));
        btnOkErro.onClick.AddListener(() => EsconderErro(popUpErro));
        btnSairApoiar.onClick.AddListener(() => EsconderTela(popUpApoiar));
        btnApoiar.onClick.AddListener(AbrirApoiaSe);
        btnInstagram.onClick.AddListener(AbrirInstagram);
        btnYoutube.onClick.AddListener(AbrirYoutube);
        btnTwitch.onClick.AddListener(AbrirTwitch);

        float width = contentSons.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / 2, 80);
        Debug.Log("newsize: " + newSize);
        Debug.Log("width: " + width);
        contentSons.GetComponent<GridLayoutGroup>().cellSize = newSize;

        PopularGridBotoesMusica();
    }

    #region Músicas

    public void AdicionarMusicaAmbiente(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        // Seto o loop para true
        Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>());
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaCombate(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaCriaturas(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaMagia(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaModerna(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        // Seto o loop para true
        Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>());
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaMusicas(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        // Seto o loop para true
        Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>());
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaNatureza(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        // Seto o loop para true
        Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>());
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaObjetos(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void AdicionarMusicaPersonagens(string nmMusica, string caminhoMusica)
    {
        GameObject sldMusica; // Create GameObject instance
        
        sldMusica = Instantiate(controleVolume, transform) as GameObject;
        sldMusica.transform.SetParent(contentVolumes.transform);
        sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
        sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);
        sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, Loaded, true);
        sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
        sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
        sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
    }

    void Play(int musicID, Button btnPlay){
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

    void Stop(int musicID, Button btnPlay)
	{
        btnPlay.GetComponent<Image>().sprite = imagePlay;
		ANAMusic.pause(musicID);
		ANAMusic.seekTo(musicID, 0);
	}

    void Loop(int musicID, Button btnLoop)
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

    public void Deletar(int musicID, GameObject volume)
    {
        ANAMusic.pause(musicID);
        ANAMusic.seekTo(musicID, 0);
        ANAMusic.release(musicID);
        Destroy(volume);
    }

    public void VolumeChange(int musicID, float sliderVolume)
    {
        ANAMusic.setVolume(musicID, sliderVolume);
    }

    void Loaded(int musicID)
	{
        ANAMusic.play(musicID, MusicaConcluida);
    }

    void MusicaConcluida(int musicID){
        foreach (BotoesPlay b in botoesPlay)
        {
            if(b.idMusica == musicID){
                b.btnPlay.GetComponent<Image>().sprite = imagePlay;
            }
        }
    }

    #endregion

    #region "Categorias"

    public void PopularGridBotoesMusica()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance

        foreach (string caminhoMusica in musicas)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(8); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaMusicas(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Músicas";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesAmbiente()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in ambiente)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(9); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaAmbiente(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Ambiente";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesCombate()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in combate)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(8); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaCombate(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Combate";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesMonstros()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in criaturas)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(10); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaCriaturas(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Monstros";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesMagias()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in magia)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(6); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaMagia(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Magias";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesModerna()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in moderna)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(8); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaModerna(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Moderna";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesNatureza()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in natureza)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(9); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaNatureza(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Natureza";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesObjetos()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in objetos)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(8); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaObjetos(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Objetos";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void PopularGridBotoesPersonagens()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance
        
        foreach (string caminhoMusica in personagens)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(12); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaPersonagens(nomeMusica, caminhoMusica));
        }

        nmCategoria.text = "Personagens";
        canvasFavoritos.alpha = 0f;
        canvasFavoritos.interactable = false;
        canvasFavoritos.blocksRaycasts = false;
    }

    public void LoadPopUpInfo()
    {
        MostrarTela(popUpConfig);
    }

    #endregion

    #region "Favoritos"

    public void PopularGridBotoesFavoritos()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance

        string path = Application.persistentDataPath +  "/favoritos.epic";

        if (File.Exists(path)){

            List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();

            foreach (FavoritosData favorito in favoritosArquivo)
            {
                btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
                btnMusica.transform.SetParent(contentFavoritos.transform);
                btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;
                
                btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = favorito.nomeFavorito;
                btnMusica.GetComponent<Button>().onClick.AddListener(() => AdicionarMusicaFavoritos(favorito.sonsFavoritos, favorito.nomeFavorito, favorito.idFavorito));
            }

        } else {
            //MostrarErro(popupErro, "Lista de favoritos não encontrada!");
        }

        canvasFavoritos.alpha = 1f;
        canvasFavoritos.interactable = true;
        canvasFavoritos.blocksRaycasts = true;
    }

    public void AdicionarMusicaFavoritos(List<string> caminhoMusica, string nomeFavorito, int idFavorito)
    {

        foreach (string caminho in caminhoMusica)
        {
            GameObject sldMusica; // Create GameObject instance
        
            sldMusica = Instantiate(controleVolume, transform) as GameObject;
            sldMusica.transform.SetParent(contentVolumes.transform);
            sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
            sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);

            string nomeMusicaComExtensao = caminho.Substring(caminho.IndexOf("/") + 1);
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nomeMusica;
            
            int musicID = ANAMusic.load(caminho, false, true, null, true);
            sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminho;
            sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
            sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
            sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
            sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => Deletar(musicID, sldMusica));
            sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
            
            sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = imagePlay;
            
            // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
            botoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        }

        idFavoritoSelecionado = idFavorito;
        nomeFavoritoSelecionado = nomeFavorito;   
    }

    void ValidaFavoritos(){
        if(ListaCaminhos().Count > 0){
            MostrarTela(popupFavoritos);
        } else{
            MostrarErro(popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!");
        }
    }
    
    public void SalvarFavoritos(){
        if(txtNmFavorito.text == ""){
            MostrarErro(popUpErro, "Aventureiro(a), você precisa digitar um nome para lista de favoritos!");
        } else {
            SalvarDadosArquivo(txtNmFavorito.text, ListaCaminhos());
            EsconderTela(popupFavoritos);
        }
    }
    
    List<FavoritosData> RecuperarDadosArquivo(){
        string path = Application.persistentDataPath +  "/favoritos.epic";

        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();

            return data.fav;
        } else {
            return null;
        }
    }

    void SalvarDadosArquivo(string nomeFavorito, List<string> caminhos){

        if (caminhos.Count > 0){
            
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath +  "/favoritos.epic";
            

            List<FavoritosData> favoritos = new List<FavoritosData>();
            int idFavorito = 1;

            if (File.Exists(path)){
                List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();   

                foreach (FavoritosData favorito in favoritosArquivo)
                {
                    favoritos.Add(favorito);
                    idFavorito = favorito.idFavorito + 1;
                } 
            }

            FileStream stream = new FileStream(path, FileMode.Create);
            FavoritosData favoritosData = new FavoritosData(idFavorito ,nomeFavorito, caminhos);
            favoritos.Add(favoritosData);

            Data data = new Data(favoritos);
            formatter.Serialize(stream, data);
            stream.Close();
            //TODO criar botao
            PopularGridBotoesFavoritos();
        }else {
            MostrarErro(popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!");
        }
    }
    
    void ValidaDeletar(){
        
        string path = Application.persistentDataPath +  "/favoritos.epic";

        if (File.Exists(path))
        {
            List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();

            if (favoritosArquivo.Count > 0)
            {
                if (idFavoritoSelecionado != 0)
                {
                    txtMensagemDeletar.text = "Aventureiro(a), você tem certeza que quer banir a playlist '" + nomeFavoritoSelecionado + "' desse plano de existencia?";
                    MostrarTela(popupFavoritosDeletar);
                } else{
                    MostrarErro(popUpErro, "Aventureiro(a), selecione uma lista para deletar!");    
                }
            } else {
                MostrarErro(popUpErro, "Aventureiro(a), antes de deletar, crie uma lista de favoritos!");
            }
            
        } else {
            MostrarErro(popUpErro, "Aventureiro(a), antes de deletar, crie uma lista de favoritos!");
        }
    }

    public void DeletarFavoritos(){

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath +  "/favoritos.epic";

        List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();
        
        for (int i = 0; i < favoritosArquivo.Count; i++)
        {
            if(favoritosArquivo[i].idFavorito == idFavoritoSelecionado){
                favoritosArquivo.RemoveAt(i);
                FileStream stream = new FileStream(path, FileMode.Create);
                Data data = new Data(favoritosArquivo);
                formatter.Serialize(stream, data);
                stream.Close();
            }
        }

        idFavoritoSelecionado = 0;
        EsconderTela(popupFavoritosDeletar);
        //TODO remover botao
        PopularGridBotoesFavoritos();
    }

    // Retorna a lista com os caminhos das músicas no grid de Volumes
    List<string> ListaCaminhos(){
        List<string> caminhos = new List<string>();

        foreach (Transform child in contentVolumes.transform)
        {
            caminhos.Add(child.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text);
        }

        return caminhos;
    }

    #endregion

    #region "Mostrar/Esconder Tela"

    void MostrarTela(CanvasGroup canvas){

        // Desativo o raycast para previnir que o usuário clique na tela anterior enquanto a animação de mostrar a tela ainda está sendo executada
        canvas.blocksRaycasts = true;
        LeanTween.alphaCanvas(canvas, 1, 0.3f).setOnComplete(() =>
        {
            // Ativo a interação com a tela depois que a animação terminar
            canvas.interactable = true;        
        });
    }

    void EsconderTela(CanvasGroup canvas){
        
        // Desativo a interação com a tela para previnir que o usuário clique nela enquanto a animação de esconder a tela ainda está sendo executada
        canvas.interactable = false;
        LeanTween.alphaCanvas(canvas, 0, 0.3f).setOnComplete(() =>
        {
            // Desativo o raycast da tela depois que a animação terminar. Permitindo que os usuários interajam com a tela anterior
            canvas.blocksRaycasts = false;
        });

        txtNmFavorito.text = "";
    }

    #endregion

    #region "PopUp Erro"

    void MostrarErro(CanvasGroup canvas, string erro)
    {
        txtMensagemErro.text = erro;

        canvas.blocksRaycasts = true;
        LeanTween.alphaCanvas(canvas, 1, 0.3f).setOnComplete(() =>
        {            
            canvas.interactable = true;    
        });
    }

    void EsconderErro(CanvasGroup canvas){

        canvas.interactable = false;
        LeanTween.alphaCanvas(canvas, 0, 0.3f).setOnComplete(() =>
        {            
            canvas.blocksRaycasts = false;
        });
    }

    #endregion

    #region "PopUp Apoiar"

    void AbrirApoiaSe(){
        Application.OpenURL("https://apoia.se/beepic");
    }

    void AbrirInstagram(){
        Application.OpenURL("instagram://user?username=will_beepic");
    }

    void AbrirYoutube(){
        Application.OpenURL("https://www.youtube.com/channel/UCCYbGgLKrd4aV2j0qT_OXpw");
    }

    void AbrirTwitch(){
        Application.OpenURL("https://www.twitch.tv/lastfencer");
    }

    #endregion
}

public class BotoesPlay {
    public int idMusica;
    public Button btnPlay;

    public BotoesPlay(int id, Button btn){
        idMusica = id;
        btnPlay = btn;
    }
}
