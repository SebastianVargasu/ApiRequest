using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpHandler : MonoBehaviour
{
    public RawImage[] image;
    public TextMeshProUGUI[] cardName;
    public TextMeshProUGUI[] specieName;
    public TextMeshProUGUI username;
    public int imagePosition=0;
    private int currentId = 1;

    public GameObject BotonComenzar;
    public GameObject Botonsiguiente;
    public GameObject BotonAnterior;
    
    private string fakeApiUrl = "https://my-json-server.typicode.com/Dhako197/JsonPlaceholder";
    private string RickAndMortyUrl= "https://rickandmortyapi.com/api";
    
    public void SendRequest()
    {
        StartCoroutine("GetUserData", currentId);
    }
    /*
    IEnumerator GetCharacters()
    {
        UnityWebRequest request = UnityWebRequest.Get(RickAndMortyUrl +"/character");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            //byte[] result = request.downloadHandler.data;

            if (request.responseCode == 200)
            {
                JsonData data =JsonUtility.FromJson<JsonData>(request.downloadHandler.text);
                foreach (CharacterData character in data.results)
                {
                    Debug.Log( character.name+ " is a "+ character.species);
                }
                
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error );
            }
        }
        
    }
    */

    IEnumerator GetUserData(int uid)
    {
        UnityWebRequest request = UnityWebRequest.Get(fakeApiUrl +"/users/" + uid);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            //byte[] result = request.downloadHandler.data;

            if (request.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                username.text = "Usuario: "+user.username;
               Debug.Log(user.username);

               foreach (int card in user.deck )
               {
                   StartCoroutine("GetCharacter", card);
                   
               }
               imagePosition = 0;
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error );
            }
        }
    }
    
    IEnumerator GetCharacter(int id)
    {
        UnityWebRequest request = UnityWebRequest.Get(RickAndMortyUrl +"/character/" + id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            //byte[] result = request.downloadHandler.data;

            if (request.responseCode == 200)
            {
                CharacterData character =JsonUtility.FromJson<CharacterData>(request.downloadHandler.text);
               Debug.Log(character.name + " is a " + character.species);
               cardName[imagePosition].text = character.name;
               specieName[imagePosition].text = character.species;
               StartCoroutine(DownloadImage(character.image, imagePosition));
               imagePosition++;

            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error );
            }
        }
        
    }

    IEnumerator DownloadImage(string url, int place)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else if(request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else image[place].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

    public void Comenzar()
    {
        BotonComenzar.SetActive(false);
        Botonsiguiente.SetActive(true);
        BotonAnterior.SetActive(true);
    }

    public void Siguiente()
    {
        currentId++;
        if (currentId > 3)
            currentId = 1;
        StartCoroutine("GetUserData", currentId);
    }

    public void Anterior()
    {
        currentId--;
        if (currentId < 1)
            currentId = 3;
        StartCoroutine("GetUserData", currentId);
    }
   
}

[System.Serializable]
public class JsonData
{
    public CharacterData[] results;
    public UserData[] users;
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public string species;
    public string image;
}

[System.Serializable]
public class UserData
{
    public int id;
    public string username;
    public int[] deck;

}
