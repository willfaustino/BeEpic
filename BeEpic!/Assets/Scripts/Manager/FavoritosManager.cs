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

    void Start()
    {
        btnFavoritosSalvar.onClick.AddListener(ValidaFavoritos);
        btnFavoritosDeletar.onClick.AddListener(ValidaDeletar);
        btnPopupFavoritosConfirmar.onClick.AddListener(SalvarFavoritos);
        btnPopupFavoritosCancelar.onClick.AddListener(() => AppManager.Instance.HabilitarTela(popupFavoritos, false));
        btnPopupFavoritosDeletarConfirmar.onClick.AddListener(DeletarFavoritos);
        btnPopupFavoritosDeletarCancelar.onClick.AddListener(() => AppManager.Instance.HabilitarTela(popupFavoritosDeletar, false));
    }

    // Verifico se existe alguma música selecionada para criar o favorito
    void ValidaFavoritos(){
        if(AppManager.Instance.ListaCaminhos().Count > 0){
            AppManager.Instance.HabilitarTela(popupFavoritos, true);
        } else{
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!", true);
        }
    }
    
    // Verifico se o usuário digitou um nome para a lista de favoritos e se estiver tudo certo, ele salva a lista
    public void SalvarFavoritos(){
        if(txtNmFavorito.text == ""){
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), você precisa digitar um nome para lista de favoritos!", true);
        } else {
            SalvarDadosArquivo(txtNmFavorito.text, AppManager.Instance.ListaCaminhos());
            AppManager.Instance.HabilitarTela(popupFavoritos, false);
        }
    }
    
    // recupera a lista de favoritos do arquivo json favoritos.epic
    public List<FavoritosData> RecuperarDadosArquivo(){
        // caminho da localização do arquivo dentro do celular
        string path = Application.persistentDataPath +  "/favoritos.epic";

        // se o arquivo exister, recupera os favoritos
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

    // Salva os favoritos em um json
    void SalvarDadosArquivo(string nomeFavorito, List<string> caminhos){

        // Se tiver alguma música selecionada, crio o json favoritos.epic
        if (caminhos.Count > 0){
            
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath +  "/favoritos.epic";
            

            List<FavoritosData> favoritos = new List<FavoritosData>();
            int idFavorito = 1;

            // Se o arquivo favoritos.epic já existe, então eu recupero o conteúdo dele e adiciono a nova lista
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
            
            // Dou um refresh na lista de favoritos
            AppManager.Instance.PopularBotoesCategoriaFavoritos();
        }else {
            AppManager.Instance.HabilitarErro(AppManager.Instance.popUpErro, "Aventureiro(a), escolha pelo menos uma música para criar a lista de favoritos!", true);
        }
    }
    
    // valido se o usuário quer deletar um favorito
    void ValidaDeletar(){
        
        string path = Application.persistentDataPath +  "/favoritos.epic";

        // Verifico primeiro se o arquivo existe
        if (File.Exists(path))
        {
            List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();

            if (favoritosArquivo.Count > 0)
            {
                // Verifico se tem alguma lista de favoritos selecionada
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

    // Deleto o favorito selecionado
    public void DeletarFavoritos(){
        
        // Recupero a lista de favoritos salva
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath +  "/favoritos.epic";

        List<FavoritosData> favoritosArquivo = RecuperarDadosArquivo();
        
        // se o id do favorito selecionado for igual ao id da lista, eu deleto ele
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

        // seto o id para 0 porque depois de deletar, nao tem mais nenhuma lista de favoritos selecionada
        idFavoritoSelecionado = 0;
        // Volto para tela principal e dou refresh na lista de favoritos
        AppManager.Instance.HabilitarTela(popupFavoritosDeletar, false);
        AppManager.Instance.PopularBotoesCategoriaFavoritos();
    }

}
