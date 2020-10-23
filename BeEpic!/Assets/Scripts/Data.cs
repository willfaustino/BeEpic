using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data 
{
    public List<FavoritosData> fav;

    public Data(List<FavoritosData> favoritos){
        fav = new List<FavoritosData>(favoritos);
    }
}
