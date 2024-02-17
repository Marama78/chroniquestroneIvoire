using DinguEngine.IO;
using DinguEngine.Shared;
using DinguEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.XML_manager
{
    public class Xml_Manager
    {
        string contentfilename = "Content.ini";
        public void WriteFileNamesINI(List<string> filenames, string intputArchive)
        {
            try
            {
                FileManager.DeleteEntryInZip(contentfilename, intputArchive);
            }
            catch
            {

            }


            string xmlRoot = "<Content></Content>";
            XDocument doc = XDocument.Parse(xmlRoot);
            XElement data = doc.Root;

            XElement cell = new XElement("ListNames");
            data.Add(cell);
            List<XElement> list = new List<XElement>();

            foreach (string filename in filenames)
            {
                list.Add(new XElement("Name", filename));

                list.ForEach(x => cell.Add(x));
            }

            byte[] xmlASByteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                xmlASByteArray = ms.ToArray();
            }
            FileManager.SaveEntryInZip(xmlASByteArray, contentfilename, intputArchive);

        }
        public List<string> ReadFileNamesINI(string archiveName)
        {


            string folder = Directory.GetCurrentDirectory() + "\\Data";
            string path_to_archiveName = folder + "\\" + archiveName + TE_Manager.customextension; ;

            List<string> result = new List<string>();
            XmlDocument doc = FileManager.LoadXMLfromZipFile(contentfilename, path_to_archiveName); //new XmlDocument();

            XmlNodeList nodeList = doc.SelectNodes("Content/ListNames");
            int gridW = 0;
            int gridH = 0;

            foreach (XmlNode item in nodeList)
            {
                foreach (XmlNode node in item)
                {
                   result.Add(node.InnerText);
                }
            }

            return result;
        }
        public void WriteINI(int gridW, int gridH, string filename, string intputArchive)
        {
            filename = filename + ".ini";
            try
            {
                FileManager.DeleteEntryInZip(filename, intputArchive);
            }
            catch
            {

            }


            string xmlRoot = "<Content></Content>";
            XDocument doc = XDocument.Parse(xmlRoot);
            XElement data = doc.Root;

            XElement cell = new XElement("GridParameters");
            data.Add(cell);
            List<XElement> list = new List<XElement>();

            list.Add(new XElement("gridW", gridW));
            list.Add(new XElement("gridH", gridH));

            list.ForEach(x => cell.Add(x));

            byte[] xmlASByteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                xmlASByteArray = ms.ToArray();
            }
            FileManager.SaveEntryInZip(xmlASByteArray, filename, intputArchive);

        }

        public void ReadINI(string filename, string archiveName, ref int2 output)
        {

            filename = filename + ".ini";

            XmlDocument doc = FileManager.LoadXMLfromZipFile(filename, archiveName); //new XmlDocument();

            XmlNodeList nodeList = doc.SelectNodes("Content/GridParameters");
            int gridW = 0;
            int gridH = 0;
            foreach (XmlNode item in nodeList)
            {
                foreach (XmlNode node in item)
                {
                    switch (node.Name)
                    {
                        case "gridW": gridW = int.Parse(node.InnerText); break;
                        case "gridH": gridH = int.Parse(node.InnerText); break;
                    }
                }
            }

            output = new int2(gridW, gridH);
        }

        public void WriteMap(ref TE_Button[] layer, string outputPath, string intputArchive)
        {

            string xmlRoot = "<Content></Content>";
            XDocument doc = XDocument.Parse(xmlRoot);
            XElement data = doc.Root;

            try {
            FileManager.DeleteEntryInZip(outputPath, intputArchive);
            }
            catch { }


            for (int i = 0; i < layer.Length; i++)
            {
                XElement cell = new XElement("Cell");
                data.Add(cell);
                List<XElement> list = new List<XElement>();

                int gridID = layer[i].drawrect.gridID;

                int horizontalFrames = layer[i].drawrect.horizontalFrames;
                int verticalFrames = layer[i].drawrect.verticalFrames;

                int positionX = layer[i].drawrect.DEFAULT_POSITION.X;
                int positionY = layer[i].drawrect.DEFAULT_POSITION.Y;
                int positionW = layer[i].drawrect.outputTileW;
                int positionH = layer[i].drawrect.outputTileH;

                int frameX = layer[i].drawrect.frame.X;
                    int frameY = layer[i].drawrect.frame.Y;
                    int framesize = layer[i].drawrect.frameSize;
                    int textureID = layer[i].drawrect.textureID;

                    string isanimated = layer[i].drawrect.isanimated;
                    string isloop = layer[i].drawrect.isloop;
                    string ispingpong = layer[i].drawrect.ispingpong;
                    string state = layer[i].drawrect.currentState;
                    int totalframes = layer[i].drawrect.totalframes;
                    float speedAnim = layer[i].drawrect.speedAnim;
                    float chronoAnim = layer[i].drawrect.chronoAnim;
                    float rotation = layer[i].drawrect.rotation;

                    list.Add(new XElement("gridID", gridID));

                    list.Add(new XElement("horizontalFrames", horizontalFrames));
                    list.Add(new XElement("verticalFrames", verticalFrames));

                list.Add(new XElement("positionX", positionX));
                    list.Add(new XElement("positionY", positionY));
                    list.Add(new XElement("positionW", positionW));
                    list.Add(new XElement("positionH", positionH));

                    list.Add(new XElement("frameX", frameX));
                    list.Add(new XElement("frameY", frameY));
                    list.Add(new XElement("framesize", framesize));
                    list.Add(new XElement("textureID", textureID));

                    list.Add(new XElement("isanimated", isanimated));
                    list.Add(new XElement("isloop", isloop));
                    list.Add(new XElement("ispingpong", ispingpong));
                    list.Add(new XElement("state", state));
                    list.Add(new XElement("totalframes", totalframes));
                    list.Add(new XElement("speedAnim", speedAnim));
                    list.Add(new XElement("chronoAnim", chronoAnim));
                    list.Add(new XElement("rotation", rotation));

                    list.ForEach(x => cell.Add(x));
               
            
            }

            byte[] xmlASByteArray;

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                xmlASByteArray = ms.ToArray();
            }
            FileManager.SaveEntryInZip(xmlASByteArray, outputPath, intputArchive);

        }

        public List<DrawRect> LoadMap(string filename, string archiveName)
        {
            XmlDocument doc = FileManager.LoadXMLfromZipFile(filename,archiveName); 
           // doc.Load(filepath);

            XmlNodeList nodeList = doc.SelectNodes("Content/Cell");


            List<DrawRect> output = new List<DrawRect>();
            
            if(nodeList.Count > 0 )
            {
            }
            else
            {
                return null;
            }

            int iteration = 0;
            foreach (XmlNode node in nodeList)
            {
                int gridID = 0;
                int horizontalFrames = 0;
                int verticalFrames = 0;
                int positionX = 0;
                int positionY = 0;
                int positionW = 0;
                int positionH = 0;

                string texture_name = "not_atribuated";
                int frameX = 0;
                int frameY = 0;
                int framesize = 0;
                int textureID = 0;
                string isanimated = "no";
                string isloop = "no";
                string ispingpong = "no";
                string state = "empty";
                int totalframes = 0;
                float speedAnim = 0;
                float chronoAnim = 0;
                float rotation = 0;

               

                foreach (XmlNode node2 in node.ChildNodes)
                {
                    switch(node2.Name)
                    {
                        case "gridID": gridID = int.Parse(node2.InnerText); break;
                        case "horizontalFrames": horizontalFrames = int.Parse(node2.InnerText); break;
                        case "verticalFrames": verticalFrames = int.Parse(node2.InnerText); break;
                        case "positionX": positionX = int.Parse(node2.InnerText); break;
                        case "positionY": positionY = int.Parse(node2.InnerText); break;
                        case "positionW": positionW = int.Parse(node2.InnerText); break;
                        case "positionH": positionH = int.Parse(node2.InnerText); break;
                        case "texture_name": texture_name = node2.InnerText; break;
                        case "frameX": frameX = int.Parse(node2.InnerText); break;
                        case "frameY": frameY = int.Parse(node2.InnerText); break;
                        case "framesize": framesize = int.Parse(node2.InnerText); break;
                        case "isanimated": isanimated = node2.InnerText; break;
                        case "isloop": isloop = node2.InnerText; break;
                        case "ispingpong": ispingpong = node2.InnerText; break;
                        case "state": state = node2.InnerText; break;
                        case "totalframes": totalframes = int.Parse(node2.InnerText); break;
                        case "textureID": textureID = int.Parse(node2.InnerText); break;
                        case "speedAnim": string format = node2.InnerText; speedAnim = float.Parse(format, CultureInfo.InvariantCulture); break;
                        case "chronoAnim":  format = node2.InnerText; chronoAnim = float.Parse(format, CultureInfo.InvariantCulture); break;
                        case "rotation":  format = node2.InnerText; rotation = float.Parse(format, CultureInfo.InvariantCulture); break;
                    }
                }
                iteration++;


                Rectangle position = new Rectangle(positionX,positionY,positionW,positionH);
                DrawRect temp = new DrawRect(position, framesize, rotation);
                temp.SetTextureID(textureID);
                temp.SetXMLFrame(frameX, frameY, framesize);
                temp.SetGridID(gridID);
               temp.SetOutputTilSize(positionW, positionH);
                
                temp.SetMultipleSelectionSize(horizontalFrames, verticalFrames);

                if (isanimated == "yes")
                {
                    temp.isanimated = "yes";
                    temp.isloop = isloop;
                    temp.ispingpong = ispingpong;
                    temp.totalframes = totalframes;
                    temp.speedAnim = speedAnim;
                    temp.chronoAnim = chronoAnim;
                }

                output.Add(temp);
            }

            return output;
        }
  
    }
}
