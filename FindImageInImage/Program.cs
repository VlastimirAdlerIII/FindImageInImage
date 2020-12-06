using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FindImageInImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Image<Bgr, byte> Image1 = new Image<Bgr, byte>(@"C:\Users\VA3\source\repos\FindImageInImage\FindImageInImage\_ikony.jpg"); //Your first image
            //Image<Bgr, byte> Image1 = new Image<Bgr, byte>(@"C:\Users\VA3\source\repos\FindImageInImage\FindImageInImage\_crop.png"); //Your first image

            int[,] big = new int[60, 60];
            //int[,] big = new int[6, 10];

            //go through icon types
            for (int cacheType = 0; cacheType <= 18; cacheType++)
            {
                Image<Bgr, byte> Image2 = new Image<Bgr, byte>($"C:\\Users\\VA3\\source\\repos\\FindImageInImage\\FindImageInImage\\{cacheType}.png"); //Your second image

                double Threshold = 0.9; //set it to a decimal value between 0 and 1.00, 1.00 meaning that the images must be identical

                //go through results for icon type
                Image<Gray, float> Matches = Image1.MatchTemplate(Image2, TemplateMatchingType.CcoeffNormed);
                for (int y = 0; y < Matches.Data.GetLength(0); y++)
                {
                    for (int x = 0; x < Matches.Data.GetLength(1); x++)
                    {
                        if (Matches.Data[y, x, 0] >= Threshold) //Check if its a valid match
                        {
                            //in big array, store the icon type found
                            int a = ((x + 11) / 19);
                            int b = ((y + 11) / 19);
                            big[b, a] = cacheType;
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Image search run for: {sw.ElapsedMilliseconds} ms");
            sw.Restart();


            Dictionary<string, int> result = new Dictionary<string, int>();

            string[,] small = new string[2, 6] { { "1", "E", "O", "16", "8", "12" }, { "14", "16", "I", "7", "17", "6" } };
            //string[,] small = new string[2, 3] { { "2", "3", "2" }, { "16", "X", "12" } };
            FindInArray(big, small, result);

            small = new string[2, 6] { { "6", "5", "H", "16", "K", "3" }, { "2", "0", "Y", "D", "11", "10" } };
            FindInArray(big, small, result);

            small = new string[2, 6] { { "L", "Á", "M", "15", "11", "0" }, { "4", "18", "3", "16", "N", "2" } };
            FindInArray(big, small, result);


            Console.WriteLine($"N 4{result["H"]}° {result["L"]}{result["E"]}.{result["D"]}{result["Á"]}{result["M"]}");
            Console.WriteLine($"E 016° {result["I"]}{result["K"]}.{result["O"]}{result["N"]}{result["Y"]}");

            sw.Stop();
            Console.WriteLine($"Result search run for: {sw.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }

        private static void FindInArray(int[,] big, string[,] small, Dictionary<string, int> result)
        {
            int i, j, a = 0, b = 0;
            int smallAsInt;
            bool isNum;
            //go through big array
            for (i = 0; i < big.GetLength(0) - small.GetLength(0); i++)
            {
                for (j = 0; j < big.GetLength(1) - small.GetLength(1); j++)
                {
                    //if first item of small array equals item of big array, check the rest of small array
                    isNum = int.TryParse(small[a, b], out smallAsInt);
                    if (!isNum || big[i, j] == smallAsInt)
                    {
                        for (a = 0; a < small.GetLength(0); a++)
                        {
                            isNum = int.TryParse(small[a, b], out smallAsInt);
                            if (isNum && big[i + a, j + b] != smallAsInt) { a = 0; b = 0; goto continueJ; };
                            for (b = 0; b < small.GetLength(1); b++)
                            {
                                isNum = int.TryParse(small[a, b], out smallAsInt);
                                if (isNum && big[i + a, j + b] != smallAsInt) { a = 0; b = 0; goto continueJ; };
                                //if we went through whole small array, we have result
                                if (a == small.GetLength(0) - 1 && b == small.GetLength(1) - 1)
                                {
                                    for (int c = 0; c < small.GetLength(0); c++)
                                    {
                                        for (int d = 0; d < small.GetLength(1); d++)
                                        {
                                            isNum = int.TryParse(small[c, d], out smallAsInt);
                                            if (!isNum)
                                            {
                                                Console.WriteLine($"{small[c, d]} = {big[i + c, j + d]}");
                                                result.Add(small[c, d], big[i + c, j + d]);
                                            }
                                        }
                                    }
                                }
                            }
                            b = 0;
                        }
                        a = 0;
                    }
                continueJ: continue;
                }
            }
        }
    }
}
