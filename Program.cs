using System;
using System.Linq;
using HtmlAgilityPack;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
//using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;

namespace HTMLreader
{
    partial class Program
    {
        static void Main(string[] args)
        {
            int licznikPlikow=0;
            String kod_L="";
            int Duration=0;
            List<Lka> Wynik = new();
            List<string> listaTest2 = new();
            Console.WriteLine("Hello World!");

            var path = @"C:\\Dane\test.html";
		
            var doc = new HtmlDocument();
         //   var doc2 = new HtmlDocument();
            doc.Load(path);
            
        //   var node = doc.DocumentNode.SelectNodes("//div");

         //   node.ToList().ForEach(i=>Console.WriteLine(i.InnerText));

         //   var test1 = node.ToList();
            //TODO: POBRANIE TOWAROW
            List<towary> listaTowarow = new();
            towary.GetTowary(listaTowarow);
            //TODO: POBRANIE KKW L
            List<KKW> listaKKW = new();
            KKW.GetKKW(listaKKW);
            //TODO: PEtla dla kazdego pliku
            licznikPlikow=licznikPlikow+1;

            int ZnacznikSekcjiDetali=0;
            List<Lka> lstRecords=new List<Lka>();
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div"))
            {
                if(node.InnerText.Contains("Informacja o pojedynczym detalu:"))
                {
                    ZnacznikSekcjiDetali=1;
                }
                //WYŁĄCZENIE POBIERANIA DANYCH
                if(node.InnerText.Contains("Zlecenia produkcji:"))
                {
                    break;
                }
                  
                    var nodeTest=node.SelectNodes(".//tr");
                if( nodeTest!=null)
                {
                        int licznikTR=0;
                    foreach (HtmlNode node2 in node.SelectNodes(".//tr"))
                    {
                                 // wycagniecie nazwy pliki
                       if(ZnacznikSekcjiDetali==1)
                       {
                                nodeTest=node2.SelectNodes(".//a[@href]");
                        if( nodeTest!=null)
                        {
                        foreach(HtmlNode node5 in node2.SelectNodes(".//a[@href]"))
                        {
                            licznikTR=licznikTR+1;
                            if(licznikTR==2)
                            {
                               kod_L= node5.InnerHtml.ToString().Substring(0,node5.InnerHtml.ToString().Length-4);
                                listaTest2.Add(node5.InnerHtml);
                            }
                        }
                        }
                       }
                            nodeTest=node2.SelectNodes(".//td");
                            if( nodeTest!=null)
                    {
                        int znacznikCzasTrwania=0;
                         foreach (HtmlNode node3 in node2.SelectNodes(".//td"))
                    {       
                            string wartosc= (string)node3.InnerText;
                             if(znacznikCzasTrwania==1)
                            {
                                //przygotowanie danych o czasie w sekundach
                                List<string> tab = wartosc.Split(' ').ToList();
                                tab = tab.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                                int licznik=-1;
                                do 
                                {
                                    licznik=licznik+1;
                                    tab[licznik]= Regex.Replace(tab[licznik], "[^0-9]", "");
                                }while(licznik<tab.Count()-1);
                                TimeSpan duration = new TimeSpan(Convert.ToInt32(tab[0]),Convert.ToInt32(tab[1]),Convert.ToInt32(tab[2]));
                                Duration =Convert.ToInt32(duration.TotalSeconds);
                             //   record.
                                listaTest2.Add(wartosc);
                                znacznikCzasTrwania=0;
                            }
                            if(wartosc.Contains("Czas trwania:"))
                            {
                                listaTest2.Add("Czas trwania:");
                                znacznikCzasTrwania=1;
                            }
                    }
                    }
                    }
                }  
                    if(ZnacznikSekcjiDetali==1 && Duration!=0)
                    {
                       Wynik.Add(new Lka()
                        {
                            Name=kod_L,
                            Time=Convert.ToInt32(Duration*1.2),
                            LP=licznikPlikow
                        });  
                    } 
            }

            //pobierz listę numerów
            var licznikDoPetli = Wynik.Select(a=>a.LP).ToList().Distinct();
            foreach(var element in licznikDoPetli)
            {
                List<Lka> ListTempWynik = new();
                ListTempWynik=Wynik.Where(X=>X.LP==element).ToList();
                
                List<KKW> ListTempKKWl = listaKKW.Where(X=>X.Licznik_Elementow==ListTempWynik.Count()).ToList();



                Console.WriteLine("");

               // var a = ints1.All(ints2.Contains);


            }
            Console.WriteLine("KONIEC");

        }

        public class Lka
        {
            public string Name { get; set; }
            public int Time { get; set; } //w sekundach
            public int LP { get; set; } 

            // Lka(string name, double time)
            // {
            //     Name=name;
            //     Time=time;
            // }

        }
    }
}
