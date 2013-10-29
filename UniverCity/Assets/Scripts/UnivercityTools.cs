using UnityEngine;
using System.Collections;

public static class UnivercityTools 
{

    static public void ScaleImage(GameObject destination, Texture2D source)
    {
        float newWidth = (destination.transform.localScale.y / source.height) * source.width;
        float newHeight = (destination.transform.localScale.x / source.width) * source.height;

        if (source.width > source.height)
        {
            if (newHeight > destination.transform.localScale.y)
            {
                destination.transform.localScale = new Vector3(
                    newWidth,
                    destination.transform.localScale.y,
                    0.0f);
            }
            else
            {
                destination.transform.localScale = new Vector3(
                    destination.transform.localScale.x,
                    newHeight,
                    0.0f);
            }
        }
        else
        {
            if (newWidth > destination.transform.localScale.x)
            {
                destination.transform.localScale = new Vector3(
                    destination.transform.localScale.x,
                    newHeight,
                    0.0f);
            }
            else
            {
                destination.transform.localScale = new Vector3(
                    newWidth,
                    destination.transform.localScale.y,
                    0.0f);
            }
        }

        destination.GetComponent<UITexture>().mainTexture = source;
    }

    static public void ScaleVideo(GameObject destination, int height, int width)
    {
        float newWidth = (destination.transform.localScale.y / height) * width;
        float newHeight = (destination.transform.localScale.x / width) * height;

        if (width > height)
        {
            if (newHeight > destination.transform.localScale.y)
            {
                destination.transform.localScale = new Vector3(
                    newWidth,
                    destination.transform.localScale.y,
                    0.0f);
            }
            else
            {
                destination.transform.localScale = new Vector3(
                    destination.transform.localScale.x,
                    newHeight,
                    0.0f);
            }
        }
        else
        {
            if (newWidth > destination.transform.localScale.x)
            {
                destination.transform.localScale = new Vector3(
                    destination.transform.localScale.x,
                    newHeight,
                    0.0f);
            }
            else
            {
                destination.transform.localScale = new Vector3(
                    newWidth,
                    destination.transform.localScale.y,
                    0.0f);
            }
        }

    }
}
