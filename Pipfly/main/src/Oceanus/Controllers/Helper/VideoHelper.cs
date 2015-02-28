using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oceanus.Controllers
{
    public class VideoHelper
    {
        public enum VideoProvider
        {
            YouTube = 0,
            Vimeo = 1
        };

        internal static void RetrieveVideoId(string videoURL, out bool isYouTube, out bool isVimeo, out string videoId)
        {
            isYouTube = false;
            isVimeo = false;
            videoId = string.Empty;

            if (videoURL.ToLower().Contains("img.youtube.com"))
            {// YouTube
                isYouTube = true;

                if (videoURL.ToLower().Contains("iframe"))
                {
                    int embedTagIndex = videoURL.ToLower().IndexOf("embed/");

                    if (embedTagIndex != -1)
                    {
                        embedTagIndex += 6;
                    }

                    int singleQuoteIndex = videoURL.ToLower().IndexOf("'", embedTagIndex);
                    singleQuoteIndex = singleQuoteIndex == -1 ? Int32.MaxValue : singleQuoteIndex;

                    int doubleQuoteIndex = videoURL.ToLower().IndexOf("\"", embedTagIndex);
                    doubleQuoteIndex = doubleQuoteIndex == -1 ? Int32.MaxValue : doubleQuoteIndex;

                    int slashIndex = videoURL.ToLower().IndexOf('\'', embedTagIndex);
                    slashIndex = slashIndex == -1 ? Int32.MaxValue : slashIndex;

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", embedTagIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int questionMarkIndex = videoURL.ToLower().IndexOf('?', embedTagIndex);
                    questionMarkIndex = questionMarkIndex == -1 ? Int32.MaxValue : questionMarkIndex;

                    int terminatingIndex = Math.Min(Math.Min(Math.Min(Math.Min(singleQuoteIndex, doubleQuoteIndex), slashIndex), ampersandIndex), questionMarkIndex);

                    videoId = videoURL.Substring(embedTagIndex, terminatingIndex - embedTagIndex);
                }
                else if (videoURL.ToLower().Contains("object"))
                {
                    int linkIndex = videoURL.ToLower().IndexOf("youtube.com/v/");

                    if (linkIndex != -1)
                    {
                        linkIndex += 14;
                    }

                    int singleQuoteIndex = videoURL.ToLower().IndexOf("'", linkIndex);
                    singleQuoteIndex = singleQuoteIndex == -1 ? Int32.MaxValue : singleQuoteIndex;

                    int doubleQuoteIndex = videoURL.ToLower().IndexOf("\"", linkIndex);
                    doubleQuoteIndex = doubleQuoteIndex == -1 ? Int32.MaxValue : doubleQuoteIndex;

                    int slashIndex = videoURL.ToLower().IndexOf('\'', linkIndex);
                    slashIndex = slashIndex == -1 ? Int32.MaxValue : slashIndex;

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", linkIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int questionMarkIndex = videoURL.ToLower().IndexOf('?', linkIndex);
                    questionMarkIndex = questionMarkIndex == -1 ? Int32.MaxValue : questionMarkIndex;

                    int terminatingIndex = Math.Min(Math.Min(Math.Min(Math.Min(singleQuoteIndex, doubleQuoteIndex), slashIndex), ampersandIndex), questionMarkIndex);

                    videoId = videoURL.Substring(linkIndex, terminatingIndex - linkIndex);
                }
                else
                {
                    int urlIndex = videoURL.ToLower().IndexOf("watch?v=");

                    if (urlIndex != -1)
                    {
                        urlIndex += 8;
                    }

                    int slashIndex = videoURL.ToLower().IndexOf('/', urlIndex);
                    slashIndex = slashIndex == -1 ? Int32.MaxValue : slashIndex;

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", urlIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int questionMarkIndex = videoURL.ToLower().IndexOf('?', urlIndex);
                    questionMarkIndex = questionMarkIndex == -1 ? Int32.MaxValue : questionMarkIndex;

                    int terminatingIndex = Math.Min(Math.Min(Math.Min(slashIndex, ampersandIndex), videoURL.Length), questionMarkIndex);

                    videoId = videoURL.Substring(urlIndex, terminatingIndex - urlIndex);
                }
            }
            else if (videoURL.ToLower().Contains("vimeo.com"))
            {//Vimeo
                isVimeo = true;

                if (videoURL.ToLower().Contains("iframe"))
                {
                    int embedTagIndex = videoURL.ToLower().IndexOf("player.vimeo.com/video/");

                    if (embedTagIndex != -1)
                    {
                        embedTagIndex += 23;
                    }

                    int singleQuoteIndex = videoURL.ToLower().IndexOf("'", embedTagIndex);
                    singleQuoteIndex = singleQuoteIndex == -1 ? Int32.MaxValue : singleQuoteIndex;

                    int doubleQuoteIndex = videoURL.ToLower().IndexOf("\"", embedTagIndex);
                    doubleQuoteIndex = doubleQuoteIndex == -1 ? Int32.MaxValue : doubleQuoteIndex;

                    int questionMarkIndex = videoURL.ToLower().IndexOf('?', embedTagIndex);
                    questionMarkIndex = questionMarkIndex == -1 ? Int32.MaxValue : questionMarkIndex;

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", embedTagIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int terminatingIndex = Math.Min(Math.Min(Math.Min(singleQuoteIndex, doubleQuoteIndex), questionMarkIndex), ampersandIndex);

                    videoId = videoURL.Substring(embedTagIndex, terminatingIndex - embedTagIndex);
                }
                else if (videoURL.ToLower().Contains("object"))
                {
                    int linkIndex = videoURL.ToLower().IndexOf("clip_id=");

                    if (linkIndex != -1)
                    {
                        linkIndex += 8;
                    }

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", linkIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int colonIndex = videoURL.ToLower().IndexOf("'", linkIndex);
                    colonIndex = colonIndex == -1 ? Int32.MaxValue : colonIndex;

                    int terminatingIndex = Math.Min(colonIndex, ampersandIndex);

                    videoId = videoURL.Substring(linkIndex, terminatingIndex - linkIndex);
                }
                else
                {
                    int urlIndex = videoURL.ToLower().IndexOf("vimeo.com/");

                    if (urlIndex != -1)
                    {
                        urlIndex += 10;
                    }

                    int slashIndex = videoURL.ToLower().IndexOf('/', urlIndex);
                    slashIndex = slashIndex == -1 ? Int32.MaxValue : slashIndex;

                    int ampersandIndex = videoURL.ToLower().IndexOf("&", urlIndex);
                    ampersandIndex = ampersandIndex == -1 ? Int32.MaxValue : ampersandIndex;

                    int terminatingIndex = Math.Min(Math.Min(slashIndex, ampersandIndex), videoURL.Length);

                    videoId = videoURL.Substring(urlIndex, terminatingIndex - urlIndex);
                }
            }
        }
    }
}