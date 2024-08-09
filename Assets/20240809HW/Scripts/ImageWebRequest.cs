using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MyProject{

    public class ImageWebRequest : MonoBehaviour
    {
        public string[] imageURL = new string[8];
        public Image[] image;

        private void Start()
        {
            for(int i = 0; i < image.Length; i++)
            {
                GetImageAsync(i);
            }
        }

        async void GetImageAsync(int index)
        {
            StartCoroutine(GetImage(index));
        }

        IEnumerator GetImage(int index)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL[index]);

            var operation = www.SendWebRequest();

            yield return operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP 통신 실패 : {www.error}");
            }
            else
            {
                Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

                image[index].sprite = sprite;
            }
        }
    }
}
