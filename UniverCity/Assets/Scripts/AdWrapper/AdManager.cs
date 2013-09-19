using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

[Serializable]
public class AdManager : MonoBehaviour
{
    public int BusinessID = -1;
    public AdData AdInfo = new AdData();
    public static string MediaURL = "http://www.univercity3d.com/univercity/admedia?id=";

    public IEnumerator GetAd(int id)
    {
        BusinessID = id;
        string adURL = "http://www.univercity3d.com/univercity/getAd?b=" + id;
        Debug.Log("Getting Ad From: " + adURL);
        WWW page = new WWW(adURL);
        yield return page;

        // Get the ad as a Dictionary object.
        Dictionary<string, object> ad = Json.Deserialize(page.text) as Dictionary<string, object>;
        PopulateAdData(ref AdInfo, ad);
    }

    private void PopulateAdData(ref AdData data, Dictionary<string, object> ad)
    {
        AdInfo.Background.PopulateFromJSON(ad["background"] as Dictionary<string, object>);
        AdInfo.Expert.PopulateFromJSON(ad["expert"] as Dictionary<string, object>);
        AdInfo.Font = ad["font"] as string;
        AdInfo.Version = Convert.ToInt32(ad["version"]);
        //Debug.Log(ad["pages"]);
        AdInfo.PopulatePagesFromJSON(ad["pages"] as List<object>);
    }
}

[Serializable]
public class AdBackground
{
    private BackgroundType type = BackgroundType.Solid;
    private Color topColor = Color.white;
    private Color bottomColor = Color.white;
    private Texture2D image;

    public BackgroundType Type
    {
        get { return type; }
        set { type = value; }
    }
    public Color TopColor
    {
        get { return topColor; }
        set { topColor = value; }
    }
    public Color BottomColor
    {
        get { return bottomColor; }
        set { bottomColor = value; }
    }
    public Texture2D Image
    {
        get { return image; }
        set { image = value; }
    }

    public void PopulateFromJSON(Dictionary<string, object> data)
    {
        topColor = ColorFromHex(data["color"] as string);
        bottomColor = ColorFromHex(data["color2"] as string);
        if (Convert.ToInt32(data["mediaId"]) == 0)
            image = null;
        else
            AssignImageFromURL(AdManager.MediaURL + data["mediaId"]);
        type = TypeFromString(data["type"] as string);
    }

    public Color ColorFromHex(string hex)
    {
        string red = hex.Substring(0, 2);
        string green = hex.Substring(2, 2);
        string blue = hex.Substring(4, 2);

        int redI = Convert.ToInt32(red, 16);
        int greenI = Convert.ToInt32(green, 16);
        int blueI = Convert.ToInt32(blue, 16);

        return new Color(redI / 255.0f, greenI / 255.0f, blueI / 255.0f);
    }
    public void AssignImageFromURL(string url)
    {
        WWW page = new WWW(url);
        while (!page.isDone) ;
        image = page.texture;
    }
    public BackgroundType TypeFromString(string str)
    {
        switch (str)
        {
            case "tile":
                return BackgroundType.Tile;
            case "gradient":
                return BackgroundType.Gradient;
            default:
                return BackgroundType.Solid;
        }
    }
}
[Serializable]
public class AdMedia
{
    private int id;
    private int height;
    private int width;
    private int duration;
    private MediaContentType contentType;
    private MediaPlayTime playTime;
    private MediaType type;
    private Texture2D image;
    private AudioClip mediaAudio;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public int Height
    {
        get { return height; }
        set { height = value; }
    }
    public int Width
    {
        get { return width; }
        set { width = value; }
    }
    public int Duration
    {
        get { return duration; }
        set { duration = value; }
    }
    public MediaContentType ContentType
    {
        get { return contentType; }
        set { contentType = value; }
    }
    public MediaPlayTime PlayTime
    {
        get { return playTime; }
        set { playTime = value; }
    }
    public MediaType Type
    {
        get { return type; }
        set { type = value; }
    }
    public Texture2D Image
    {
        get { return image; }
        set { image = value; }
    }
    public AudioClip MediaAudio
    {
        get { return mediaAudio; }
        set { mediaAudio = value; }
    }

    public void PopulateFromJSON(Dictionary<string, object> data)
    {
        contentType = ContentTypeFromString(data["contentType"] as string);
        height = Convert.ToInt32(data["height"]);
        id = Convert.ToInt32(data["id"]);
        playTime = PlayTimeFromString(data["playTime"] as string);
        type = MediaTypeFromString(data["type"] as string);
        width = Convert.ToInt32(data["width"]);

        if (type == MediaType.Audio)
            AssignAudioFromURL(AdManager.MediaURL + id);
        else if (type == MediaType.Image)
            AssignImageFromURL(AdManager.MediaURL + id);
    }

    public void AssignImageFromURL(string url)
    {
        WWW page = new WWW(url);
        while (!page.isDone);
        image = page.texture;
    }
    public void AssignAudioFromURL(string url)
    {
        WWW page = new WWW(url);
        while (!page.isDone) ;
        mediaAudio = page.audioClip;
    }
    public MediaContentType ContentTypeFromString(string str)
    {
        switch (str)
        {
            case "audio/basic":
                return MediaContentType.AudioBasic;
            case "audio/L24":
                return MediaContentType.AudioL24;
            case "audio/mp4":
                return MediaContentType.AudioMp4;
            case "audio/mpeg":
                return MediaContentType.AudioMpeg;
            case "audio/ogg":
                return MediaContentType.AudioOgg;
            case "audio/vorbis":
                return MediaContentType.AudioVorbis;
            case "audio/webm":
                return MediaContentType.AudioWebm;
            case "image/gif":
                return MediaContentType.ImageGif;
            case "image/jpeg":
                return MediaContentType.ImageJpeg;
            case "image/pjpeg":
                return MediaContentType.ImagePJpeg;
            case "image/png":
                return MediaContentType.ImagePng;
            case "image/svg+xml":
                return MediaContentType.ImageSvgXml;
            case "image/tiff":
                return MediaContentType.ImageTiff;
            case "video/mpeg":
                return MediaContentType.VideoMpeg;
            case "video/mp4":
                return MediaContentType.VideoMp4;
            case "video/ogg":
                return MediaContentType.VideoOgg;
            case "video/quicktime":
                return MediaContentType.VideoQuicktime;
            default:
                return MediaContentType.VideoWebm;
        }
    }
    public MediaPlayTime PlayTimeFromString(string str)
    {
        switch (str)
        {
            case "before":
                return MediaPlayTime.Before;
            case "after":
                return MediaPlayTime.After;
            default:
                return MediaPlayTime.During;
        }
    }
    public MediaType MediaTypeFromString(string str)
    {
        switch (str)
        {
            case "image":
                return MediaType.Image;
            case "video":
                return MediaType.Video;
            default:
                return MediaType.Audio;
        }
    }
}
[Serializable]
public class AdData
{
    private AdBackground background = new AdBackground();
    private AdMedia expert = new AdMedia();
    private string font;
    private int version;
    private List<AdPage> pages = new List<AdPage>();

    public AdBackground Background
    {
        get { return background; }
        set { background = value; }
    }
    public AdMedia Expert
    {
        get { return expert; }
        set { expert = value; }
    }
    public string Font
    {
        get { return font; }
        set { font = value; }
    }
    public int Version
    {
        get { return version; }
        set { version = value; }
    }
    public List<AdPage> Pages
    {
        get { return pages; }
        set { pages = value; }
    }

    public void PopulatePagesFromJSON(List<object> data)
    {
        foreach (Dictionary<string, object> page in data)
        {
            if ((page["title"] as string) != "")
            {
                AdPage curPage = new AdPage();
                curPage.PopulateFromJSON(page);
                pages.Add(curPage);
            }
        }
        Debug.Log(pages.Count);
    }
}
[Serializable]
public class AdPage
{
    private AdPageType type;
    private string title;
    private AdMedia audio = new AdMedia();
    private List<AdMedia> parts = new List<AdMedia>();
    private List<AdMarkup> text = new List<AdMarkup>();
    private string narrative;
    private AdPage more;
    private AdMedia expert = new AdMedia();
    private AdBackground background = new AdBackground();
    private string font;

    public AdPageType Type
    {
        get { return type; }
        set { type = value; }
    }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public AdMedia Audio
    {
        get { return audio; }
        set { audio = value; }
    }
    public List<AdMedia> Parts
    {
        get { return parts; }
        set { parts = value; }
    }
    public List<AdMarkup> Text
    {
        get { return text; }
        set { text = value; }
    }
    public string Narrative
    {
        get { return narrative; }
        set { narrative = value; }
    }
    public AdPage More
    {
        get { return more; }
        set { more = value; }
    }
    public AdMedia Expert
    {
        get { return expert; }
        set { expert = value; }
    }
    public AdBackground Background
    {
        get { return background; }
        set { background = value; }
    }
    public string Font
    {
        get { return font; }
        set { font = value; }
    }

    public void PopulateFromJSON(Dictionary<string, object> data)
    {
        type = PageTypeFromString(data["type"] as string);
        title = data["title"] as string;
        if (data.ContainsKey("audio"))
            audio.PopulateFromJSON(data["audio"] as Dictionary<string, object>);
        else
            audio = null;
        foreach (Dictionary<string, object> part in data["parts"] as List<object>)
        {
            AdMedia med = new AdMedia();
            med.PopulateFromJSON(part);
            parts.Add(med);
        }
        foreach (Dictionary<string, object> part in data["text"] as List<object>)
        {
            AdMarkup mar = new AdMarkup();
            mar.PopulateFromJSON(part);
            text.Add(mar);
        }
        narrative = data["narrative"] as string;
        more = new AdPage();
        if (data.ContainsKey("more") && ((data["more"] as Dictionary<string, object>)["title"] as string) != "")
            more.PopulateFromJSON(data["more"] as Dictionary<string, object>);
        else
            more = null;
        expert.PopulateFromJSON(data["expert"] as Dictionary<string, object>);
        background.PopulateFromJSON(data["background"] as Dictionary<string, object>);
        font = data["font"] as string;
    }

    public AdPageType PageTypeFromString(string str)
    {
        switch (str)
        {
            case "one":
                return AdPageType.One;
            case "two":
                return AdPageType.Two;
            case "three":
                return AdPageType.Three;
            default:
                return AdPageType.Many;
        }
    }
}
[Serializable]
public class AdMarkup
{
    private AdMarkupType type;
    private bool start;
    private string text;

    public AdMarkupType Type
    {
        get { return type; }
        set { type = value; }
    }
    public bool Start
    {
        get { return start; }
        set { start = value; }
    }
    private string Text
    {
        get { return text; }
        set { text = value; }
    }

    public void PopulateFromJSON(Dictionary<string, object> data)
    {
        type = MarkupTypeFromString(data["type"] as string);
        start = Convert.ToBoolean(data["start"]);
        text = data["text"] as string;
    }
    public AdMarkupType MarkupTypeFromString(string str)
    {
        switch (str)
        {
            case "text":
                return AdMarkupType.Text;
            case "break":
                return AdMarkupType.Break;
            case "bold":
                return AdMarkupType.Bold;
            case "italic":
                return AdMarkupType.Italic;
            case "larger":
                return AdMarkupType.Larger;
            case "smaller":
                return AdMarkupType.Smaller;
            case "color":
                return AdMarkupType.Color;
            case "blist":
                return AdMarkupType.Blist;
            default:
                return AdMarkupType.Item;
        }
    }
}

// ENUMERATIONS
public enum BackgroundType
{
    Solid,
    Gradient,
    Tile
}
public enum MediaContentType
{
    AudioBasic,
    AudioL24,
    AudioMp4,
    AudioMpeg,
    AudioOgg,
    AudioVorbis,
    AudioWebm,
    ImageGif,
    ImageJpeg,
    ImagePJpeg,
    ImagePng,
    ImageSvgXml,
    ImageTiff,
    VideoMpeg,
    VideoMp4,
    VideoOgg,
    VideoQuicktime,
    VideoWebm
}
public enum MediaPlayTime
{
    Before,
    After,
    During
}
public enum MediaType
{
    Image,
    Video,
    Audio
}
public enum AdPageType
{
    One,
    Two,
    Three,
    Many
}
public enum AdMarkupType
{
    Text,
    Break,
    Bold,
    Italic,
    Larger,
    Smaller,
    Color,
    Blist,
    Item
}