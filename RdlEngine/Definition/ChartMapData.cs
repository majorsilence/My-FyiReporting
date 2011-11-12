/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Globalization;

namespace fyiReporting.RDL
{
    internal class MapData
    {
        static Dictionary<string, MapData> _MapCache = new Dictionary<string, MapData>();               // cache data
        Dictionary<string, List<MapPolygon>> _DataPoints = new Dictionary<string, List<MapPolygon>>();  // all the data values
        List<MapObject> _MapObjects = new List<MapObject>();    // keep polygons to draw the outline (ie in case an entry isn't filled
        float _MaxX=0;         // Maximum x coordinate
        float _MaxY=0;         // Maximum y coordinate
        DateTime _TimeStamp;

        /// <summary>
        /// Creates MapData from XML 
        /// </summary>
        /// <param name="xr"></param>
        internal MapData(XmlReader xr)
        {
            xr.Read();
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "MapData")
                ReadXmlMapData(xr);                // Read it
            return;
        }

        private void ReadXmlMapData(XmlReader xr)
        {
            // Read thru all the mapdata objects
            while (xr.Read())
            {
                if (!xr.IsStartElement())
                    continue;

                switch (xr.Name)
                {
                    case "Polygon":
                        ReadXmlPolygon(xr);
                        break;
                    case "Lines":
                        ReadXmlLines(xr);
                        break;
                    case "Text":
                        ReadXmlText(xr);
                        break;
                    default:                // just skip unknown elements
                        break;
                }
            }
        }

        private void ReadXmlLines(XmlReader xr)
        {
            PointF[] pts = null;
            // Read thru all the Lines objects
            while (xr.Read())
            {
                if (!xr.IsStartElement())
                {
                    if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "Lines")
                        break;
                    continue;
                }

                switch (xr.Name)
                {
                    case "Points":
                        pts = ReadXmlPoints(xr);
                        break;
                }
            }
            if (pts != null)
                AddMapObject(new MapLines(pts));
        }

        private PointF[] ReadXmlPoints(XmlReader xr)
        {
            string spts = xr.ReadString();
            string[] pts = spts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            PointF[] pa = new PointF[pts.Length / 2];
            int j=0;
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].X = Convert.ToSingle(pts[j++], NumberFormatInfo.InvariantInfo);
                pa[i].Y = Convert.ToSingle(pts[j++], NumberFormatInfo.InvariantInfo);
            }
            return pa;
        }
        private void ReadXmlPolygon(XmlReader xr)
        {
            // Read thru all the Polygon objects
            PointF[] pts = null;
            string[] keys = null;
            Color fill = Color.Empty;
            while (xr.Read())
            {
                if (!xr.IsStartElement())
                {
                    if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "Polygon")
                        break;
                    continue;
                }

                switch (xr.Name)
                {
                    case "Points":
                        pts = ReadXmlPoints(xr);
                        break;
                    case "Keys":
                        keys = ReadXmlKeys(xr);
                        break;
                    case "FillColor":
                        fill = ReadXmlColor(xr);
                        break;
                }

            }
            if (pts != null)
                AddPolygon(pts, keys, fill);
        }

        private string[] ReadXmlKeys(XmlReader xr)
        {
            string skeys = xr.ReadString();
            string[] keys = skeys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return keys;
        }

        private Color ReadXmlColor(XmlReader xr)
        {
            string sc = xr.ReadString();
            return XmlUtil.ColorFromHtml(sc, Color.Empty);
        }

        private void ReadXmlText(XmlReader xr)
        {
            // Read thru all the Text elements
            string val = null;
            PointF location= new PointF();
            string family = "Arial";
            float fontsize = 8;
            bool bBold = false;
            bool bItalic = false;
            bool bUnderline = false;
            bool bLineThrough = false;

            string temp;
            while (xr.Read())
            {
                if (!xr.IsStartElement())
                {
                    if (xr.NodeType == XmlNodeType.EndElement && xr.Name == "Text")
                        break;
                    continue;
                }

                switch (xr.Name)
                {
                    case "Location":
                        PointF[] pfa = ReadXmlPoints(xr);
                        location = pfa[0];
                        break;
                    case "Value":
                        val = xr.ReadString();
                        break;
                    case "FontFamily":
                        family = xr.ReadString();
                        break;
                    case "FontSize":
                        temp = xr.ReadString();
                        try
                        {
                            fontsize = float.Parse(temp, NumberFormatInfo.InvariantInfo);
                        }
                        catch
                        {
                            fontsize = 8;
                        }
                        break;
                    case "FontWeight":
                        temp = xr.ReadString();
                        bBold = temp.ToLower() == "bold";
                        break;
                    case "FontStyle":
                        temp = xr.ReadString();
                        bItalic = temp.ToLower() == "italic";
                        break;
                    case "TextDecoration":
                        temp = xr.ReadString();
                        switch (temp.ToLower())
                        {
                            case "underline":
                                bUnderline = true;
                                break;
                            case "linethrough":
                                bUnderline = true;
                                break;
                        }
                        break;
                }
            }
            if (val != null && location != null)
            {
                MapText mt = new MapText(val, location);
                mt.FontFamily = family;
                mt.FontSize = fontsize;
                mt.bBold = bBold;
                mt.bItalic = bItalic;
                mt.bLineThrough = bLineThrough;
                mt.bUnderline = bUnderline;
                AddMapObject(mt);
            }
        }

        /// <summary>
        /// Given a file name constructs a MapData
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static internal MapData Create(string file)
        {

            lock (_MapCache)
            {
                DateTime filetime = File.GetLastWriteTimeUtc(file);
                MapData mp = null;
                if (_MapCache.TryGetValue(file, out mp))
                {
                    if (mp._TimeStamp.Ticks == filetime.Ticks)
                        return mp;
                    _MapCache.Remove(file);
                    mp = null;
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(file, settings);
                try
                {
                    mp = new MapData(reader);
                    mp._TimeStamp = filetime;
                    _MapCache.Add(file, mp);        // add this to the cache
                }
                finally
                {
                    reader.Close();
                }
                return mp;
            }
        }
        /// <summary>
        /// this constructor for testing purposes it creates a rough 48 state US map
        /// </summary>
        internal MapData()
		{
            AddPolygon(new PointF[] { new PointF(338, 104), new PointF(339, 111), new PointF(344, 111), new PointF(344, 109) }, new string[] { "de" });
            AddPolygon(new PointF[] { new PointF(312, 106), new PointF(335, 102), new PointF(332, 112), new PointF(323, 105) }, new string[] { "md" });
            AddPolygon(new PointF[] { new PointF(355, 46), new PointF(358, 29), new PointF(364, 26), new PointF(367, 25), new PointF(371, 35), new PointF(378, 44), new PointF(372, 50), new PointF(369, 48), new PointF(368, 54), new PointF(360, 64) }, new string[] { "me" });
            AddPolygon(new PointF[] { new PointF(352, 49), new PointF(360, 65), new PointF(359, 67), new PointF(351, 67) }, new string[] { "nh" });
            AddPolygon(new PointF[] { new PointF(358, 76), new PointF(360, 77), new PointF(360, 80), new PointF(359, 80) }, new string[] { "ri" });
            AddPolygon(new PointF[] { new PointF(348, 78), new PointF(361, 77), new PointF(357, 81), new PointF(349, 85) }, new string[] { "ct" });
            AddPolygon(new PointF[] { new PointF(348, 72), new PointF(360, 68), new PointF(361, 72), new PointF(364, 74), new PointF(367, 73), new PointF(367, 76), new PointF(365, 77), new PointF(362, 78), new PointF(361, 74), new PointF(348, 77) }, new string[] { "ma" });
            AddPolygon(new PointF[] { new PointF(343, 53), new PointF(352, 50), new PointF(350, 70), new PointF(347, 69) }, new string[] { "vt" });
            AddPolygon(new PointF[] { new PointF(308, 86), new PointF(312, 80), new PointF(312, 75), new PointF(324, 72), new PointF(326, 68), new PointF(326, 63), new PointF(334, 54), new PointF(341, 54), new PointF(347, 71), new PointF(347, 85), new PointF(341, 85), new PointF(336, 81) }, new string[] { "ny" });
            AddPolygon(new PointF[] { new PointF(340, 86), new PointF(346, 86), new PointF(347, 96), new PointF(344, 104), new PointF(338, 100), new PointF(343, 96), new PointF(340, 92) }, new string[] { "nj" });
            AddPolygon(new PointF[] { new PointF(304, 88), new PointF(336, 81), new PointF(339, 86), new PointF(339, 91), new PointF(341, 94), new PointF(339, 100), new PointF(306, 106) }, new string[] { "pa" });
            AddPolygon(new PointF[] { new PointF(305, 107), new PointF(312, 107), new PointF(315, 109), new PointF(320, 106), new PointF(321, 107), new PointF(310, 124), new PointF(302, 127), new PointF(300, 125), new PointF(295, 119) }, new string[] { "wv" });
            AddPolygon(new PointF[] { new PointF(295, 136), new PointF(338, 126), new PointF(339, 119), new PointF(328, 110), new PointF(323, 107), new PointF(316, 115), new PointF(309, 126), new PointF(302, 129), new PointF(295, 132) }, new string[] { "va" });
            AddPolygon(new PointF[] { new PointF(304, 135), new PointF(334, 129), new PointF(343, 131), new PointF(345, 137), new PointF(330, 152), new PointF(319, 147), new PointF(312, 146), new PointF(305, 145), new PointF(298, 147), new PointF(289, 150) }, new string[] { "nc" });
            AddPolygon(new PointF[] { new PointF(296, 150), new PointF(303, 147), new PointF(310, 146), new PointF(312, 148), new PointF(319, 148), new PointF(328, 154), new PointF(315, 170) }, new string[] { "sc" });
            AddPolygon(new PointF[] { new PointF(280, 94), new PointF(287, 93), new PointF(290, 94), new PointF(303, 88), new PointF(305, 105), new PointF(295, 119), new PointF(281, 115), new PointF(277, 94), new PointF(283, 93) }, new string[] { "oh" });
            AddPolygon(new PointF[] { new PointF(267, 188), new PointF(313, 188), new PointF(331, 219), new PointF(328, 230), new PointF(321, 231), new PointF(313, 219), new PointF(305, 201), new PointF(295, 192), new PointF(283, 195), new PointF(280, 192), new PointF(270, 191) }, new string[] { "fl" });
            AddPolygon(new PointF[] { new PointF(279, 152), new PointF(294, 150), new PointF(313, 170), new PointF(311, 186), new PointF(307, 186), new PointF(287, 183) }, new string[] { "ga" });
            AddPolygon(new PointF[] { new PointF(261, 154), new PointF(277, 152), new PointF(285, 185), new PointF(267, 187), new PointF(269, 192), new PointF(263, 192) }, new string[] { "al" });
            AddPolygon(new PointF[] { new PointF(246, 156), new PointF(260, 155), new PointF(260, 191), new PointF(251, 192), new PointF(250, 188), new PointF(237, 187), new PointF(242, 175), new PointF(240, 169) }, new string[] { "ms" });
            AddPolygon(new PointF[] { new PointF(250, 143), new PointF(302, 135), new PointF(286, 150), new PointF(246, 155) }, new string[] { "tn" });
            AddPolygon(new PointF[] { new PointF(250, 140), new PointF(291, 135), new PointF(298, 126), new PointF(295, 120), new PointF(279, 116), new PointF(271, 126), new PointF(262, 129) }, new string[] { "ky" });
            AddPolygon(new PointF[] { new PointF(260, 96), new PointF(275, 95), new PointF(278, 118), new PointF(259, 129) }, new string[] { "in" });
            AddPolygon(new PointF[] { new PointF(240, 91), new PointF(257, 91), new PointF(258, 97), new PointF(258, 127), new PointF(252, 136), new PointF(235, 110) }, new string[] { "il" });
            AddPolygon(new PointF[] { new PointF(239, 56), new PointF(251, 48), new PointF(258, 53), new PointF(269, 53), new PointF(276, 56), new PointF(280, 61), new PointF(281, 77), new PointF(287, 75), new PointF(288, 83), new PointF(284, 91), new PointF(264, 94), new PointF(268, 85), new PointF(265, 73), new PointF(269, 67), new PointF(272, 60), new PointF(272, 57), new PointF(260, 60), new PointF(256, 64) }, new string[] { "mi" });
            AddPolygon(new PointF[] { new PointF(229, 56), new PointF(235, 54), new PointF(253, 63), new PointF(256, 70), new PointF(255, 90), new PointF(238, 87), new PointF(235, 81), new PointF(227, 72), new PointF(224, 64) }, new string[] { "wi" });
            AddPolygon(new PointF[] { new PointF(219, 173), new PointF(239, 174), new PointF(236, 188), new PointF(251, 190), new PointF(251, 194), new PointF(251, 200), new PointF(246, 203), new PointF(235, 199), new PointF(232, 201), new PointF(223, 199), new PointF(223, 189) }, new string[] { "la" });
            AddPolygon(new PointF[] { new PointF(216, 145), new PointF(244, 143), new PointF(245, 149), new PointF(238, 172), new PointF(219, 168) }, new string[] { "ar" });
            AddPolygon(new PointF[] { new PointF(208, 110), new PointF(233, 108), new PointF(250, 136), new PointF(247, 145), new PointF(243, 141), new PointF(215, 144), new PointF(216, 121) }, new string[] { "mo" });
            AddPolygon(new PointF[] { new PointF(202, 84), new PointF(236, 84), new PointF(237, 91), new PointF(240, 95), new PointF(235, 107), new PointF(209, 108) }, new string[] { "ia" });
            AddPolygon(new PointF[] { new PointF(200, 37), new PointF(210, 35), new PointF(241, 43), new PointF(227, 56), new PointF(225, 64), new PointF(224, 72), new PointF(234, 82), new PointF(203, 83) }, new string[] { "mn" });
            AddPolygon(new PointF[] { new PointF(159, 142), new PointF(177, 144), new PointF(175, 160), new PointF(190, 166), new PointF(217, 170), new PointF(222, 199), new PointF(195, 216), new PointF(194, 233), new PointF(179, 227), new PointF(166, 202), new PointF(156, 199), new PointF(150, 206), new PointF(140, 199), new PointF(139, 192), new PointF(128, 178), new PointF(153, 181) }, new string[] { "tx" });
            AddPolygon(new PointF[] { new PointF(157, 138), new PointF(213, 139), new PointF(215, 169), new PointF(206, 165), new PointF(186, 163), new PointF(176, 159), new PointF(176, 143), new PointF(155, 142) }, new string[] { "ok" });
            AddPolygon(new PointF[] { new PointF(166, 112), new PointF(209, 112), new PointF(214, 122), new PointF(214, 138), new PointF(162, 137) }, new string[] { "ks" });
            AddPolygon(new PointF[] { new PointF(155, 86), new PointF(202, 90), new PointF(209, 113), new PointF(165, 112), new PointF(165, 103), new PointF(151, 102) }, new string[] { "ne" });
            AddPolygon(new PointF[] { new PointF(156, 61), new PointF(200, 62), new PointF(202, 90), new PointF(188, 88), new PointF(154, 85) }, new string[] { "sd" });
            AddPolygon(new PointF[] { new PointF(159, 34), new PointF(197, 36), new PointF(200, 62), new PointF(158, 60) }, new string[] { "nd" });
            AddPolygon(new PointF[] { new PointF(121, 100), new PointF(165, 103), new PointF(163, 137), new PointF(116, 134) }, new string[] { "co" });
            AddPolygon(new PointF[] { new PointF(117, 134), new PointF(157, 137), new PointF(153, 180), new PointF(128, 177), new PointF(125, 180), new PointF(114, 177), new PointF(113, 181), new PointF(111, 181) }, new string[] { "nm" });
            AddPolygon(new PointF[] { new PointF(83, 128), new PointF(116, 134), new PointF(110, 181), new PointF(70, 163) }, new string[] { "az" });
            AddPolygon(new PointF[] { new PointF(92, 86), new PointF(109, 88), new PointF(107, 97), new PointF(121, 100), new PointF(115, 134), new PointF(84, 127) }, new string[] { "ut" });
            AddPolygon(new PointF[] { new PointF(113, 63), new PointF(155, 69), new PointF(151, 102), new PointF(108, 97) }, new string[] { "wy" });
            AddPolygon(new PointF[] { new PointF(91, 24), new PointF(159, 35), new PointF(155, 68), new PointF(114, 63), new PointF(112, 65), new PointF(102, 66) }, new string[] { "mt" });
            AddPolygon(new PointF[] { new PointF(53, 77), new PointF(91, 86), new PointF(81, 133), new PointF(75, 132), new PointF(73, 141), new PointF(48, 101) }, new string[] { "nv" });
            AddPolygon(new PointF[] { new PointF(86, 23), new PointF(90, 23), new PointF(89, 34), new PointF(95, 46), new PointF(93, 55), new PointF(97, 55), new PointF(101, 66), new PointF(112, 65), new PointF(108, 88), new PointF(73, 81) }, new string[] { "id" });
            AddPolygon(new PointF[] { new PointF(29, 69), new PointF(53, 76), new PointF(47, 101), new PointF(75, 144), new PointF(76, 149), new PointF(69, 162), new PointF(51, 159), new PointF(48, 147), new PointF(42, 143), new PointF(32, 136), new PointF(32, 131), new PointF(29, 117), new PointF(28, 108), new PointF(33, 105), new PointF(27, 103), new PointF(22, 91), new PointF(22, 81) }, new string[] { "ca" });
            AddPolygon(new PointF[] { new PointF(39, 36), new PointF(27, 69), new PointF(72, 80), new PointF(76, 62), new PointF(73, 62), new PointF(81, 52), new PointF(79, 47), new PointF(48, 44) }, new string[] { "or" });
            AddPolygon(new PointF[] { new PointF(53, 14), new PointF(54, 24), new PointF(49, 29), new PointF(46, 27), new PointF(48, 24), new PointF(50, 21), new PointF(43, 16), new PointF(40, 34), new PointF(45, 37), new PointF(47, 43), new PointF(79, 46), new PointF(85, 22), new PointF(53,14) }, new string[] { "wa" });
		    AddMapObject(new MapText("Map data failed to load.", new PointF(0,0)));
        }

        internal void AddMapObject(MapObject mo)
        {
            _MapObjects.Add(mo);
            _MaxX = Math.Max(_MaxX, mo.MaxX);
            _MaxY = Math.Max(_MaxY, mo.MaxY);
        }

        internal void AddPolygon(PointF[] polygon, string[] entries)
        {
            AddPolygon(polygon, entries, Color.Empty);
        }

        internal void AddPolygon(PointF[] polygon, string[] entries, Color fill)
        {
            MapPolygon mp = new MapPolygon(polygon);
            mp.Fill = fill;

            AddMapObject(mp);

            if (entries == null)
                return;

            // add the MapPolygon to the entries
            foreach (string key in entries)
            {
                List<MapPolygon> pl;
                if (!_DataPoints.TryGetValue(key, out pl))
                {
                    pl = new List<MapPolygon>();
                    _DataPoints.Add(key, pl);
                }
                pl.Add(mp);
            }
        }


        internal List<MapPolygon> GetPolygon(string key)
        {
            List<MapPolygon> mp = null;
            if (_DataPoints.TryGetValue(key, out mp))
                return mp;
            return null;
        }

        internal float GetScale(float width, float height)
        {
            return Math.Min(width / _MaxX , height / _MaxY);
        }

        internal float MaxX
        {
            get { return _MaxX; }
        }

        internal float MaxY
        {
            get { return _MaxY; }
        }

        internal List<MapObject> MapObjects
        {
            get { return _MapObjects; }
        }
    }

    internal abstract class MapObject
    {
        internal abstract float MaxX { get;}
        internal abstract float MaxY { get;}
        internal abstract void Draw(Graphics g, float scale, float offX, float offY);
    }

    internal class MapLines : MapObject
    {
        PointF[] _Lines = null;
        float _MaxX=0;
        float _MaxY=0;

        internal MapLines(PointF[] lines) : base()
        {
            _Lines = lines;
            foreach (PointF p in lines)
            {
                if (p.X > _MaxX)
                    _MaxX = p.X;
                if (p.Y > _MaxY)
                    _MaxY = p.Y;
            }
        }

        internal PointF[] Lines
        {
            get { return _Lines; }
        }

        internal override float MaxX
        {
            get { return _MaxX; }
        }
        internal override float MaxY
        {
            get { return _MaxY; }
        }
        internal override void Draw(Graphics g, float scale, float offX, float offY)
        {
            PointF[] poly = this.Lines;
            PointF[] drawpoly = new PointF[poly.Length];
            // make points relative to plot area --- need to scale this as well
            for (int ip = 0; ip < drawpoly.Length; ip++)
            {
                drawpoly[ip] = new PointF(offX + (poly[ip].X * scale), offY + (poly[ip].Y * scale));
            }
            g.DrawPolygon(Pens.Black, drawpoly);
        }
    }

    internal class MapPolygon : MapObject
    {
        PointF[] _Polygon = null;
        float _MaxX=0;
        float _MaxY=0;
        Color _Fill = Color.Empty;
        internal MapPolygon(PointF[] poly) : base()
        {
            PointF[] fpoly;
            if (poly[0].X == poly[poly.Length - 1].X &&
                poly[0].Y == poly[poly.Length - 1].Y)
                fpoly = poly;
            else
            {   // close the polygon;
                fpoly = new PointF[poly.Length + 1];
                for (int pi = 0; pi < poly.Length; pi++)
                    fpoly[pi] = poly[pi];
                fpoly[poly.Length] = fpoly[0];
            }
            _Polygon = fpoly;
            foreach (PointF p in _Polygon)
            {
                if (p.X > _MaxX)
                    _MaxX = p.X;
                if (p.Y > _MaxY)
                    _MaxY = p.Y;
            }

        }
        internal Color Fill
        {
            get { return _Fill; }
            set { _Fill = value; }
        }
        internal PointF[] Polygon
        {
            get { return _Polygon; }
        }
        internal override float MaxX
        {
            get { return _MaxX; }
        }
        internal override float MaxY
        {
            get { return _MaxY; }
        }
        internal override void Draw(Graphics g, float scale, float offX, float offY)
        {
            PointF[] poly = this.Polygon;
            PointF[] drawpoly = new PointF[poly.Length];
            // make points relative to plotarea --- need to scale this as well
            for (int ip = 0; ip < drawpoly.Length; ip++)
            {
                drawpoly[ip] = new PointF(offX + (poly[ip].X * scale), offY + (poly[ip].Y * scale));
            }
            
            g.DrawPolygon(Pens.Black, drawpoly);
            if (!Fill.IsEmpty)
            {
                Brush brush = new SolidBrush(Fill);
                g.FillPolygon(brush, drawpoly);
                brush.Dispose();
            }
        }
    }

    internal class MapText : MapObject
    {
        string _Text;              // text value
        PointF _Location;           // location of text
        public string FontFamily = "Arial";
        public float FontSize = 8;
        public bool bBold = false;
        public bool bItalic = false;
        public bool bUnderline = false;
        public bool bLineThrough = false;

        internal MapText(string val, PointF xy) : base()
        {
            _Text = val;
            _Location = xy;
        }

        internal string Text
        {
            get { return _Text; }
        }

        internal PointF Location
        {
            get { return _Location; }
        }

        internal override float MaxX
        {
            get { return _Location.X; }
        }
        internal override float MaxY
        {
            get { return _Location.Y; }
        }
        internal Font GetFont(float scale)
        {
            FontStyle fs = 0;

            if (bItalic)
                fs |= System.Drawing.FontStyle.Italic;
            if (bBold)
                fs |= System.Drawing.FontStyle.Bold;
            if (bUnderline)
                fs |= System.Drawing.FontStyle.Underline;
            if (bLineThrough)
                fs |= System.Drawing.FontStyle.Strikeout;
            float size = (float)Math.Max(FontSize * scale, .25);

            Font font;
            try     // when fontfamily is bad then sometimes there is an exception 
            {
                font = new Font(FontFamily, size, fs);
            }
            catch
            {
                font = new Font("Arial", size, fs);
            }
            return font;
        }

        internal override void Draw(Graphics g, float scale, float offX, float offY)
        {
            Font f = this.GetFont(scale);
            g.DrawString(this.Text, f, Brushes.Black, new PointF(offX + this.Location.X * scale, offY + this.Location.Y * scale));
            f.Dispose();
        }
    }
}
