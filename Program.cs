using System;
using System.Linq;
using HtmlAgilityPack;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
//using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace HTMLreader
{
    partial class Program
    {
        static void Main(string[] args)
        {

            List<raport> Raport = new();
            int licznikPlikow=0;
            String kod_L="";
            int Duration=0;
            List<Lka> Wynik = new();
            List<string> listaTest2 = new();
            Console.WriteLine("Hello World!");
            string Ilosc="";

        //    var path = @"C:\\Dane\test.html";
		
            var doc = new HtmlDocument();
         //   var doc2 = new HtmlDocument();
          //  doc.Load(path);
            
        //   var node = doc.DocumentNode.SelectNodes("//div");

         //   node.ToList().ForEach(i=>Console.WriteLine(i.InnerText));

         //   var test1 = node.ToList();
            //TODO: POBRANIE TOWAROW
            List<towary> listaTowarow = new();
            towary.GetTowary(listaTowarow);
            //TODO: POBRANIE KKW L
            List<KKW> listaKKW = new();
            KKW.GetKKW(listaKKW);

            // Get list of files in the specific directory.
        // ... Please change the first argument.
            // var directories = Directory.GetDirectories(@"\\10.1.5.47\Serwer\DZ-Prod\Tech_Prog\programy\HYDRAULIK\2022\03\130_brak\","*.*",SearchOption.AllDirectories); //"Pakiet produkcyjny" && ".html"
            // List<string> directoriesAll=directories.ToList();
            //**************************************************************************************************************************************************************************************
            var directories = Directory.GetDirectories(@"\\10.1.5.47\Serwer\DZ-Prod\Tech_Prog\programy\Alstom RLH\2022\04\228_brak","*.*",SearchOption.AllDirectories);//.Where(X=>X.Contains(@"\2022\")); //"Pakiet produkcyjny" && ".html"
            List<string> directoreisFilter =directories.Where(X=>(X.Contains(@"\2021\12\") || X.Contains(@"\2022\"))).ToList();
         //   var directories2 = Directory.GetDirectories(@"\\10.1.5.47\Serwer\DZ-Prod\Tech_Prog\Programy_waterjet\","*.*",SearchOption.AllDirectories);//.Where(X=>X.Contains(@"\2022\")); //"Pakiet produkcyjny" && ".html"
         //   List<string> directoreis2Filter =directories2.ToList();

         //   List<string> directoriesAll=directoreisFilter.Concat(directoreis2Filter).ToList();

    
            //TODO: PEtla dla kazdej kolalizacji
            foreach(var elementLok in directoreisFilter)
            {
                String[] filesInDir = Directory.GetFiles(elementLok,"*.*",SearchOption.AllDirectories); //"Pakiet produkcyjny" && ".html"
                List<String> filsInDir2 =filesInDir.Where(X=>(X.ToUpper().Contains(".HTML"))).ToList();

                DirectoryInfo di = new DirectoryInfo(elementLok);
                FileInfo[] fiArray = di.GetFiles();
                //lista tylko z rozszerzeniem HTML
                var fiArray2=fiArray.Where(X=>X.FullName.ToUpper().Contains(".HTML"));
                fiArray2=fiArray2.OrderByDescending(X=>X.CreationTime);
                Array.Sort(fiArray, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.CreationTime, y.CreationTime));
                if(!fiArray2.Any())
                {
                    continue;
                }
               // string fullpath=Ar


            licznikPlikow=licznikPlikow+1;

            int ZnacznikSekcjiDetali=0;
            List<Lka> lstRecords=new List<Lka>();
            string plik=fiArray2.FirstOrDefault().FullName.ToString();
            doc.Load(fiArray2.FirstOrDefault().FullName.ToString());
           // doc.Load(path);
         //  string tsgfgf=doc.DocumentNode.OuterHtml;
        //   if(doc.DocumentNode.OuterHtml.Contains("//div"))
         //  {
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div"))
            {
                if(node.InnerText.Contains("Informacja o pojedynczym detalu:"))
                {
                    ZnacznikSekcjiDetali=1;
                }
                //WYŁĄCZENIE POBIERANIA DANYCH
                if(node.InnerText.Contains("Zlecenia produkcji:"))
                {
                    ZnacznikSekcjiDetali=0;
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
                        int znacznikSzt=0;
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

                            //ILOSC

                            if(znacznikSzt==1)
                            {
                                //przygotowanie danych o czasie w sekundach
                                Ilosc =wartosc;
                             //   record.
                                listaTest2.Add(Ilosc);
                                znacznikSzt=0;
                            }

                            if(wartosc.Contains("Szt.:"))
                            {
                                listaTest2.Add("SZt.:");
                                znacznikSzt=1;
                            }

                    }
                    }
                    }
                }  
                    if(ZnacznikSekcjiDetali==1 && Duration!=0)
                    {
                        if(kod_L=="probka")
                        {
                            kod_L="PROBA";
                        }
                       Wynik.Add(new Lka()
                        {
                            Name=kod_L,
                            Time=Convert.ToInt32(Duration*1.2),
                            LP=licznikPlikow,
                            ilosc=Ilosc

                        });  
                        kod_L="";
                        Duration=0;
                        Ilosc="";

                    } 
            }
   //         }
            }

            //pobierz listę numerów
            var licznikDoPetli = Wynik.Select(a=>a.LP).ToList().Distinct();
            foreach(var element in licznikDoPetli)
            {
                List<Lka> ListTempWynik = new();
                ListTempWynik=Wynik.Where(X=>X.LP==element).ToList();
                
                List<KKW> ListTempKKWl = listaKKW.Where(X=>X.Licznik_Elementow==ListTempWynik.Count()).ToList();
                //lista string towarow identyfikowanego pliku
              //   var listaTowarowPlik = ListTempKKWl.Select(a=>a.).ToList().Distinct();

                 //todo: LISTA Z PLIKU MASZYNOWEGO
                    List<string> towaryZpliku = new();
                    foreach(var elementPliku in ListTempWynik)
                    {
                        StringBuilder sb = new StringBuilder(elementPliku.Name.ToUpper());
                        sb.Append('_');
                        sb.Append(elementPliku.ilosc);
                        towaryZpliku.Add(sb.ToString());
                    }

                foreach(var element2 in ListTempKKWl)
                {
                    int kwh_idheadu=element2.kwh_idheadu;
                   
                    //TODO: LISTA Z KKW
                    //kody
                    List<String> towaryZkkw = new();
                    string[] arrayTemp = element2.towary.ToString().Split(';');
                    //ilosc
                     List<String> ilosciZkkw = new();
                    string[] arrayTempilosci = element2.ilosci.ToString().Split(';');

                    int licznik=0;
                     foreach(var elementKKW in arrayTemp)
                    {
                        StringBuilder sb = new StringBuilder(elementKKW.ToUpper());
                        sb.Append('_');
                        sb.Append(arrayTempilosci[licznik]);
                        towaryZkkw.Add(sb.ToString());

                        licznik=licznik+1;
                    }

                    //TODO: PORÓWNANIE LIST
                  //  var rowne = new HashSet<String>(set, StringComparer.OrdinalIgnoreCase);
                    var rowne = new HashSet<String>(towaryZpliku).SetEquals(towaryZkkw);

                    if(rowne)
                    {   
                        foreach(var elementP in ListTempWynik)
                        {
                            Raport.Add(new raport()
                            {
                                ttw_idtowaru=Convert.ToInt32(listaTowarow.Where(X=>X.ttw_klucz==elementP.Name).Select(a=>a.ttw_idtowaru).FirstOrDefault()),
                                kwh_idheadu=kwh_idheadu,
                                CzasLki=elementP.Time//Convert.ToInt32(ListTempWynik.Where(X=>X.Name==elementP.Name).Select(a=>a.Time)),
                            }); 
                        } 
                //    Wynik.Clear();
                        break;
                    }
                //    Console.WriteLine("");
                }
            }
            
            //TODO: ZAPISANIE RPORTU DO DB
            METODY.InsertRaportToDB(Raport);
            Console.WriteLine("KONIEC");

        }

        public class Lka
        {
            public string Name { get; set; }
            public int Time { get; set; } //w sekundach
            public int LP { get; set; } 
            public string ilosc{get;set;}

            // Lka(string name, double time)
            // {
            //     Name=name;
            //     Time=time;
            // }

        }
    }
}
