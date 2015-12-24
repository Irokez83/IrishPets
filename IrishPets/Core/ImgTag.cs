using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IrishPets.Core
{
    public class ImgTag : IHtmlString
    {
        private readonly string m_ImagePath;
        private readonly Func<string, string> m_MapVirtualPath;
        private readonly HashSet<int> m_PixelDensities;
        private readonly IDictionary<string, string> m_HtmlAttributes;

        public ImgTag(string _imagePath, string _altText, Func<string, string> _mapVirtualPath)
        {
            m_ImagePath = _imagePath;
            m_MapVirtualPath = _mapVirtualPath;

            m_PixelDensities = new HashSet<int>();
            m_HtmlAttributes = new Dictionary<string, string>
        {
            { "src", _mapVirtualPath(_imagePath) },
            { "alt", _altText }
        };
        }

        public string ToHtmlString()
        {
            var __imgTag = new TagBuilder("img");

            if (m_PixelDensities.Any())
                this.AddSrcsetAttribute(__imgTag);
 
            foreach (KeyValuePair<string, string> _attribute in m_HtmlAttributes)
                __imgTag.Attributes[_attribute.Key] = _attribute.Value;

            return __imgTag.ToString(TagRenderMode.SelfClosing);
        }

        private void AddSrcsetAttribute(TagBuilder _imgTag)
        {
            int __densityIndex = m_ImagePath.LastIndexOf('.');

            IEnumerable<string> srcsetImagePaths = from density in m_PixelDensities
                                    let densityX = density + "x"
                                    let highResImagePath = m_ImagePath.Insert(__densityIndex, "@" + densityX) + " " + densityX
                                    select m_MapVirtualPath(highResImagePath);

            _imgTag.Attributes["srcset"] = string.Join(", ", srcsetImagePaths);
        }

        public ImgTag WithDensities(params int[] _densities)
        {
            foreach (int _density in _densities)
                m_PixelDensities.Add(_density);

            return this;
        }

        public ImgTag WithSize(int _width, int? _height = null)
        {
            m_HtmlAttributes["width"] = _width.ToString();
            m_HtmlAttributes["height"] = (_height ?? _width).ToString();

            return this;
        }
    }
}