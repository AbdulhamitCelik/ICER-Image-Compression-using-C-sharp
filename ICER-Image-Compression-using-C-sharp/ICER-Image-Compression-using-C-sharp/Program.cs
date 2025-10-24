using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq.Expressions;
using System.IO;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Globalization;

namespace Impress
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while(!exit)
            {
                int algorithm = UserInterface.ChooseAlgorithm();
                string inputfilePath = UserInterface.GetInputFilePath();
                string outputfilePath = UserInterface.GetOutputFilePath(algorithm);
                bool compressState = UserInterface.ChooseCompressOrDecompress();
                switch (algorithm)
                {
                    case 1:
                        //ICER
                        double[] waveletFilterParameters = UserInterface.ChooseWaveletFilter();
                        int stages = UserInterface.GetNumberOfDecompositionStages();
                        int s = UserInterface.ChooseSegmentNo();
                        _ = new ICER(waveletFilterParameters, inputfilePath, outputfilePath, stages, compressState, s);
                        break;
                    case 2:
                        //JPEG
                        break;
                    case 3:
                        //WavPack
                        break;
                    case 4:
                        _ = new LZ78(compressState, inputfilePath, outputfilePath);
                        //LZ78
                        break;
                    case 5:
                        //RLE
                        _ = new RLE(compressState, inputfilePath, outputfilePath);
                        break;
                }
                exit = UserInterface.exit();
            }
        }
    }
    static class UserInterface
    {
        public static bool ChooseCompressOrDecompress()
        {
            bool exit = false;
            int choice = 0;
            bool output;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Choose Option: ");
                    Console.WriteLine("1 - Compress File");
                    Console.WriteLine("2 - Decompress File");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 1 || choice == 2)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter 1 or 2.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter 1 or 2.");
                }
            }

            if (choice == 1)
            {
                output = true;
            }
            else
            {
                output = false;
            }

            return output;
        }
        public static int ChooseSegmentNo()
        {
            bool exit = false;
            int choice = 0;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Choose number of segments (1 to 32 inclusive): ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice >= 1 && choice <= 32)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number from 1 to 32 inclusive.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number from 1 to 32 inclusive.");
                }
            }
            return choice;
        }
        public static int ChooseAlgorithm()
        {
            bool exit = false;
            int choice = 0;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Choose algorithm: ");
                    Console.WriteLine("1 - ICER Image Compression");
                    Console.WriteLine("2 - JPEG Image Compression");
                    Console.WriteLine("3 - WavPack Sound Compression");
                    Console.WriteLine("4 - LZ78 Text compression");
                    Console.WriteLine("5 - RLE Text Compression");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice >= 1 && choice <= 5)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number from 1 to 5 inclusive.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number from 1 to 5 inclusive.");
                }
            }
            return choice;
        }
        public static string GetInputFilePath()
        {
            bool exit = false;
            string path = "";
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Enter File Path (Right-Click file, choose 'copy as path' and paste): ");
                    Console.WriteLine("Do not include quotation marks.");
                    path = Console.ReadLine();

                    if (path.StartsWith(Convert.ToString((char)34)) || path.EndsWith(Convert.ToString((char)34)))
                    {
                        if (path.StartsWith(Convert.ToString((char)34))) path.Remove(path.IndexOf((char)34));
                        if (path.EndsWith(Convert.ToString((char)34))) path.Remove(path.LastIndexOf((char)34));
                    }

                    if (File.Exists(path) == true)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine(@"Please enter a valid file path. It must be the full path. e.g C:\Users\user\Downloads\filename.jpg");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(@"Please enter a valid file path. It must be the full path. e.g C:\Users\user\Downloads\filename.jpg");
                }
            }
            return path;
        }
        public static string GetOutputFilePath(int algorithm)
        {
            bool exit = false;
            string path = "";
            string outPath = "";
            string fileName = "";
            string changedFileName = "";
            string extension = "";
            int i = 1;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Enter Destination Folder's Path (Right-Click folder, choose 'copy as path' and paste): ");
                    Console.WriteLine("Do not include quotation marks.");
                    path = Console.ReadLine();
                    Console.WriteLine("Enter File Name (Do not include any special characters (@ # . $ % etc)): ");
                    fileName = Console.ReadLine();

                    if (!path.EndsWith(@"\"))
                    {
                        path = path + @"\";
                    }

                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo dir = Directory.CreateDirectory(path);
                    }
                    for (int j = 0; j < fileName.Length; j++)
                    {
                        if ((((int)fileName[j] >= 48 && (int)fileName[j] <= 57) || ((int)fileName[j] >= 65 && (int)fileName[j] <= 90) || ((int)fileName[j] >= 97 && (int)fileName[j] <= 122)))
                        {
                            changedFileName += fileName[j];
                        }
                    }

                    switch (algorithm)
                    {
                        case 1:
                            extension += ".bmp";
                            break;
                        case 2:
                            extension += ".bmp";
                            break;
                        case 3:
                            extension += ".WAV";
                            break;
                        case 4:
                            extension += ".txt";
                            break;
                        case 5:
                            extension += ".txt";
                            break;
                        default:
                            Console.WriteLine("Unknown Error");
                            break;
                    }

                    while (File.Exists(path + changedFileName + extension))
                    {
                        if(!File.Exists(changedFileName = changedFileName + "(" + Convert.ToString(i) + ")"))
                        {
                            changedFileName = changedFileName + "(" + Convert.ToString(i) + ")";
                        }
                        i++;
                    }

                    if (File.Exists(path + changedFileName + extension) == false)
                    {
                        outPath = path + changedFileName + extension;
                        exit = true;
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine(@"Please enter a valid file path and name. e.g C:\Users\user\Downloads\   and   myImage2");
                }
            }
            return outPath;
        }
        public static double[] ChooseWaveletFilter()
        {
            bool exit = false;
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine("Choose a wavelet filter parameter:");
                    Console.WriteLine("0 - filter A");
                    Console.WriteLine("1 - filter B");
                    Console.WriteLine("2 - filter C");
                    Console.WriteLine("3 - filter D");
                    Console.WriteLine("4 - filter E");
                    Console.WriteLine("5 - filter F");
                    Console.WriteLine("6 - filter Q");

                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice <= 6 && choice >= 0)
                    {
                        return ICER.waveletFilterParameters(choice);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number from 0 to 6 inclusive.");
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number from 0 to 6 inclusive.");
                }

            } while (!exit);

            return ICER.waveletFilterParameters(choice);
        }
        public static int GetNumberOfDecompositionStages()
        {
            bool exit = false;
            int choice = 0;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("After n stages, an image with dimensions W * H has dimensions W/(2^n) * H/(2^n). ");
                    Console.WriteLine("Number of stages to be done (3 to 6 recommended): ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice >= 0 && choice <= 10)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number from 0 to 10 inclusive.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number from 0 to 10 inclusive.");
                }
            }
            return choice;
        }
        public static bool exit()
        {
            bool exit = false;
            int choice = 0;
            bool output = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Would you like to exit program or continue?: ");
                    Console.WriteLine("1 - Continue program");
                    Console.WriteLine("2 - Exit program");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice >= 1 && choice <= 2)
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter 1 or 2.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter 1 or 2.");
                }
            }

            if (choice == 1)
            {
                output = false;
            }
            else
            {
                output = true;
            }

            return output;
        }
    } //Done?
    public class ICER
    {
        double[] waveletFilterParameter { get; set; }
        int stages { get; set; }
        string inPath { get; set; }
        int[,] x { get; set; }
        int s { get; set; }
        string outPath { get; set; }

        static int[][] mSet;
        static int[][,] sMMSet;
        static bool[][,][] sMFSSet;
        static int[][,] bands; // Results of Wavelet Transforms
        static int[][,] tempSegments;
        static int[][][,] segments; // Results of Wavelet Transform Segmentation
        static int[] means; // Mean Values of the Pixels of Segments
        static bool[][][,][] signMagnitudeFormSegments; // Segments in Binary Sign-Magnitude Form
        // signMagnitudeFormSegments[band][segment][,][sign magnitude binary array]
        //static bool[][][,][] subbandBitPlanes; // Segments Divided Into Planes by Bit Significance
        // subbandBitPlanes[band][segment][,][BitNo]
        static int[][][,] Category;
        // Category[band][segment][,] = category
        static int[][][,][] Contexts;
        // Contexts[band][segment][,] = [contexts]

        public ICER(double[] waveletFilterParameter, string inPath, string outPath, int stages, bool compress, int s)
        {
            this.s = s;
            this.inPath = inPath;
            this.outPath = outPath;
            this.stages = stages;
            this.waveletFilterParameter = waveletFilterParameter;
            this.x = GetSaveFile.GetImage(this.inPath);

            mSet = new int[s][];
            sMMSet = new int[s][,];
            sMFSSet = new bool[s][,][];

            bands = new int[3 * stages + 1][,];
            segments = new int[3 * stages + 1][][,];
            signMagnitudeFormSegments = new bool[3 * stages + 1][][,][];
            Category = new int[3 * stages + 1][][,];
            Contexts = new int[3 * stages + 1][][,][];


            for (int i = 0; i < 3 * stages + 1; i++)
            {
                Category[i] = new int[s][,];
                Contexts[i] = new int[s][,][];
            }
            // subbandBitPlanes[bit plane significances][bands][segments][pixel x, pixel y][bit]

            for (int i = 0; i < bands.Length; i++)
            {
                signMagnitudeFormSegments[i] = sMFSSet;
            }

            // clear memory
            mSet = null;
            sMMSet = null;
            sMFSSet = null;

            if (compress)
            {
                WaveletTransform2D(this.x, this.stages, 1, this.waveletFilterParameter);

                SegmentPartitioningAlgorithm(this.s);
                CalculateSegmentMeanLL();
                SubtractSegmentMeanFromSegmentLL();
                SignMagnitudeForm();
                AssignCategories();

                //SubbandBitPlanes();
                /* for (int i = 0; i < 3 * stages + 1; i++)
                {
                    for (int j = 0; j < this.s; j++)
                    {
                        GetSaveFile.SaveImage(segments[i][j], outPath + Convert.ToString(i) + "-" + Convert.ToString(j) + ".bmp");
                    }
                } */
                GetSaveFile.SaveImage(bands[bands.Length - 1], outPath);
                
            }
            if (!compress)
            {
                Console.WriteLine("In Progress");
                //outTextChar = Decompress(this.dict, this.inText);
                //RLE.SaveText(outTextChar, this.outPath);
            }
        }

        static void LosslessCompression()
        {

        }
        static void LossyCompression()
        {

        }
        public static double[] waveletFilterParameters(int x)
        {
            double[][] filterParameters = new double[7][];
            filterParameters[0] = [0, 0.25, 0.25, 0]; //A
            filterParameters[1] = [0, 2 / 8, 3 / 8, 2 / 8]; //B
            filterParameters[2] = [-1 / 16, 4 / 16, 8 / 16, 6 / 16]; //C
            filterParameters[3] = [0, 4 / 16, 5 / 16, 2 / 16]; //D
            filterParameters[4] = [0, 3 / 16, 8 / 16, 6 / 16]; //E
            filterParameters[5] = [0, 3 / 16, 9 / 16, 8 / 16]; //F
            filterParameters[6] = [0, 1 / 4, 1 / 4, 1 / 4]; //Q

            return filterParameters[x];
        } //Done
        static void WaveletTransform2D(int[,] x, int stages, int stage, double[] waveletFilterParameter)
        {
            (int[,] LL, int[,] LH, int[,] HL, int[,] HH) = WaveletTransform1D(x, waveletFilterParameter);

            if (stage == stages) bands[3 * stages] = LL;
            if (stages == 0) bands[3 * stages] = x;
            if (!(stages == 0))
            {
                bands[3 * stage - 1] = HL;
                bands[3 * stage - 2] = LH;
                bands[3 * stage - 3] = HH;
            }

            Console.WriteLine("Stage {0} done", Convert.ToString(stage));
            if (stages > stage) WaveletTransform2D(LL, stages, stage + 1, waveletFilterParameter);

        } //Done
        static (int[,], int[,], int[,], int[,]) WaveletTransform1D(int[,] x, double[] waveletFilterParameter)
        {
            (int[,] lowPassRows, int[] tempLPR) = LowPassFilterRows(x);
            int[,] highPassRows = HighPassFilterRows(x, tempLPR, waveletFilterParameter);
            //Columns
            (int[,] LL, int[] tempLL) = LowPassFilterColumns(lowPassRows);
            int[,] LH = HighPassFilterColumns(lowPassRows, tempLL, waveletFilterParameter);
            (int[,] HL, int[] tempHL) = LowPassFilterColumns(highPassRows);
            int[,] HH = HighPassFilterColumns(highPassRows, tempHL, waveletFilterParameter);

            return (LL, LH, HL, HH);
        } //Done
        static (int[,], int[]) LowPassFilterRows(int[,] x)
        {
            int N = x.Length;
            (int width, int height) = (x.GetLength(0), x.GetLength(1));
            if (width % 2 != 0) width -= 1;
            int lowN = N / 2; // floor n/2
            bool isOdd = false;
            int[,] lowPassRows = new int[width / 2, height];

            if (N % 2 != 0)
            {
                isOdd = true;
                lowN += 1; // flooring gives 1 less than it should so we add 1 if odd
            }

            int[] tempx = new int[N];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempx[j + (i * width)] = x[j, i];
                }
            }

            int[] templ = new int[lowN];

            for (int i = 0; i < lowN; i++)
            {
                if (i < lowN - 1 || i == lowN - 1 && isOdd == false)
                {
                    templ[i] = (int)Math.Floor(0.5 * (tempx[2 * i] + tempx[2 * i + 1]));
                }
                else if (i == lowN - 1 && isOdd == true)
                {
                    templ[i] = tempx[N - 1];
                }
            }

            for (int i = 0; i < width / 2; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    lowPassRows[i, j] = templ[i + j * (width / 2)];
                }
            }

            return (lowPassRows, templ);
        } //Done
        static int[,] HighPassFilterRows(int[,] x, int[] lowPassRows, double[] waveletFilterParameter)
        {
            bool isOdd = false;
            int N = x.Length;
            (int width, int height) = (x.GetLength(0), x.GetLength(1));
            if (width % 2 != 0) width -= 1;
            int highN = N / 2; // floor n/2
            int lowN = lowPassRows.Length;
            int[,] highPassRows = new int[width / 2, height];
            int[] tempx = new int[N];
            int[] temph = new int[highN];
            int[] d_highPassOutputs;
            int[] r_highPassOutputs;

            if (N % 2 != 0)
            {
                isOdd = true;
                r_highPassOutputs = new int[highN + 1];
                d_highPassOutputs = new int[highN + 1];
            }
            else
            {
                r_highPassOutputs = new int[highN];
                d_highPassOutputs = new int[highN];
            }

            //2d to 1d x
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempx[j + (i * width)] = x[j, i];
                }
            }

            //d[n]
            for (int i = 0; i < highN; i++)
            {
                if (i < (highN - 1) || (i == (highN - 1) && isOdd == false))
                {
                    d_highPassOutputs[i] = tempx[2 * i] - tempx[2 * i + 1];
                }
                else
                {
                    d_highPassOutputs[i] = 0;
                }

            }

            //r[n]
            r_highPassOutputs[0] = 1;
            for (int i = 1; i < lowN; i++)
            {
                r_highPassOutputs[i] = lowPassRows[i - 1] - lowPassRows[i];
            }

            //h[n]
            for (int i = 0; i < highN; i++)
            {
                if (i == 0)
                {
                    temph[i] = d_highPassOutputs[i] - (r_highPassOutputs[1]) / 4;
                }
                else if (i == 1 && waveletFilterParameter[0] != 0)
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(0.25 * (r_highPassOutputs[1]) / 4 + 0.375 * r_highPassOutputs[2] - 0.25 * d_highPassOutputs[2] + 0.5);
                }
                else if (isOdd == false && i == highN - 1)
                {
                    temph[i] = d_highPassOutputs[i] - r_highPassOutputs[(N / 2) - 1] / 4;
                }
                else
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(
                        waveletFilterParameter[0] * r_highPassOutputs[i - 1] + waveletFilterParameter[1] * r_highPassOutputs[i]
                        + waveletFilterParameter[2] * r_highPassOutputs[i + 1] - waveletFilterParameter[3] * d_highPassOutputs[i + 1] + 0.5);

                }
            }

            //1d to 2d outputs
            for (int i = 0; i < width / 2; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    highPassRows[i, j] = temph[i + j * (width / 2)];
                }
            }
            return highPassRows;
        } //Done
        static (int[,], int[]) LowPassFilterColumns(int[,] x)
        {
            int N = x.Length;
            (int width, int height) = (x.GetLength(0), x.GetLength(1));
            if (height % 2 != 0) height -= 1;
            int lowN = N / 2; // floor n/2
            bool isOdd = false;
            int[,] lowPassColumns = new int[width, height / 2];

            if (N % 2 != 0)
            {
                isOdd = true;
                lowN += 1; // flooring gives 1 less than it should so we add 1 if odd
            }

            int[] tempx = new int[N];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    try
                    {
                        tempx[i + j * height] = x[j, i];
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(Convert.ToString(i) + " " + Convert.ToString(width) + " " + Convert.ToString(j) + " " + Convert.ToString(height) + " " + Convert.ToString(i + (j * height)) + " " + Convert.ToString(N));
                    }

                }
            }

            int[] templ = new int[lowN];

            for (int i = 0; i < lowN; i++)
            {
                if (i < lowN - 1 || i == lowN - 1 && isOdd == false)
                {
                    templ[i] = (int)Math.Floor(0.5 * (tempx[2 * i] + tempx[2 * i + 1]));
                }
                else if (i == lowN - 1 && isOdd == true)
                {
                    templ[i] = tempx[N - 1];
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < (height / 2); j++)
                {
                    lowPassColumns[i, j] = templ[j + i * (height / 2)];
                }
            }

            return (lowPassColumns, templ);
        } //Done
        static int[,] HighPassFilterColumns(int[,] x, int[] lowPassColumns, double[] waveletFilterParameter)
        {
            bool isOdd = false;
            int N = x.Length;
            (int width, int height) = (x.GetLength(0), x.GetLength(1));
            if (height % 2 != 0) height -= 1;
            int highN = N / 2; // floor n/2
            int lowN = lowPassColumns.Length;
            int[,] highPassColumns = new int[width, height / 2];
            int[] tempx = new int[N];
            int[] temph = new int[highN];
            int[] d_highPassOutputs;
            int[] r_highPassOutputs;

            if (N % 2 != 0)
            {
                isOdd = true;
                r_highPassOutputs = new int[highN + 1];
                d_highPassOutputs = new int[highN + 1];
            }
            else
            {
                r_highPassOutputs = new int[highN];
                d_highPassOutputs = new int[highN];
            }

            //2d to 1d x
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempx[i + j * height] = x[j, i];
                }
            }

            //d[n]
            for (int i = 0; i < highN; i++)
            {
                if (i < (highN - 1) || (i == (highN - 1) && isOdd == false))
                {
                    d_highPassOutputs[i] = tempx[2 * i] - tempx[2 * i + 1];
                }
                else
                {
                    d_highPassOutputs[i] = 0;
                }

            }

            //r[n]
            r_highPassOutputs[0] = 1;
            for (int i = 1; i < lowN; i++)
            {
                r_highPassOutputs[i] = lowPassColumns[i - 1] - lowPassColumns[i];
            }

            //h[n]
            for (int i = 0; i < highN; i++)
            {
                if (i == 0)
                {
                    temph[i] = d_highPassOutputs[i] - (r_highPassOutputs[1]) / 4;
                }
                else if (i == 1 && waveletFilterParameter[0] != 0)
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(0.25 * (r_highPassOutputs[1]) / 4 + 0.375 * r_highPassOutputs[2] - 0.25 * d_highPassOutputs[2] + 0.5);
                }
                else if (isOdd == false && i == highN - 1)
                {
                    temph[i] = d_highPassOutputs[i] - r_highPassOutputs[(N / 2) - 1] / 4;
                }
                else
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(
                        waveletFilterParameter[0] * r_highPassOutputs[i - 1] + waveletFilterParameter[1] * r_highPassOutputs[i]
                        + waveletFilterParameter[2] * r_highPassOutputs[i + 1] - waveletFilterParameter[3] * d_highPassOutputs[i + 1] + 0.5);

                }
            }

            //1d to 2d outputs
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height / 2; j++)
                {
                    highPassColumns[i, j] = temph[j + i * (height / 2)];
                }
            }

            return highPassColumns;
        } //Done
        static int[,] HighPassFilterColumns(int[,] x, int[,] input, double[] waveletFilterParameter)
        {
            bool isOdd = false;
            int N = x.Length;
            (int width, int height) = (x.GetLength(0), x.GetLength(1));
            if (height % 2 != 0) height -= 1;
            int highN = N / 2; // floor n/2
            int lowN = input.Length;
            int[,] highPassColumns = new int[width, height / 2];
            int[] tempi = new int[lowN];
            int[] tempx = new int[N];
            int[] temph = new int[highN];
            int[] d_highPassOutputs;
            int[] r_highPassOutputs;

            if (N % 2 != 0)
            {
                isOdd = true;
                r_highPassOutputs = new int[highN + 1];
                d_highPassOutputs = new int[highN + 1];
            }
            else
            {
                r_highPassOutputs = new int[highN];
                d_highPassOutputs = new int[highN];
            }

            //2d to 1d x
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tempx[i + j * height] = x[j, i];
                }
            }

            //2d to 1d highPassColumns
            for (int i = 0; i < input.GetLength(1); i++)
            {
                for (int j = 0; j < input.GetLength(0); j++)
                {
                    tempi[i + j * input.GetLength(1)] = input[j, i];
                }
            }

            //d[n]
            for (int i = 0; i < highN; i++)
            {
                if (i < (highN - 1) || (i == (highN - 1) && isOdd == false))
                {
                    d_highPassOutputs[i] = tempx[2 * i] - tempx[2 * i + 1];
                }
                else
                {
                    d_highPassOutputs[i] = 0;
                }

            }
            Console.WriteLine("high d done");

            //r[n]
            r_highPassOutputs[0] = 1;
            for (int i = 1; i < lowN; i++)
            {
                r_highPassOutputs[i] = tempi[i - 1] - tempi[i];
            }
            Console.WriteLine("high r done");

            //h[n]
            for (int i = 0; i < highN; i++)
            {
                if (i == 0)
                {
                    temph[i] = d_highPassOutputs[i] - (r_highPassOutputs[1]) / 4;
                }
                else if (i == 1 && waveletFilterParameter[0] != 0)
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(0.25 * (r_highPassOutputs[1]) / 4 + 0.375 * r_highPassOutputs[2] - 0.25 * d_highPassOutputs[2] + 0.5);
                }
                else if (isOdd == false && i == highN - 1)
                {
                    temph[i] = d_highPassOutputs[i] - r_highPassOutputs[(N / 2) - 1] / 4;
                }
                else
                {
                    temph[i] = d_highPassOutputs[i] - (int)Math.Floor(
                        waveletFilterParameter[0] * r_highPassOutputs[i - 1] + waveletFilterParameter[1] * r_highPassOutputs[i]
                        + waveletFilterParameter[2] * r_highPassOutputs[i + 1] - waveletFilterParameter[3] * d_highPassOutputs[i + 1] + 0.5);

                }
            }
            Console.WriteLine("high 1d done");

            //1d to 2d outputs
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height / 2; j++)
                {
                    highPassColumns[i, j] = temph[j + i * (height / 2)];
                }
            }
            Console.WriteLine("high 2d done");

            return highPassColumns;
        } //Done
        static void SegmentPartitioningAlgorithm(int s)
        {
            int r = 0; int c = 0; int ht = 0; int rt = 0; int rt0 = 0; int yt = 0; int ct0 = 0; int xt = 0; int rb0 = 0; int yb = 0; int cb0 = 0; int xb = 0;
            int segment = 0;

            Console.WriteLine("Segment Partitioning Started");
            for (int i = 0; i < bands.Length; i++)
            {
                segment = 0;
                tempSegments = new int[s][,];
                int w = bands[i].GetLength(0); //w = width
                int h = bands[i].GetLength(1); // h = height
                //Compute r
                //If h > (s − 1)w, then r = s
                if (h > ((s - 1) * w)) r = s;
                //(r − 1)rw < hs ≤ (r + 1)rw
                else for (r = 1; (r < s) && (((r + 1) * r * w) < (h * s)); r++) ;
                //Compute c
                c = s / r;
                //Compute rt
                rt = (c + 1) * r - s;
                //Compute ht
                /*if (rt > (h * c * rt + s / 2) / s) ht = rt;
                else ht = (h * c * rt + s / 2) / s;*/
                ht = Math.Max(rt, (int)((h * c * rt) / s) + 1 / 2);
                //Compute xt
                xt = w / c;
                //Compute ct0
                ct0 = (xt + 1) * c - w;
                //Compute yt
                yt = ht / rt;
                //Compute rt0
                rt0 = (yt + 1) * rt - ht;

                if (rt < r)
                {
                    //Compute xb
                    xb = w / (c + 1);
                    //Compute cb0
                    cb0 = (xb + 1) * (c + 1) - w;
                    //Compute yb
                    yb = (h - ht) / (r - rt);
                    //Compute rb0
                    rb0 = (yb + 1) * (r - rt) - (h - ht);
                }
                Console.WriteLine("Partition variable values obtained for image {0}", Convert.ToString(i));
                //Console.WriteLine("r-{0} c-{1} ht-{2} rt-{3} rt0-{4} yt-{5} ct0-{6} xt-{7} rb0-{8} yb-{9} cb0-{10} xb-{11}", Convert.ToString(r), Convert.ToString(c), Convert.ToString(ht), Convert.ToString(rt), Convert.ToString(rt0), Convert.ToString(yt), Convert.ToString(ct0), Convert.ToString(xt), Convert.ToString(rb0), Convert.ToString(yb), Convert.ToString(cb0), Convert.ToString(xb));

                if (s <= bands[i].Length)
                {
                    for (int columns = 0; columns < rt; columns++)
                    {
                        for (int rows = 0; rows < c; rows++)
                        {
                            if (rows < ct0)
                            {
                                if (columns < rt0)
                                {
                                    GetSegment(i, xt, yt, (rows * xt), (columns * yt), (rows * xt + xt), (columns * yt + yt), segment);//
                                    Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                    segment++;
                                }
                                else
                                {
                                    GetSegment(i, xt, (yt + 1), (rows * xt), (columns * yt), (rows * xt + xt), (columns * yt + yt + 1), segment);//
                                    Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                    segment++;
                                }
                            }
                            else
                            {
                                if (columns < rt0)
                                {
                                    GetSegment(i, (xt + 1), yt, (rows * xt), (columns * yt), (rows * xt + xt + 1), (columns * yt + yt), segment);//
                                    Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                    segment++;
                                }
                                else
                                {
                                    GetSegment(i, (xt + 1), (yt + 1), (rows * xt), (columns * yt), (rows * xt + xt + 1), (columns * yt + yt + 1), segment);//
                                    Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                    segment++;
                                }
                            }
                        }
                    }
                    if (rt < r)
                    {
                        Console.WriteLine("Bottom Section:");
                        for (int columns = 0; columns < (r - rt); columns++)
                        {
                            for (int rows = 0; rows < (c + 1); rows++)
                            {
                                if (rows < cb0)
                                {
                                    if (columns < rb0)
                                    {
                                        //xt
                                        GetSegment(i, xb, yb, (rows * xb), (columns * yb + ht), (rows * xb + xb), (columns * yb + yb + ht), segment);
                                        Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                        segment++;
                                    }
                                    else
                                    {
                                        GetSegment(i, xb, (yb + 1), (rows * xb), (columns * yb + ht), (rows * xb + xb), (columns * yb + yb + ht + 1), segment);
                                        Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                        segment++;
                                    }
                                }
                                else
                                {
                                    if (columns < rb0)
                                    {
                                        GetSegment(i, (xb + 1), yb, (rows * xb), (columns * yb + ht), (rows * xb + xb + 1), (columns * yb + yb + ht), segment);
                                        Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                        segment++;
                                    }
                                    else
                                    {
                                        GetSegment(i, (xb + 1), (yb + 1), (rows * xb), (columns * yb + ht), (rows * xb + xb + 1), (columns * yb + yb + ht + 1), segment);
                                        Console.WriteLine("Band {1} Segment {0} Done", Convert.ToString(segment), Convert.ToString(i));
                                        segment++;
                                    }
                                }
                            }
                        }
                    }
                    segments[i] = tempSegments;
                }
            }

        } //Done
        static void GetSegment(int b, int x, int y, int startX, int startY, int endX, int endY, int segment)
        {
            Console.WriteLine("GetSegment Executed, segment {0}", Convert.ToString(segment));
            int[,] temp2D = new int[x, y];

            int k = 0;
            int l = 0;
            for (int j = startY; j < endY; j++, k++)
            {
                for (int i = startX; i < endX; i++, l++)
                {
                    temp2D[l, k] = bands[b][i, j];
                }
                l = 0;
            }
            tempSegments[segment] = temp2D;
        } //Done
        static void CalculateSegmentMeanLL()
        {
            means = new int[segments[segments.Length - 1].Length];

            for (int segment = 0; segment < segments[segments.Length - 1].Length; segment++)
            {
                int num = 0;

                for (int i = 0; i < segments[segments.Length - 1][segment].GetLength(0); i++)
                {
                    for (int j = 0; j < segments[segments.Length - 1][segment].GetLength(1); j++)
                    {
                        num += segments[segments.Length - 1][segment][i, j];
                    }
                }
                means[segment] = (num) / (segments[segments.Length - 1][segment].Length);
            }
        } //Done
        static void SubtractSegmentMeanFromSegmentLL()
        {
            for (int segment = 0; segment < segments[segments.Length - 1].Length; segment++)
            {
                int[,] temp = new int[segments[segments.Length - 1][segment].GetLength(0), segments[segments.Length - 1][segment].GetLength(1)];

                for (int i = 0; i < segments[segments.Length - 1][segment].GetLength(0); i++)
                {
                    for (int j = 0; j < segments[segments.Length - 1][segment].GetLength(1); j++)
                    {
                        temp[i, j] = segments[segments.Length - 1][segment][i, j] - means[segment];
                    }
                }
                segments[segments.Length - 1][segment] = temp;
            }
        } //Done
        static void SignMagnitudeForm()
        {
            Console.WriteLine("Converting to Sign-Magnitude Form");
            bool[] signMagnitude = new bool[32];
            int num;
            int count = 31;

            for (int band = 0; band < segments.Length; band++)
            {
                for (int segment = 0; segment < segments[band].Length; segment++)
                {
                    bool[,][] temp = new bool[segments[band][segment].GetLength(0), segments[band][segment].GetLength(1)][];
                    signMagnitudeFormSegments[band][segment] = new bool[(segments[band][segment].GetLength(0)), (segments[band][segment].GetLength(1))][];
                    for (int i = 0; i < segments[band][segment].GetLength(0); i++)
                    {
                        for (int j = 0; j < segments[band][segment].GetLength(1); j++)
                        {
                            //Console.WriteLine("band: {3} segment: {2} i: {1} j: {0}", Convert.ToString(j), Convert.ToString(i), Convert.ToString(segment), Convert.ToString(band));
                            num = segments[band][segment][i, j];
                            int numOriginal = segments[band][segment][i, j];
                            while (count > 0)
                            {
                                count = count - 1;
                                int two = (int)Math.Pow(2, count);
                                if (numOriginal < 0)
                                {
                                    signMagnitude[31] = true;

                                    if ((two + num) <= 0 && num != 0)
                                    {
                                        signMagnitude[count] = true;
                                        num += two;
                                    }
                                    else if((num + two) > 0 || num == 0)
                                    {
                                        signMagnitude[count] = false;
                                    }
                                }
                                else if(numOriginal >= 0)
                                {
                                    signMagnitude[31] = false;

                                    if ((num - two) >= 0 && num != 0)
                                    {
                                        signMagnitude[count] = true;
                                        num -= two;
                                    }
                                    else if((num - two) < 0 || num == 0)
                                    {
                                        signMagnitude[count] = false;
                                    }
                                }
                            }

                            /*//testing
                            Console.Write(Convert.ToString(segments[band][segment][i, j]) + "   ");
                            for (int a = signMagnitude.Length-1; a > -1; a--)
                            {
                                if (signMagnitude[a] == true)
                                {
                                    Console.Write("1");
                                }
                                else if (signMagnitude[a] == false)
                                {
                                    Console.Write("0");
                                }
                            }
                            Console.WriteLine();*/

                            temp[i, j] = signMagnitude;
                            signMagnitude = new bool[32];
                            count = 31;
                        }
                    }
                    signMagnitudeFormSegments[band][segment] = temp;
                }
            }

            // To clear some memory
            means = null;

        } //Done
        /* static void SubbandBitPlanes()
        {
            bool[] tempBitPlane = new bool[32];
            //static bool[][][,][] signMagnitudeFormSegments; // Segments in Binary Sign-Magnitude Form
            //static bool[][][,][] subbandBitPlanes; // Segments Divided Into Planes by Bit Significance
                                                   // subbandBitPlanes[band][segment][,][BitNo]
            //static bool[][][,][][][] Category;
            // Category[band][segment][,][category][BitNo][context]

            for (int band = 0; band < subbandBitPlanes.Length; band++)
            {
                for (int segment = 0; segment < subbandBitPlanes[band].Length; segment++)
                {
                    bool[,] temp = new bool[segments[band][segment].GetLength(0), segments[band][segment].GetLength(1)];
                    
                    subbandBitPlanes[band][segment] = new bool[(signMagnitudeFormSegments[band][segment].GetLength(0)), (signMagnitudeFormSegments[band][segment].GetLength(1))][];
                    for (int i = 0; i < subbandBitPlanes[band][segment].GetLength(0); i++)
                    {
                        for (int j = 0; j < subbandBitPlanes[band][segment].GetLength(1); j++)
                        {
                            for (int bitNo = 0; bitNo < 32; bitNo++)
                            {
                                //Console.WriteLine("band: {3} segment: {2} i: {1} j: {0}", Convert.ToString(j), Convert.ToString(i), Convert.ToString(segment), Convert.ToString(band));
                                for (int b = 0; b < signMagnitudeFormSegments[band][segment][i, j].Length; b++)
                                {
                                    temp[i, j] = signMagnitudeFormSegments[band][segment][i, j][b];

                                    subbandBitPlanes[band][segment][i, j] = temp;
                                }
                            }
                        }
                    }
                }
            }
        } //unneeded */
        static void AssignCategories()
        {
            Console.WriteLine("Assigning Categories");
            int tempCategory = 0;
            // Category[band][segment][,] = category
            // Contexts[band][segment][,] = [contexts]

            for (int band = 0; band < signMagnitudeFormSegments.Length; band++)
            {
                for (int segment = 0; segment < signMagnitudeFormSegments[band].Length; segment++)
                {
                    Category[band][segment] = new int[(signMagnitudeFormSegments[band][segment].GetLength(0)), (signMagnitudeFormSegments[band][segment].GetLength(1))];

                    for (int i = 0; i < signMagnitudeFormSegments[band][segment].GetLength(0); i++)
                    {
                        for (int j = 0; j < signMagnitudeFormSegments[band][segment].GetLength(1); j++)
                        {
                            //Console.WriteLine("band: {3} segment: {2} i: {1} j: {0}", Convert.ToString(j), Convert.ToString(i), Convert.ToString(segment), Convert.ToString(band));
                            for (int b = 0; b < signMagnitudeFormSegments[band][segment][i, j].Length; b++)
                            {
                                if(signMagnitudeFormSegments[band][segment][i, j][b] == true && tempCategory == 0)
                                {
                                    tempCategory = 1;
                                }
                                else if (tempCategory == 1)
                                {
                                    tempCategory = 2;
                                }
                                else if (tempCategory == 2)
                                {
                                    tempCategory = 3;
                                }
                            }
                            Category[band][segment][i,j] = tempCategory;
                            tempCategory = 0;
                        }
                    }
                }
            }
        } //Done
        static void AssignContexts()
        {
            Console.WriteLine("Assigning Contexts");
            int[] tempContexts = new int[32];
            // Contexts[band][segment][,] = [contexts]

            for (int band = 0; band < signMagnitudeFormSegments.Length; band++)
            {
                for (int segment = 0; segment < signMagnitudeFormSegments[band].Length; segment++)
                {
                    Contexts[band][segment] = new int[(signMagnitudeFormSegments[band][segment].GetLength(0)), (signMagnitudeFormSegments[band][segment].GetLength(1))][];

                    for (int i = 0; i < signMagnitudeFormSegments[band][segment].GetLength(0); i++)
                    {
                        for (int j = 0; j < signMagnitudeFormSegments[band][segment].GetLength(1); j++)
                        {
                            //Console.WriteLine("band: {3} segment: {2} i: {1} j: {0}", Convert.ToString(j), Convert.ToString(i), Convert.ToString(segment), Convert.ToString(band));

                            (int h, int v, int d) = (0, 0, 0);
                            // h is horizontally adjacent pixels
                            // v is vertically adjacent pixels
                            // d is diagonally adjacent pixels
                            // sections of array (also used for pixels where current pixel is 5)
                            // 1  2  3
                            // 4  5  6
                            // 7  8  9
                            // {d, v, d, h, h, d, v, d}
                            int[] adjacentPixelCategotries = {3, 3, 3, 3, 3, 3, 3, 3};

                            for(int p = -1; p < 2; p++)
                            {
                                for (int q = -1; q < 2; q++)
                                {
                                    try
                                    {
                                        if(p != 0 && q != 0)
                                        {
                                            if (p == -1)
                                            {
                                                adjacentPixelCategotries[q + p + 2] = Category[band][segment][i + p, i + q];
                                            }
                                            else if (p == 0)
                                            {
                                                adjacentPixelCategotries[q + p + 5] = Category[band][segment][i + p, i + q];
                                            }
                                            else if (p == 1)
                                            {
                                                adjacentPixelCategotries[q + p + 7] = Category[band][segment][i + p, i + q];
                                            }
                                        }
                                    }
                                    catch (IndexOutOfRangeException)
                                    {

                                    }
                                }
                            }
                            int[] diagonals = new int[4];
                            int[] horizontals = new int[2];
                            int[] verticals = new int[2];
                            horizontals[0] = adjacentPixelCategotries[3];
                            horizontals[1] = adjacentPixelCategotries[4];
                            verticals[0] = adjacentPixelCategotries[1];
                            verticals[1] = adjacentPixelCategotries[6];
                            diagonals[0] = adjacentPixelCategotries[0];
                            diagonals[1] = adjacentPixelCategotries[2];
                            diagonals[2] = adjacentPixelCategotries[5];
                            diagonals[3] = adjacentPixelCategotries[7];

                            for (int p = 0; p < 2; p++)
                            {
                                if (horizontals[p] != 3)
                                {
                                    h++;
                                }
                            }
                            for (int p = 0; p < 2; p++)
                            {
                                if (verticals[p] != 3)
                                {
                                    v++;
                                }
                            }
                            for (int p = 0; p < 4; p++)
                            {
                                if (diagonals[p] != 3)
                                {
                                    d++;
                                }
                            }

                            // Implement sign bit context after this for loop reminder
                            for (int b = 1; b < signMagnitudeFormSegments[band][segment][i, j].Length; b++)
                            {
                                //signMagnitudeFormSegments[band][segment][i, j][b]

                                if (band != signMagnitudeFormSegments.Length - 1)
                                {
                                    switch (band % 3)
                                    {
                                        case 0: // HH
                                            switch (Category[band][segment][i, j])
                                            {
                                                case 0:
                                                    if (d == 0)
                                                    {
                                                        if (h + v == 0)
                                                        {
                                                            tempContexts[b] = 0;
                                                        }
                                                        else if (h + v == 1)
                                                        {
                                                            tempContexts[b] = 1;
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 2;
                                                        }
                                                    }
                                                    //
                                                    else if (d == 1)
                                                    {
                                                        if (h + v == 0)
                                                        {
                                                            tempContexts[b] = 3;
                                                        }
                                                        else if (h + v == 1)
                                                        {
                                                            tempContexts[b] = 4;
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 5;
                                                        }
                                                    }
                                                    //
                                                    else if (d == 2)
                                                    {
                                                        if (h + v == 0)
                                                        {
                                                            tempContexts[b] = 6;
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 7;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 8;
                                                    }
                                                    break;
                                                case 1:
                                                    if (v == 0 && h == 0)
                                                    {
                                                        tempContexts[b] = 9;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 10;
                                                    }
                                                    break;
                                                case 2:
                                                    tempContexts[b] = 11;
                                                    break;
                                                case 3:
                                                    // do nothing
                                                    break;
                                            }
                                            break;
                                        case 1: // HL
                                            (h, v) = (v, h);

                                            switch (Category[band][segment][i, j])
                                            {
                                                case 0:
                                                    //
                                                    if (d == 0)
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if(v == 0)
                                                            {
                                                                tempContexts[b] = 0;
                                                            }
                                                            else if(v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if(h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 5;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    //
                                                    else if(d == 1)
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 1;
                                                            }
                                                            else if (v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if (h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 6;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    //
                                                    else
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 2;
                                                            }
                                                            else if (v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if (h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    break;
                                                case 1:
                                                    if (v == 0 && h == 0)
                                                    {
                                                        tempContexts[b] = 9;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 10;
                                                    }
                                                    break;
                                                case 2:
                                                    tempContexts[b] = 11;
                                                    break;
                                                case 3:
                                                    // do nothing
                                                    break;
                                            }
                                            break;
                                        case 2: // LH
                                            switch (Category[band][segment][i, j])
                                            {
                                                case 0:
                                                    //
                                                    if (d == 0)
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 0;
                                                            }
                                                            else if (v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if (h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 5;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    //
                                                    else if (d == 1)
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 1;
                                                            }
                                                            else if (v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if (h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 6;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    //
                                                    else
                                                    {
                                                        if (h == 0)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 2;
                                                            }
                                                            else if (v == 1)
                                                            {
                                                                tempContexts[b] = 3;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 4;
                                                            }
                                                        }
                                                        else if (h == 1)
                                                        {
                                                            if (v == 0)
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                            else
                                                            {
                                                                tempContexts[b] = 7;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tempContexts[b] = 8;
                                                        }
                                                    }
                                                    break;
                                                case 1:
                                                    if (v == 0 && h == 0)
                                                    {
                                                        tempContexts[b] = 9;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 10;
                                                    }
                                                    break;
                                                case 2:
                                                    tempContexts[b] = 11;
                                                    break;
                                                case 3:
                                                    // do nothing
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                else //LL
                                {
                                    switch (Category[band][segment][i, j])
                                    {
                                        case 0:
                                            //
                                            if (d == 0)
                                            {
                                                if (h == 0)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 0;
                                                    }
                                                    else if (v == 1)
                                                    {
                                                        tempContexts[b] = 3;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 4;
                                                    }
                                                }
                                                else if (h == 1)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 5;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 7;
                                                    }
                                                }
                                                else
                                                {
                                                    tempContexts[b] = 8;
                                                }
                                            }
                                            //
                                            else if (d == 1)
                                            {
                                                if (h == 0)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 1;
                                                    }
                                                    else if (v == 1)
                                                    {
                                                        tempContexts[b] = 3;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 4;
                                                    }
                                                }
                                                else if (h == 1)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 6;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 7;
                                                    }
                                                }
                                                else
                                                {
                                                    tempContexts[b] = 8;
                                                }
                                            }
                                            //
                                            else
                                            {
                                                if (h == 0)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 2;
                                                    }
                                                    else if (v == 1)
                                                    {
                                                        tempContexts[b] = 3;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 4;
                                                    }
                                                }
                                                else if (h == 1)
                                                {
                                                    if (v == 0)
                                                    {
                                                        tempContexts[b] = 7;
                                                    }
                                                    else
                                                    {
                                                        tempContexts[b] = 7;
                                                    }
                                                }
                                                else
                                                {
                                                    tempContexts[b] = 8;
                                                }
                                            }
                                            break;
                                        case 1:
                                            if (v == 0 && h == 0)
                                            {
                                                tempContexts[b] = 9;
                                            }
                                            else
                                            {
                                                tempContexts[b] = 10;
                                            }
                                            break;
                                        case 2:
                                            tempContexts[b] = 11;
                                            break;
                                        case 3:
                                            // do nothing
                                            break;
                                    }
                                }
                            }
                            Contexts[band][segment][i, j] = tempContexts;
                            tempContexts = new int[32];
                        }
                    }
                }
            }
        }

        static void ProbabilityOfZeroEstimation()
        {

        } //III. A
        static void ContextAssignment()
        {

        } //III.
        static void BitPlaneInterleaving()
        {

        } //III.
        static void BitPlaneEncoder()
        {

        }
        static void EntropyCoder()
        {

        } //IV
        static void ByteQuota()
        {

        }
        static void QualityGoal()
        {

        }

        static int[,] InverseWaveletTransform1D(int N, int width, int height, int[,] highPass, int[,] lowPass, int[] waveletFilterParameter)
        {
            int lwidth = lowPass.GetLength(0);
            int hwidth = highPass.GetLength(0);
            int lheight = lowPass.GetLength(1);
            int hheight = highPass.GetLength(1);

            if (lwidth % 2 != 0) lwidth -= 1;
            if (hwidth % 2 != 0) hwidth -= 1;
            if (lheight % 2 != 0) lheight -= 1;
            if (hheight % 2 != 0) hheight -= 1;

            int[] x = new int[N];
            int[] r = new int[N / 2 + (N % 2)];
            int[] d = new int[N / 2 + (N % 2)];

            int[] temph = new int[highPass.Length];
            int[] templ = new int[lowPass.Length];

            int[,] xOut = new int[width, height];

            bool isOdd = false;
            if (N % 2 != 0) isOdd = true;

            for (int i = 0; i < lwidth; i++)
            {
                for (int j = 0; j < lowPass.GetLength(1); j++)
                {
                    templ[i + j * lwidth] = lowPass[i, j];
                }
            }
            for (int i = 0; i < hwidth; i++)
            {
                for (int j = 0; j < highPass.GetLength(1); j++)
                {
                    temph[i + j * lwidth] = highPass[i, j];
                }
            }

            // r[n]
            for (int i = 0; i < templ.Length; i++)
            {
                r[i] = templ[i - 1] - templ[i];
            }

            // d[n]
            for (int i = temph.Length - 1; i > 0; i--)
            {
                if (i == 0)
                {
                    d[i] = temph[i] + (r[1]) / 4;
                }
                else if (i == 1 && waveletFilterParameter[0] != 0)
                {
                    d[i] = temph[i] + (int)Math.Floor(0.25 * (r[1]) / 4 + 0.375 * r[2] - 0.25 * d[2] + 0.5);
                }
                else if (isOdd == false && i == temph.Length - 1)
                {
                    d[i] = temph[i] + r[(N / 2) - 1] / 4;
                }
                else
                {
                    d[i] = temph[i] + (int)Math.Floor(waveletFilterParameter[0] * r[i - 1] + waveletFilterParameter[1] * r[i]
                        + waveletFilterParameter[2] * r[i + 1] - waveletFilterParameter[3] * d[i + 1] + 0.5);

                }
            }

            for (int i = 0; i < templ.Length; i++)
            {
                x[2 * i] = templ[i] + (d[i] + 1) / 2;
            }

            for (int i = 0; i < templ.Length; i++)
            {
                x[2 * i + 1] = x[i] - d[i];
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    xOut[i, j] = x[i + j * width];
                }
            }

            return xOut;

        }
        static void InverseWaveletTransform2D()
        {

        }
        static void BitPlaneDecoder()
        {

        }
    }
    public class GetSaveFile()
    {
        public static int[,] GetImage(string originalFilePath)
        {
            Console.WriteLine("Getting Image");
            int rgbValue;
            Color colour;
            Bitmap bmp = new(originalFilePath);
            (int originalImgWidth, int originalImgHeight) = (bmp.Width, bmp.Height);

            int[,] x = new int[originalImgWidth, originalImgHeight];

            for (int j = 0; j < originalImgHeight; j++)
            {
                for (int i = 0; i < originalImgWidth; i++)
                {
                    colour = bmp.GetPixel(i, j);
                    rgbValue = (int)Math.Round(0.299 * colour.R + 0.587 * colour.G + 0.114 * colour.B);
                    bmp.SetPixel(i, j, Color.FromArgb(rgbValue, rgbValue, rgbValue));
                    x[i, j] = rgbValue;
                }
            }
            return x;
        } 
        public static void SaveImage(int[,] outputFl, string outputFilePath)
        {
            Console.WriteLine("Saving Image");
            (int outputFlWidth, int outputFlHeight) = (outputFl.GetLength(0), outputFl.GetLength(1));
            Bitmap output = new(outputFlWidth, outputFlHeight);

            for (int i = 0; i < outputFlWidth; i++)
            {
                for (int j = 0; j < outputFlHeight; j++)
                {
                    if (outputFl[i, j] >= 0 && outputFl[i, j] <= 255)
                    {
                        output.SetPixel(i, j, Color.FromArgb(outputFl[i, j], outputFl[i, j], outputFl[i, j]));
                    }
                    else
                    {
                        int a = Math.Abs(outputFl[i, j] & 255);
                        output.SetPixel(i, j, Color.FromArgb(a, a, a));
                    }
                }
            }
            output.Save(outputFilePath);
        }
        public static char[] GetText(string path)
        {
            Console.WriteLine("Getting Text");
            List<char> list = new List<char>();
            int line = 0;

            using (StreamReader sr = new(path))
            {
                while ((line = sr.Read()) != -1)
                {
                    list.Add((char)line);
                }
            }

            char[] arr = new char[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i];
            }

            return arr;
        }
        public static void SaveText(char[] outputFl, string path)
        {
            Console.WriteLine("Saving Text");
            int length = outputFl.Length;

            if (File.Exists(path) == false)
            {
                var file = File.Create(path);
                file.Close();
            }

            using (StreamWriter sw = new(path))
            {
                for (int i = 0; i < length; i++)
                {
                    sw.Write(outputFl[i]);
                }
            }
        }
        public static void SaveText(string[] outputFl, string path)
        {
            Console.WriteLine("Saving Text");
            int length = outputFl.Length;

            if (File.Exists(path) == false)
            {
                var file = File.Create(path);
                file.Close();
            }

            using (StreamWriter sw = new(path))
            {
                for (int i = 0; i < length; i++)
                {
                    sw.Write(outputFl[i]);
                }
            }
        }
    } //Done
    public class RLE
    {
        private string inPath { get; set; }
        private string outPath { get; set; }
        private char[] inText { get; set; }
        private char[] outTextChar { get; set; }
        private string[] outTextStr { get; set; }
        private int[] inImage { get; set; }
        private int[] outImage { get; set; }
        public RLE(/*int type, */bool compress, string inPath, string outPath)
        {
            this.inPath = inPath;
            this.outPath = outPath;
            this.inText = GetSaveFile.GetText(this.inPath);

            /*
            switch(type)
            {
                case(0):
                    break;
                case(1):
                    break;
            }
            */

            if (compress)
            {
                outTextStr = CompressText(this.inText);
                GetSaveFile.SaveText(outTextStr, this.outPath);
            }
            if (!compress)
            {
                outTextChar = DecompressText(this.inText);
                GetSaveFile.SaveText(outTextChar, this.outPath);
            }
        }

        private static string[] CompressText(char[] arr)
        {
            List<string> compressed = new List<string>();
            List<char> temp = new List<char>();
            int[] percentages = new int[100];
            int[] numsToTen = new int[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }

            for (int i = 0; i <= arr.Length; i++)
            {
                try
                {
                    if (percentages.Contains((i * 100) / (arr.Length)))
                    {
                        Console.WriteLine("Compression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                        percentages[(i * 100) / (arr.Length)] = -1;
                    }
                    if (temp.Contains(arr[i]) == true)
                    {
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Count == 0) && (i < arr.Length))
                    {
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == true))
                    {
                        compressed.Add("`" + Convert.ToString(temp.Count) + "`" + Convert.ToString(temp[0]));
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == false))
                    {
                        compressed.Add("~" + Convert.ToString(temp.Count) + temp[0]);
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count <= 3) && (temp.Count > 0))
                    {
                        for (int j = 0; j < temp.Count; j++)
                        {
                            compressed.Add(Convert.ToString(temp[j]));
                        }
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                }
                catch (Exception)
                {
                    if (i == arr.Length)
                    {
                        /*if (temp.Count > 3)
                        {
                            compressed.Add("~" + Convert.ToString(temp.Count) + temp[0]);
                            temp.Clear();
                        }
                        else if ((temp.Count <= 3) && (temp.Count > 0))
                        {
                            for (int j = 0; j < temp.Count; j++)
                            {
                                compressed.Add(Convert.ToString(temp[j]));
                            }
                            temp.Clear();
                        }*/
                        if ((temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == true))
                        {
                            compressed.Add("`" + Convert.ToString(temp.Count) + "`" + Convert.ToString(temp[0]));
                            temp.Clear();
                            temp.Add(arr[i - 1]);
                        }
                        else if ((temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == false))
                        {
                            compressed.Add("~" + Convert.ToString(temp.Count) + temp[0]);
                            temp.Clear();
                            temp.Add(arr[i - 1]);
                        }
                    }
                }
            }
            temp.Clear();

            string[] compressedarr = new string[compressed.Count];

            for (int i = 0; i < compressed.Count; i++)
            {
                compressedarr[i] = compressed[i];
            }

            return compressedarr;
        }
        private static char[] DecompressText(char[] arr)
        {
            List<char> decompressed = new List<char>();
            int[] percentages = new int[100];
            int[] numsToTen = new int[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int count = 1;
            string num = "0";

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(Convert.ToString(i) + " " + Convert.ToString(arr[i]));
                if (percentages.Contains((i * 100) / (arr.Length)))
                {
                    Console.WriteLine("Decompression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                    percentages[(i * 100) / (arr.Length)] = -1;
                }
                if (arr[i] == '~')
                {
                    try
                    {
                        while (numsToTen.Contains((int)arr[i + count]) == true)
                        {
                            num += Convert.ToString(arr[i + count]);
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    int numi = Convert.ToInt32(num);
                    for (int j = 0; j < numi; j++)
                    {
                        decompressed.Add(arr[i + count]);
                    }
                    i += count;
                    count = 1;
                    num = "";
                }
                else if (arr[i] == '`')
                {
                    while (numsToTen.Contains((int)arr[i + count]) == true)
                    {
                        Console.WriteLine(arr[i + count]);
                        num += Convert.ToString(arr[i + count]);
                        count++;
                    }

                    int numi = Convert.ToInt32(num);
                    for (int j = 0; j < numi; j++)
                    {
                        decompressed.Add(arr[i + count + 1]);
                    }
                    i += count;
                    i++;
                    count = 1;
                    num = "";
                }
                else if (arr[i] != '~' && arr[i] != '`')
                {
                    decompressed.Add(arr[i]);
                }
            }

            char[] decompressedarr = new char[decompressed.Count];

            for (int i = 0; i < decompressed.Count; i++)
            {
                decompressedarr[i] = decompressed[i];
            }

            return decompressedarr;
        }
        /*private static string[] CompressImage(int[] arr)
        {
            List<string> compressed = new List<string>();
            List<char> temp = new List<char>();
            int[] percentages = new int[100];
            int[] numsToTen = new int[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }

            for (int i = 0; i <= arr.Length; i++)
            {
                try
                {
                    if (percentages.Contains((i * 100) / (arr.Length)))
                    {
                        Console.WriteLine("Compression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                        percentages[(i * 100) / (arr.Length)] = -1;
                    }
                    if (temp.Contains(arr[i]) == true)
                    {
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Count == 0) && (i < arr.Length))
                    {
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == true))
                    {
                        compressed.Add("`" + Convert.ToString(temp.Count) + "`" + Convert.ToString(temp[0]));
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == false))
                    {
                        compressed.Add("~" + Convert.ToString(temp.Count) + temp[0]);
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                    else if ((temp.Contains(arr[i]) == false) && (temp.Count <= 3) && (temp.Count > 0))
                    {
                        for (int j = 0; j < temp.Count; j++)
                        {
                            compressed.Add(Convert.ToString(temp[j]));
                        }
                        temp.Clear();
                        temp.Add(arr[i]);
                    }
                }
                catch (Exception)
                {
                    if (i == arr.Length)
                    {
                        if ((temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == true))
                        {
                            compressed.Add("`" + Convert.ToString(temp.Count) + "`" + Convert.ToString(temp[0]));
                            temp.Clear();
                            temp.Add(arr[i - 1]);
                        }
                        else if ((temp.Count > 3) && (numsToTen.Contains((int)temp[0]) == false))
                        {
                            compressed.Add("~" + Convert.ToString(temp.Count) + temp[0]);
                            temp.Clear();
                            temp.Add(arr[i - 1]);
                        }
                    }
                }
            }
            temp.Clear();

            string[] compressedarr = new string[compressed.Count];

            for (int i = 0; i < compressed.Count; i++)
            {
                compressedarr[i] = compressed[i];
            }

            return compressedarr;
        }
        private static char[] DecompressImage(int[] arr)
        {
            List<char> decompressed = new List<char>();
            int[] percentages = new int[100];
            int[] numsToTen = new int[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int count = 1;
            string num = "0";
            bool skip = false;

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(Convert.ToString(i) + " " + Convert.ToString(arr[i]));
                if (percentages.Contains((i * 100) / (arr.Length)))
                {
                    Console.WriteLine("Decompression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                    percentages[(i * 100) / (arr.Length)] = -1;
                }
                if (arr[i] == '~')
                {
                    try
                    {
                        while (numsToTen.Contains((int)arr[i + count]) == true)
                        {
                            num += Convert.ToString(arr[i + count]);
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    int numi = Convert.ToInt32(num);
                    for (int j = 0; j < numi; j++)
                    {
                        decompressed.Add(arr[i + count]);
                    }
                    i += count;
                    count = 1;
                    num = "";
                }
                else if (arr[i] == '`')
                {
                    while (numsToTen.Contains((int)arr[i + count]) == true)
                    {
                        Console.WriteLine(arr[i + count]);
                        num += Convert.ToString(arr[i + count]);
                        count++;
                    }

                    int numi = Convert.ToInt32(num);
                    for (int j = 0; j < numi; j++)
                    {
                        decompressed.Add(arr[i + count + 1]);
                    }
                    i += count;
                    i++;
                    count = 1;
                    num = "";
                }
                else if (arr[i] != '~' && arr[i] != '`')
                {
                    decompressed.Add(arr[i]);
                }
            }

            char[] decompressedarr = new char[decompressed.Count];

            for (int i = 0; i < decompressed.Count; i++)
            {
                decompressedarr[i] = decompressed[i];
            }

            return decompressedarr;
        }*/
    } //Done
    public class LZ78
    {
        private char[] inText { get; set; }
        private string inPath { get; set; }
        private string outPath { get; set; }
        private char[] outTextChar { get; set; }
        private string[] outTextStr { get; set; }

        static List<string> tempOutput = new List<string>();
        public LZ78(bool compress, string inPath, string outPath)
        {
            this.inPath = inPath;
            this.outPath = outPath;
            this.inText = GetSaveFile.GetText(this.inPath);

            if (compress)
            {
                this.outTextStr = Compress(this.inText);
                GetSaveFile.SaveText(outTextStr, this.outPath);
            }
            if (!compress)
            {
                outTextStr = Decompress(this.inText);
                GetSaveFile.SaveText(outTextStr, this.outPath);
            }
        }
        static string[] Compress(char[] arr)
        {
            int index = 0;
            bool inList = false;
            int[] percentages = new int[100];
            List<string> tempOutput = new List<string>();
            //List<byte> byteOutput = new List<byte>();

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }
            char[] numsToTen = new char[10] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            List<string[]> list = new List<string[]>(); //previous symbol index, value
            list.Add(null);
            for (int i = 0; i < arr.Length; i++)
            {
                if (percentages.Contains((i * 100) / (arr.Length)))
                {
                    Console.WriteLine("Compression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                    percentages[(i * 100) / (arr.Length)] = -1;
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (j != 0)
                    {
                        if (list[j][0] == Convert.ToString(index) && list[j][1] == Convert.ToString(arr[i]))
                        {
                            index = j;
                            inList = true;
                            break;
                        }
                    }
                    inList = false;
                }
                /*if (inList && i == (arr.Length-1))
                {
                    list.Add([Convert.ToString(index), ""]);
                }
                if(!inList)
                {
                    list.Add([Convert.ToString(index), Convert.ToString(arr[i])]);
                    tempOutput.Add("(" + Convert.ToString(index) + "," + Convert.ToString(arr[i]) + ")");
                    index = 0;
                }*/

                if (inList && i == (arr.Length - 1))
                {
                    list.Add([Convert.ToString(index), ""]);
                    tempOutput.Add(Convert.ToString(index) + "");
                }
                if (!inList)
                {
                    if (numsToTen.Contains(arr[i]))
                    {
                        list.Add([Convert.ToString(index), Convert.ToString(arr[i])]);
                        tempOutput.Add(Convert.ToString(index) +"~"+ Convert.ToString(arr[i])+"`");
                        //byteOutput.Add(Convert.ToByte(index));
                        //byteOutput.Add(Convert.ToByte('~'));
                        //byteOutput.Add(Convert.ToByte(arr[i]));
                        //byteOutput.Add(Convert.ToByte('`'));
                        index = 0;
                    }
                    else
                    {
                        list.Add([Convert.ToString(index), Convert.ToString(arr[i])]);
                        tempOutput.Add(Convert.ToString(index) + Convert.ToString(arr[i]));
                        //byteOutput.Add(Convert.ToByte(index));
                        //byteOutput.Add(Convert.ToByte(arr[i]));
                        index = 0;
                    }
                }
            }

            /*for (int i = 0; i < byteOutput.Count; i++)
            {
                string bits = Convert.ToString(byteOutput[i]).TrimStart('0');
                byteOutput[i] = Convert.ToByte(bits);
            }*/

            string[] output = new string[tempOutput.Count];
            for (int i = 0; i < tempOutput.Count; i++)
            {
                output[i] = tempOutput[i];
            }

            return output;
        }
        static string[] Decompress(char[] arr)
        {
            int count = 0;
            string num = "";
            int[] percentages = new int[100];
            List<string[]> list = new List<string[]>();
            char[] numsToTen = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < 100; i++)
            {
                percentages[i] = i;
            }

            list.Add(null);
            
            for (int i = 0; i < arr.Length; i++)
            {
                if (percentages.Contains((i * 100) / (arr.Length)))
                {
                    Console.WriteLine("Decompression " + Convert.ToString((i * 100) / (arr.Length) + "% done"));
                    percentages[(i * 100) / (arr.Length)] = -1;
                }

                while (numsToTen.Contains(arr[i + count]))
                {
                    num += arr[i + count];
                    count++;
                }

                i += count;
                count = 0;
                
                if (arr[i] == '~')
                {
                    list.Add([num, Convert.ToString(arr[i+1])]);
                    i += 2;
                }
                else
                {
                    list.Add([num, Convert.ToString(arr[i])]);
                }
                num = "";
            }

            for (int i = 1; i < list.Count; i++)
            {
                GetOriginalFromList(list, i);
            }

            string[] output = new string[tempOutput.Count];
            for (int i = 0; i < tempOutput.Count; i++)
            {
                output[i] = tempOutput[i];
            }

            return output;
        }
        static void GetOriginalFromList(List<string[]> list, int index)
        {
            //recursively get values of index 0, 1 .. n and write in list
            if (list[index][0] != "0")
            {
                GetOriginalFromList(list, Convert.ToInt32(list[index][0]));
            }
            tempOutput.Add(list[index][1]);


        }
    } //Done
}
