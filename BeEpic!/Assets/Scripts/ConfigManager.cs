using System.Collections;
using System.Collections.Generic;
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
        btnApoiadores.onClick.AddListener(() => MostrarTela(canvasApoiadores));
        btnVoltarApoiadores.onClick.AddListener(() => EsconderTela(canvasApoiadores));
        btnEsqueleto.onClick.AddListener(() => MostrarTela(canvasEsqueleto));
        btnVoltarEsqueleto.onClick.AddListener(() => EsconderTela(canvasEsqueleto));
        btnEnt.onClick.AddListener(() => MostrarTela(canvasEnt));
        btnVoltarEnt.onClick.AddListener(() => EsconderTela(canvasEnt));
        btnDragao.onClick.AddListener(() => MostrarTela(canvasDragao));
        btnVoltarDragao.onClick.AddListener(() => EsconderTela(canvasDragao));
        btnTarrasque.onClick.AddListener(() => MostrarTela(canvasTarrasque));
        btnVoltarTarrasque.onClick.AddListener(() => EsconderTela(canvasTarrasque));
        
    }

    public void VoltarConfig() {
        EsconderTela(canvasConfig);
    }

    public void AbrirPolitica() {
        Application.OpenURL("https://sites.google.com/view/beepic/privacy-policy");
    }

    public void AbrirInstagram() {
        Application.OpenURL("instagram://user?username=will_beepic");
    }

    public void AbrirTutorial() {
        MostrarTela(canvasTutorial);
    }

    public void VoltarTutorial() {
        EsconderTela(canvasTutorial);
    }

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
    }
}
