using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FavoritosManager : MonoBehaviour
{
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

    [HideInInspector]
    public int idFavoritoSelecionado;
    [HideInInspector]
    public string nomeFavoritoSelecionado;

    // Start is called before the first frame update
    void Start()
    {
        btnFavoritosSalvar.onClick.AddListener(ValidaFavoritos);
        btnFavoritosDeletar.onClick.AddListener(ValidaDeletar);
        btnPopupFavoritosConfirmar.onClick.AddListener(SalvarFavoritos);
        btnPopupFavoritosCancelar.onClick.AddListener(() => AppManager.Instance.HabilitarTela(popupFavoritos, false));
        btnPopupFavoritosDeletarConfirmar.onClick.AddListener(DeletarFavoritos);
        btnPopupFavoritosDeletarCancelar.onClick.AddListener(() => AppManager.Instance.HabilitarTela(popupFavoritosDeletar, false));
    }

    void ValidaFavoritos(){
        if(AppManager.Instance.ListaCaminhos().Count > 0){
            AppManager.Instance.HabilitarTela(popupFavoritos, true);
        } else{
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!", true);
        }
    }
    
    public void SalvarFavoritos(){
        if(txtNmFavorito.text == ""){
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), você precisa digitar um nome para lista de favoritos!", true);
        } else {
            SalvarDadosArquivo(txtNmFavorito.text, AppManager.Instance.ListaCaminhos());
            AppManager.Instance.HabilitarTela(popupFavoritos, false);
        }
    }
    
    public List<FavoritosData> RecuperarDadosArquivo(){
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
            AppManager.Instance.PopularBotoesCategoriaFavoritos();
        }else {
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!", true);
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
                    AppManager.Instance.HabilitarTela(popupFavoritosDeletar, true);
                } else{
                    AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), selecione uma lista para deletar!", true);    
                }
            } else {
                AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), antes de deletar, crie uma lista de favoritos!", true);
            }
            
        } else {
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), antes de deletar, crie uma lista de favoritos!", true);
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
        AppManager.Instance.HabilitarTela(popupFavoritosDeletar, false);
        //TODO remover botao
        AppManager.Instance.PopularBotoesCategoriaFavoritos();
    }

}
