using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

// Classe singleton que controla todas as funcionalidades do app e interações entre os outros Managers
public class AppManager : MonoBehaviour {

    #region "Instance"

    private static AppManager _instance;

    public static AppManager Instance {
        get { return _instance; }
        set {
            if (_instance == null) {
                _instance = value;
                DontDestroyOnLoad(_instance.gameObject);           
            }
        }
    }

    protected virtual void Awake() {
        transform.SetParent(null);
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }
            
        Instance = this;
        
    }

    #endregion

    // Managers
    public FavoritosManager favoritosManager;
    public MusicasManager musicasManager;

    [Header("Prefab Sound Player")]
    public GameObject prefabSoundPlayer; // Prefab do controle de volume

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
    public GameObject contentFavoritos;
    public CanvasGroup canvasFavoritos;

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

    // Lista de Sons
    string[] musicas;
    string[] ambiente;
    string[] combate;
    string[] monstros;
    string[] magias;
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
        monstros = BetterStreamingAssets.GetFiles("Criaturas", "*.ogg");
        magias = BetterStreamingAssets.GetFiles("Magia", "*.ogg");
        moderna = BetterStreamingAssets.GetFiles("Moderna", "*.ogg");
        natureza = BetterStreamingAssets.GetFiles("Natureza", "*.ogg");
        objetos = BetterStreamingAssets.GetFiles("Objetos", "*.ogg");
        personagens = BetterStreamingAssets.GetFiles("Personagens", "*.ogg");
        
        Array.Sort(musicas);
        Array.Sort(ambiente);
        Array.Sort(combate);
        Array.Sort(monstros);
        Array.Sort(magias);
        Array.Sort(moderna);
        Array.Sort(natureza);
        Array.Sort(objetos);
        Array.Sort(personagens);

        btnCategoriaMusicas.onClick.AddListener(() => PopularBotoesCategoria(musicas, "Músicas", true));
        btnCategoriaAmbiente.onClick.AddListener(() => PopularBotoesCategoria(ambiente, "Ambiente", true));
        btnCategoriaCombate.onClick.AddListener(() => PopularBotoesCategoria(combate, "Combate", false));
        btnCategoriaCriaturas.onClick.AddListener(() => PopularBotoesCategoria(monstros, "Monstros", false));
        btnCategoriaMagia.onClick.AddListener(() => PopularBotoesCategoria(magias, "Magias", false));
        btnCategoriaModerna.onClick.AddListener(() => PopularBotoesCategoria(moderna, "Moderna", true));
        btnCategoriaNatureza.onClick.AddListener(() => PopularBotoesCategoria(natureza, "Natureza", true));
        btnCategoriaObjetos.onClick.AddListener(() => PopularBotoesCategoria(objetos, "Objetos", false));
        btnCategoriaPersonagens.onClick.AddListener(() => PopularBotoesCategoria(personagens, "Personagens", false));
        btnFavoritos.onClick.AddListener(PopularBotoesCategoriaFavoritos);
        btnInfo.onClick.AddListener(() => HabilitarTela(popUpConfig, true));

        btnOkErro.onClick.AddListener(() => HabilitarErro(popUpErro, null, false));
        btnSairApoiar.onClick.AddListener(() => HabilitarTela(popUpApoiar, false));
        btnApoiar.onClick.AddListener(AbrirApoiaSe);
        btnInstagram.onClick.AddListener(AbrirInstagram);
        btnYoutube.onClick.AddListener(AbrirYoutube);
        btnTwitch.onClick.AddListener(AbrirTwitch);

        float width = contentSons.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / 2, 80);
        contentSons.GetComponent<GridLayoutGroup>().cellSize = newSize;

        PopularBotoesCategoria(musicas, "Músicas", true);
    }

    #region Músicas

    public void SelecionarMusica(string nmMusica, string caminhoMusica, Boolean isLooping)
    {
        GameObject soundPlayer; // Create GameObject instance
        
        soundPlayer = Instantiate(prefabSoundPlayer, transform) as GameObject;
        soundPlayer.transform.SetParent(contentVolumes.transform);
        soundPlayer.GetComponent<RectTransform>().localScale = Vector3.one;
        soundPlayer.transform.position = new Vector3(soundPlayer.transform.position.x, soundPlayer.transform.position.y, 0);
        soundPlayer.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nmMusica;
        
        int musicID = ANAMusic.load(caminhoMusica, false, true, musicasManager.Loaded, true);
        soundPlayer.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminhoMusica;
        soundPlayer.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => musicasManager.Play(musicID, soundPlayer.transform.Find("btnPlay").GetComponent<Button>()));
        soundPlayer.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => musicasManager.Stop(musicID, soundPlayer.transform.Find("btnPlay").GetComponent<Button>()));
        soundPlayer.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => musicasManager.Loop(musicID, soundPlayer.transform.Find("btnLoop").GetComponent<Button>()));
        soundPlayer.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => musicasManager.Deletar(musicID, soundPlayer));
        soundPlayer.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ musicasManager.VolumeChange(musicID, soundPlayer.transform.Find("sldVolume").GetComponent<Slider>().value); });
        
        if(isLooping){
            // Seto o loop para true
            musicasManager.Loop(musicID, soundPlayer.transform.Find("btnLoop").GetComponent<Button>());
        }

        soundPlayer.transform.Find("btnPlay").GetComponent<Image>().sprite = musicasManager.imagePause;

        // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
        musicasManager.listaBotoesPlay.Add(new BotoesPlay(musicID, soundPlayer.transform.Find("btnPlay").GetComponent<Button>()));
    }

    public void SelecionarMusicaFavoritos(List<string> caminhoMusica, string nomeFavorito, int idFavorito)
    {

        foreach (string caminho in caminhoMusica)
        {
            GameObject sldMusica; // Create GameObject instance
        
            sldMusica = Instantiate(prefabSoundPlayer, transform) as GameObject;
            sldMusica.transform.SetParent(contentVolumes.transform);
            sldMusica.GetComponent<RectTransform>().localScale = Vector3.one;
            sldMusica.transform.position = new Vector3(sldMusica.transform.position.x, sldMusica.transform.position.y, 0);

            string nomeMusicaComExtensao = caminho.Substring(caminho.IndexOf("/") + 1);
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            sldMusica.transform.Find("imgBaseVolume").Find("txtImg").GetComponent<TextMeshProUGUI>().text = nomeMusica;
            
            int musicID = ANAMusic.load(caminho, false, true, null, true);
            sldMusica.transform.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text = caminho;
            sldMusica.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(() => musicasManager.Play(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
            sldMusica.transform.Find("btnStop").GetComponent<Button>().onClick.AddListener(() => musicasManager.Stop(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
            sldMusica.transform.Find("btnLoop").GetComponent<Button>().onClick.AddListener(() => musicasManager.Loop(musicID, sldMusica.transform.Find("btnLoop").GetComponent<Button>()));
            sldMusica.transform.Find("btnDeletar").GetComponent<Button>().onClick.AddListener(() => musicasManager.Deletar(musicID, sldMusica));
            sldMusica.transform.Find("sldVolume").GetComponent<Slider>().onValueChanged.AddListener(delegate{ musicasManager.VolumeChange(musicID, sldMusica.transform.Find("sldVolume").GetComponent<Slider>().value); });
            
            sldMusica.transform.Find("btnPlay").GetComponent<Image>().sprite = musicasManager.imagePlay;
            
            // Uso o botoesPlay para fazer o callback do botão Play/Pause alterar a imagem caso a música termine
            musicasManager.listaBotoesPlay.Add(new BotoesPlay(musicID, sldMusica.transform.Find("btnPlay").GetComponent<Button>()));
        }

        favoritosManager.idFavoritoSelecionado = idFavorito;
        favoritosManager.nomeFavoritoSelecionado = nomeFavorito;   
    }

    #endregion

    #region "Categorias"

    public void PopularBotoesCategoria(string[] listaSons, string nomeCategoria, Boolean loop)
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance

        foreach (string caminhoMusica in listaSons)
        {
            btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
            btnMusica.transform.SetParent(contentSons.transform);
            btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;

            string nomeMusicaComExtensao = caminhoMusica.Substring(8); 
            string nomeMusica = nomeMusicaComExtensao.Substring(0, nomeMusicaComExtensao.IndexOf("."));

            btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = nomeMusica;
            btnMusica.GetComponent<Button>().onClick.AddListener(() => SelecionarMusica(nomeMusica, caminhoMusica, loop));
        }

        nmCategoria.text = nomeCategoria;
        HabilitarFavoritos(false);
    }

    public void PopularBotoesCategoriaFavoritos()
    {
        // Destruo todos os botões e populo o scroll view com os botões da categoria selecionada
        GameObject[] botoes = GameObject.FindGameObjectsWithTag("btnSom");
        foreach (GameObject btnSom in botoes) {
            GameObject.Destroy(btnSom);
        }
        
        GameObject btnMusica; // Create GameObject instance

        string path = Application.persistentDataPath +  "/favoritos.epic";

        if (File.Exists(path)){

            List<FavoritosData> favoritosArquivo = favoritosManager.RecuperarDadosArquivo();

            foreach (FavoritosData favorito in favoritosArquivo)
            {
                btnMusica = Instantiate(btnSomPrefab, transform) as GameObject;
                btnMusica.transform.SetParent(contentFavoritos.transform);
                btnMusica.GetComponent<RectTransform>().localScale = Vector3.one;
                
                btnMusica.GetComponentInChildren<TextMeshProUGUI>().text = favorito.nomeFavorito;
                btnMusica.GetComponent<Button>().onClick.AddListener(() => SelecionarMusicaFavoritos(favorito.sonsFavoritos, favorito.nomeFavorito, favorito.idFavorito));
            }

        } else {
            //MostrarErro(popupErro, "Lista de favoritos não encontrada!");
        }

        HabilitarFavoritos(true);
    }

    #endregion

    #region "Favoritos"

    void HabilitarFavoritos(Boolean habilitar){
        if(habilitar){
            canvasFavoritos.alpha = 1f;
            canvasFavoritos.interactable = true;
            canvasFavoritos.blocksRaycasts = true;
        } else{
            canvasFavoritos.alpha = 0f;
            canvasFavoritos.interactable = false;
            canvasFavoritos.blocksRaycasts = false;
        }
    }

    // Retorna a lista com os caminhos das músicas no grid de Volumes
    public List<string> ListaCaminhos(){
        List<string> caminhos = new List<string>();

        foreach (Transform child in contentVolumes.transform)
        {
            caminhos.Add(child.Find("txtCaminho").GetComponent<TextMeshProUGUI>().text);
        }

        return caminhos;
    }

    #endregion

    public void HabilitarTela(CanvasGroup canvas, Boolean isActive){
        if(isActive){
            // Desativo o raycast para previnir que o usuário clique na tela anterior enquanto a animação de mostrar a tela ainda está sendo executada
            canvas.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvas, 1, 0.3f).setOnComplete(() =>
            {
                // Ativo a interação com a tela depois que a animação terminar
                canvas.interactable = true;        
            });
        } else{
            // Desativo a interação com a tela para previnir que o usuário clique nela enquanto a animação de esconder a tela ainda está sendo executada
            canvas.interactable = false;
            LeanTween.alphaCanvas(canvas, 0, 0.3f).setOnComplete(() =>
            {
                // Desativo o raycast da tela depois que a animação terminar. Permitindo que os usuários interajam com a tela anterior
                canvas.blocksRaycasts = false;
            });

            favoritosManager.txtNmFavorito.text = "";
        }
    }

    public void HabilitarErro(CanvasGroup canvas, string erro, Boolean isActive){

        if(isActive){

            txtMensagemErro.text = erro;

            canvas.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvas, 1, 0.3f).setOnComplete(() =>
            {            
                canvas.interactable = true;    
            });

        } else{

            canvas.interactable = false;
            LeanTween.alphaCanvas(canvas, 0, 0.3f).setOnComplete(() =>
            {            
                canvas.blocksRaycasts = false;
            });

        }
    }

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
