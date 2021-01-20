using UnityEngine;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour {

    [Header ("Tela Config")]
    public CanvasGroup canvasConfig;
    public Button btnVoltarConfig;
    public Button btnPoliticaPrivacidade;
    public Button btnTutorial;
    public Button btnApoiadores;
    public Button btnInstagram;

    [Header ("Tela Tutorial")]
    public CanvasGroup canvasTutorial;
    public Button btnVoltarTutorial;

    [Header ("Tela Apoiadores")]
    public CanvasGroup canvasApoiadores;
    public CanvasGroup canvasEsqueleto;
    public CanvasGroup canvasEnt;
    public CanvasGroup canvasDragao;
    public CanvasGroup canvasTarrasque;
    public Button btnVoltarApoiadores;
    public Button btnEsqueleto;
    public Button btnVoltarEsqueleto;
    public Button btnEnt;
    public Button btnVoltarEnt;
    public Button btnDragao;
    public Button btnVoltarDragao;
    public Button btnTarrasque;
    public Button btnVoltarTarrasque;
    
    void Start()
    {
        // Adiciono as funções do click dos botões da UI
        // Canvas Config
        btnVoltarConfig.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasConfig, false));
        btnPoliticaPrivacidade.onClick.AddListener(AbrirPolitica);
        btnInstagram.onClick.AddListener(AbrirInstagram);
        btnTutorial.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasTutorial, true));
        btnApoiadores.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasApoiadores, true));

        // Canvas Tutorial
        btnVoltarTutorial.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasTutorial, false));
        
        // Canvas Apoiadores
        btnVoltarApoiadores.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasApoiadores, false));
        btnEsqueleto.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasEsqueleto, true));
        btnVoltarEsqueleto.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasEsqueleto, false));
        btnEnt.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasEnt, true));
        btnVoltarEnt.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasEnt, false));
        btnDragao.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasDragao, true));
        btnVoltarDragao.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasDragao, false));
        btnTarrasque.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasTarrasque, true));
        btnVoltarTarrasque.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasTarrasque, false));
        
    }

    // Abro a política de privacidade
    public void AbrirPolitica() {
        Application.OpenURL("https://sites.google.com/view/beepic/privacy-policy");
    }

    // Redireciono para o perfil do instagram
    public void AbrirInstagram() {
        Application.OpenURL("instagram://user?username=will_beepic");
    }

}
