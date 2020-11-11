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
        btnVoltarConfig.onClick.AddListener(VoltarConfig);
        btnPoliticaPrivacidade.onClick.AddListener(AbrirPolitica);
        btnInstagram.onClick.AddListener(AbrirInstagram);
        btnTutorial.onClick.AddListener(AbrirTutorial);
        btnVoltarTutorial.onClick.AddListener(VoltarTutorial);
        btnApoiadores.onClick.AddListener(() => AppManager.Instance.HabilitarTela(canvasApoiadores, true));
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

    public void VoltarConfig() {
        AppManager.Instance.HabilitarTela(canvasConfig, false);
    }

    public void AbrirPolitica() {
        Application.OpenURL("https://sites.google.com/view/beepic/privacy-policy");
    }

    public void AbrirInstagram() {
        Application.OpenURL("instagram://user?username=will_beepic");
    }

    public void AbrirTutorial() {
        AppManager.Instance.HabilitarTela(canvasTutorial, true);
    }

    public void VoltarTutorial() {
        AppManager.Instance.HabilitarTela(canvasTutorial, true);
    }

}
