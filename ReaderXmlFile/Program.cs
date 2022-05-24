using System;
using System.Net;
using System.IO;
using System.Xml;

namespace ReaderXmlFile
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            string ForintXml = "";
            string NorwayCronXML = "";
            using (Stream stream = client.OpenRead("http://www.cbr.ru/scripts/XML_daily.asp"))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "Valute")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name == "ID")
                                            {
                                                if (reader.Value == "R01135")
                                                {
                                                    reader.MoveToElement();
                                                    ForintXml = reader.ReadOuterXml();
                                                }
                                            }
                                            if (reader.Name == "ID")
                                            {
                                                if (reader.Value == "R01535")
                                                {
                                                    reader.MoveToElement();
                                                    NorwayCronXML = reader.ReadOuterXml();
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            Console.WriteLine("Файл загружен");

            XmlDocument forintXmlDocument = new XmlDocument();
            forintXmlDocument.LoadXml(ForintXml);
            XmlDocument norwayCronXmlDocument = new XmlDocument();
            norwayCronXmlDocument.LoadXml(NorwayCronXML);

            XmlNode xmlNodeValue = forintXmlDocument.SelectSingleNode("Valute/Value");
            XmlNode xmlNodeNominal = forintXmlDocument.SelectSingleNode("Valute/Nominal");
            decimal forintValue = Convert.ToDecimal(xmlNodeValue.InnerText) / Convert.ToDecimal(xmlNodeNominal.InnerText);

            xmlNodeValue = norwayCronXmlDocument.SelectSingleNode("Valute/Value");
            xmlNodeNominal = norwayCronXmlDocument.SelectSingleNode("Valute/Nominal");
            decimal norwayCronValue = Convert.ToDecimal(xmlNodeValue.InnerText) / Convert.ToDecimal(xmlNodeNominal.InnerText);

            Console.WriteLine($"1 крон(а) --> {norwayCronValue} рубль(ей) --> {norwayCronValue / forintValue} форинт");

            Console.Read();
        }
    }
}
