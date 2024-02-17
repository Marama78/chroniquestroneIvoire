using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.IO
{
    public static class FileManager
    {
        private static string filePath = Directory.GetCurrentDirectory()+"\\Data\\";// string.Empty;

        public static void SetFilePath(string _filePath)
        {
            filePath = _filePath;
        }

        public static void CreateNewZipFile(string filename)
        {
            string output = filePath + filename;

            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var demoFile = archive.CreateEntry("foo.txt");

                    using (var entryStream = demoFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write("Bar!");
                    }

                    using (var fileStream = new FileStream(output, FileMode.Create))
                    {
                        ms.CopyTo(fileStream);
                    }
                }
            }
        }

        public static void DeleteEntryInZip(string entryName, string _outputArchive)
        {
            if (entryName == string.Empty) return;
            if (_outputArchive == string.Empty) return;
            using (FileStream zipToOpen = new FileStream(_outputArchive, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    archive.Entries.Where(x => x.FullName.Contains(entryName)).ToList().ForEach(y =>
                    { archive.GetEntry(y.FullName).Delete(); });
                }
            }
        }

        public static void SaveEntryInZip(byte[] fileAsByte, string entryName, string _outputArchive)
        {
            if (fileAsByte == null) return;
            if (entryName == string.Empty) return;
            if (_outputArchive == string.Empty) return;
            using (FileStream zipToOpen = new FileStream(_outputArchive, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry newFile = archive.CreateEntry(entryName);
                    using (StreamWriter writer = new StreamWriter(newFile.Open()))
                    {
                        writer.BaseStream.Write(fileAsByte, 0, fileAsByte.Length);
                    }
                }
            }
        }


        public static List<string> GetNames(string _outputArchive)
        {
            if (_outputArchive == string.Empty) return null;
          
            List<string> names = new List<string>();

            using (FileStream zipToOpen = new FileStream(_outputArchive, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        Console.WriteLine(entry.FullName);
                        names.Add(entry.FullName.ToString());
                    }
                }
            }

            return names;
        }


        public static XmlDocument LoadXMLfromZipFile(string xmlFileName, string _outputArchive)
        {
            XmlDocument doc = new XmlDocument();
            using (var zip = ZipFile.OpenRead(_outputArchive))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.Name == xmlFileName)
                    {
                        using (var zipStream = entry.Open())
                        using (var memoryStream = new MemoryStream())
                        {
                            zipStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;
                            doc.Load(memoryStream);
                        }
                    }
                }
            }
            return doc;
        }
    }
}
