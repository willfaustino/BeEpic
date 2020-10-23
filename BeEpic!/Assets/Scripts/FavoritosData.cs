using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FavoritosData
{
    public int idFavorito;
    public string nomeFavorito;
    public List<string> sonsFavoritos;

    public FavoritosData(int idFav, string nomeFav, List<string> sons){
        idFavorito = idFav;
        nomeFavorito = nomeFav;
        sonsFavoritos = new List<string>(sons);
    }
}
