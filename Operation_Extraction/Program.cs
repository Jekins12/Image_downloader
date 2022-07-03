using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace Operation_Extraction
{
    class Program
    {
        static void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        public static String code(string Url)
        {
            WebClient web1 = new WebClient();
            string data = web1.DownloadString(Url);

            return data;
        }

        static void Main(string[] args)
        {

            string source_path = @"e:\start.txt";                       //path to txt with main album html code (need to scroll to the end to load, and then copy all html to txt)
            string store_path = "E:/ok1/";                        //path to destination folder
            string raw_first_html = File.ReadAllText(source_path);
            string[] first_url = raw_first_html.Split(' ');
            string[] second_url;
            string raw_second_html;
            int first_counter = 0;
            int second_counter = 0;

            int limit = 30;         
            bool noLimit = true;        //change to true, to download all

            for (int i = 0; i < first_url.Length; i++)
            {
                if (first_url[i].Contains("/profile/34499489xxxx/album/41627865xxxx"))         //path to specific profile Id and album Id
                {
                    first_counter++;
                    first_url[i] = first_url[i].Remove(0, 6);
                    first_url[i] = first_url[i].Remove(first_url[i].Length - 1, 1);
                    first_url[i] = "https://ok.ru" + first_url[i];


                    if (first_counter <= limit || noLimit)
                    {
                        second_counter++;
                        raw_second_html = code(first_url[i]);
                        second_url = raw_second_html.Split(' ');

                        for (int j = 0; j < second_url.Length; j++)
                        {

                            if (second_url[j].Contains("href=") && second_url[j].Contains("https://i.mycdn.me/i?r="))   //in this case, path to server where img are stored (after "=" unique Id of pic)
                            {

                                second_url[j] = second_url[j].Remove(0, 6);
                                second_url[j] = second_url[j].Remove(second_url[j].Length - 14, 14);


                                try
                                {
                                    SaveImage(second_url[j], store_path + second_counter + ".png", ImageFormat.Png);
                                    Console.WriteLine("Successfully downloaded #" + second_counter);
                                }

                                catch (Exception)
                                {
                                    Console.WriteLine("Failed to download pic #" + second_counter);
                                }
                            }
                        }
                    }

                    else
                        break; 
                }

            }
        }
    }
}
