using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace VoxelMapImageStitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputDir;
            while (true)
            {
                Console.Write("地图切片文件路径：");
                inputDir = Console.ReadLine();
                if (Directory.Exists(inputDir))
                {
                    if (inputDir.Substring(inputDir.Length - 1, 1) != "\\") inputDir = inputDir + "\\";
                    if (Directory.GetFiles(inputDir, "*.png", SearchOption.TopDirectoryOnly).Length < 1) { Console.WriteLine("该路径下没有有效文件！"); }
                    else { break; }
                }
                else { Console.WriteLine("路径无效！"); }
            }
            string baseFN = string.Empty;
            string[] idxParts; int idxX, idxY;
            int minX = 0, minY = 0, maxX = 0, maxY = 0;
            foreach (string inputPath in Directory.GetFiles(inputDir, "*.png"))
            {
                baseFN = inputPath.Substring(inputPath.LastIndexOf("\\") + 1);
                baseFN = baseFN.Substring(0, baseFN.LastIndexOf("."));
                idxParts = baseFN.Split(',');
                idxX = int.Parse(idxParts[0]);
                idxY = int.Parse(idxParts[1]);
                if (minX > idxX) minX = idxX; if (minY > idxY) minY = idxY;
                if (maxX < idxX) maxX = idxX; if (maxY < idxY) maxY = idxY;
            }
            Bitmap outputBMP = new Bitmap((maxX - minX) * 256, (maxY - minY) * 256);
            Graphics outputGRP = Graphics.FromImage(outputBMP);
            outputGRP.Clear(Color.White);
            string currFXY = string.Empty;
            Image currIMG;
            for (int drawCX = 0; drawCX < maxX - minX + 1; drawCX++)
            {
                for (int drawCY = 0; drawCY < maxY - minY + 1; drawCY++)
                {
                    currFXY = (drawCX + minX).ToString() + "," + (drawCY + minY) + ".png";
                    if (File.Exists(inputDir + currFXY))
                    {
                        currIMG = Image.FromFile(inputDir + currFXY);
                        outputGRP.DrawImage(currIMG, drawCX * 256, drawCY * 256);
                        currIMG.Dispose();
                    }
                }
            }
            outputGRP.Dispose();
            outputBMP.Save(inputDir + "output.png", ImageFormat.Png);
        }
    }
}
