using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    
    class GeneratorUrl
    {
        public string postUrl;
        public Regex tags_header;
        public Regex tags_body;
        public string endBlock = "</d>";
        public List<string> replaces = new List<string>(){"<p>", "<img"};
    }
    [System.Serializable]
    public struct ResponseStruct
    {
        public string y;
        public string z;
    }
    [System.Serializable]
    public struct Response
    {
        public ResponseStruct[] posts;
    }
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text main;
    [SerializeField] private RectTransform mainCont;
    [SerializeField] private Image _image;
    public List<Sprite> images = new List<Sprite>();
    Regex rx = new Regex(@"",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Dictionary<string, GeneratorUrl> generatorsULRS = new Dictionary<string, GeneratorUrl>()
    {
        {"Генерация локации", new GeneratorUrl{postUrl = "https://stormtower.ru/wp-content/themes/stormboot/gen/region.php", tags_header = new Regex(@"<b>(.+)<\/b>", RegexOptions.Compiled | RegexOptions.IgnoreCase), tags_body = new Regex(@"<p>([\s\S]+)<img", RegexOptions.Compiled | RegexOptions.IgnoreCase)}},
        {"Генерация святых мест", new GeneratorUrl{postUrl = "https://stormtower.ru/wp-content/themes/stormboot/gen/holy.php", tags_header = new Regex(@"<b>(.+)<\/b>"), tags_body = new Regex(@"<p>([\s\S]+)<img")}}
    };
        
    void Start()
    {
        mainCont.anchoredPosition = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateButton()
    {
        StartCoroutine(Generate());
    } 
    public IEnumerator Generate()
    {
        GeneratorUrl url = generatorsULRS.Values.ToList()[Random.Range(0, generatorsULRS.Count)];
        Debug.Log(url.postUrl);
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("y", 1);
        wwwForm.AddField("z", Random.Range(0, 6));
        UnityWebRequest req = UnityWebRequest.Post(url.postUrl, wwwForm);
        yield return req.SendWebRequest();
        Match header_match = url.tags_header.Match(req.downloadHandler.text);
        header.text = header_match.Value;
        
        Match main_match = url.tags_body.Match(req.downloadHandler.text);
        string text = main_match.Value;
        foreach (var str in url.replaces)
        {
            text = text.Replace(str, "");
        }
        main.text = text;
        mainCont.anchoredPosition = new Vector2(0, 0);
        _image.sprite = images[Random.Range(0, images.Count)];
    }
    
}
