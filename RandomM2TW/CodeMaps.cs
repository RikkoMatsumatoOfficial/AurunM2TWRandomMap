using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

public class StratMap
{
    public struct UInt24
    {
        public static explicit operator UInt24(int value)
        {
            return new UInt24(value);
        }
        public UInt24(int value)
        {
            _mostSignificant = (byte)(value >> 16);
            _leastSignificant = (ushort)value;
        }
        public readonly ushort _leastSignificant;
        public readonly byte _mostSignificant;
    }

    public struct TWPort
    {
        public int X, Y;
        public bool Is;
    };

    public struct TWReg
    {
        public Color C;
        public int X, Y;
        public bool Is;
        public TWPort Port;
    };

    public struct record
    {
        public int x, y;
        public double val;
        public int num;
        public int k;
        public int c;
    };

    public Image hmapfrompath;
    public bool hist = true;
    public int land_type = 0;
    public bool new_names = true;
    public int W, H;
    public double pw;
    public int r;
    public bool cP;
    public int money;
    public int RN;
    public int FN;
    public int RFN = 2;
    public int Forests = 200;
    public int Rivers = 80;
    public int SeasArea;
    public int maxLake = 300;
    public double[] PopOfRegs;
    public int[] FerOfRegs;
    public int[,] ClmOfRegs;
    public int[] MainClmOfRegs;
    public int[] AORsOfRegs;
    public int AORs;
    public int[] ClmOfAORs;
    public bool[,] RegsToRegs;
    public int[] RegsOfFacs;
    public int[] FacsOfRegs;
    int[,] OcMM;

    //climate params
    //public int clm_hmd_DD = 255;
    public int[] clm_hmd_Dw = { 255, 255, 255, 250, 150, 0, 0 };
    public int[] clm_hmd_De = { 0, 0, 0, 0, 0, 0, 0 };
    //public int clm_hmd_d = 10;
    public int clm_tmp_c = -20;
    public int clm_tmp_C = 255;

    public int NFA = 21;
    public bool[] FIs = {true,true,true,true,true,
                         true,true,true,true,true,
                         true,true,true,true,true,
                         true,true,true,true,true,true};
    public string[] Names = {"William","Philip","Heinrich","Alfonso","Domenico",
                             "Roger","Giorgio","Malcolm","Alexius","Ivan",
                             "Tahar","Selim","Mubarak","Knud","Henrique",
                             "Wladyslaw","Laszlo","Gregory","Montezuma","Batu","Timur"};
    public string[] Names_init = {"William","Philip","Heinrich","Alfonso","Domenico",
                             "Roger","Giorgio","Malcolm","Alexius","Ivan",
                             "Tahar","Selim","Mubarak","Knud","Henrique",
                             "Wladyslaw","Laszlo","Gregory","Montezuma","Batu","Timur"};
    public string[] egUnitNames;
    public bool garrs = false;
    public string[] Generals = {"NE Bodyguard","NE Bodyguard","NE Bodyguard","SE Bodyguard","SE Bodyguard",
                                "SE Bodyguard","SE Bodyguard","NE Bodyguard","Greek Bodyguard","EE Bodyguard",
                                "ME Bodyguard","ME Bodyguard","ME Bodyguard","NE Bodyguard","SE Bodyguard",
                                "NE Bodyguard","NE Bodyguard","SE Bodyguard","Aztec Bodyguard","Mongol Bodyguard","Mongol Bodyguard"};
    public Color[] FacClrs = { Color.FromArgb(215,0,0),Color.FromArgb(0,87,177),Color.FromArgb(125,105,0),Color.FromArgb(255,210,0), Color.FromArgb(121,3,3),
                               Color.FromArgb(70,70,70),Color.FromArgb(1,82,9),Color.FromArgb(0,16,94),Color.FromArgb(133,18,137),Color.FromArgb(12,30,119),
                               Color.FromArgb(254,120,0),Color.FromArgb(35,120,6),Color.FromArgb(221,176,69),Color.FromArgb(166,2,2),Color.FromArgb(200,200,255),
                               Color.FromArgb(239,139,139),Color.FromArgb(234,89,70),Color.FromArgb(255,255,255),Color.FromArgb(65,159,255),Color.FromArgb(244,231,170),Color.FromArgb(55,0,0)};
    public string[] Facs = {"england","france","hre","spain","venice",
                            "sicily","milan","scotland","byzantium","russia",
                            "moors","turks","egypt","denmark","portugal",
                            "poland","hungary","papal_states","aztecs","mongols","timurids"};
    public string[] FacsNames = {"england","france","hre","spain","venice",
                            "sicily","milan","scotland","byzantium","russia",
                            "moors","turks","egypt","denmark","portugal",
                            "poland","hungary","papal_states","aztecs","mongols","timurids"};
    public string[] ress_Names = { "gold", "silver", "fish", "furs", "timber", "iron",                   //0,1,2,3,4,5
                               "ivory", "wine", "chocolate", "marble", "textiles", "dyes", "tobacco", //6,7,8,9,10,11,12
                               "wool", "silk", "tin", "sulfur", "coal", "amber",                   //13,14,15,16,17,18
                               "elephants", "camels"};                                                      //19,20
    //public string[] ress_Names = { "gold", "silver", "pottery", "furs", "timber", "iron",                   //0,1,2,3,4,5
    //                           "olive_oil", "wine", "glass", "marble", "textiles", "purple_dye", "incense", //6,7,8,9,10,11,12
    //                           "wild_animals", "hides", "tin", "copper", "lead", "amber",                   //13,14,15,16,17,18
    //                           "elephants", "camels"};                                                      //19,20
    public string[] fb; public string[] lb; public string[] bb; public string[] aa;
    public string[][][] Letters;
    public bool lettersloaded = false;
    public bool[,] ress_Regs; int nRess = 21;
    public int[,] ress_Clm = { {2,2,1}, {2,2,1}, {8,5,4}, {4,4,1}, {4,4,5}, {2,2,1},
                             {0,0,0}, {8,2,7}, {0,0,0}, {9,10,11}, {9,8,7}, {9,9,8}, {0,9,10},
                             {2,9,10}, {0,0,9}, {2,2,2}, {2,2,2}, {2,2,2}, {4,4,5}, 
                             {0,0,9}, {10,10,11} };
    public int[,] ressMap;

    public int[] def_clms = { 8, 8, 8, 11, 9,  8, 6, 5, 4, 5,  8, 8, 8, 9, 5,  10, 6, 8, 7, 6, 6 };
    public int[] def_clms2 = { 8, 8, 8, 10, 8,  9, 9, 5, 4, 4,  8, 7, 2, 2, 7,  11, 6, 8, 8, 6, 6 };
    static Color SubArctic = Color.FromArgb(0, 166, 81);            //0
    static Color Alpine = Color.FromArgb(57, 181, 74);              //1
    static Color Highland = Color.FromArgb(141, 198, 63);           //2
    static Color Swamp = Color.FromArgb(255, 242, 0);               //3
    static Color ForestDeep = Color.FromArgb(247, 148, 29);         //4
    static Color ForestOpen = Color.FromArgb(242, 101, 34);         //5
    static Color GrasslandInfertile = Color.FromArgb(237, 28, 36);  //6
    static Color GrasslandFertile = Color.FromArgb(237, 20, 91);    //7
    static Color Mediterranian = Color.FromArgb(236, 0, 140);       //8
    static Color SemiArid = Color.FromArgb(0, 114, 188);            //9
    static Color SandyDesert = Color.FromArgb(102, 45, 145);        //10
    static Color RockyDesert = Color.FromArgb(146, 39, 143);        //11
    public Color[] Climates = {SubArctic, Alpine, Highland, Swamp, ForestDeep, ForestOpen, GrasslandInfertile, 
                                  GrasslandFertile, Mediterranian, SemiArid, SandyDesert, RockyDesert};
    public double[] vers = { 0.1, 0.05, 0.2, 0.1, 0.15, 0.7, 0.15, 0.45, 1.0, 0.3, 0.05, 0.1 };
    public double[] forest_vers = { 0.3, 0.8, 0.5, 0.0, 0.9, 0.9, 0.05, 0.4, 0.7, 0.07, 0.01, 0.03 };
    public double[] clmpopkf = { 0.0, 0.05, 0.3, 0.0, 0.3, 0.8, 0.2, 0.5, 1.0, 0.7, 0.05, 0.1 };

    public bool fautodef = true;

    public int[,] myHgtData;
    public double[,] RegHgtData;
    public int[,] OcData;
    public int[,] myHmdData;
    public int[,] myTmpData;
    public int[,] myFtrData;
    public int[,] myClmData;
    public int[,] AvMap;
    public TWReg[] Regs;
    public int[,] RegNum;
    public double[,] RegVal;
    public Color[,] myRegClr;
    public Color[,] myFacClr;
    public Color[,] myHgtClr;
    public Color[,] myGTpClr;
    public Color[,] myHmdClr;
    public Color[,] myTmpClr;
    public Color[,] myClmClr;
    public Color[,] MainClmClr;
    public Color[,] myRghClr;
    public Color[,] myFogClr;
    public Color[,] myFtrClr;
    public Color[,] myWSfClr;
    public Color[,] myTRtClr;
    public Color[,] myDisClr;
    public Color[,] myPopClr;
    public Color[,] myAvClr;
    public Color[,] myRdrClr1;
    public Color[,] myRdrClr2;
    public Color[,] myFEClr;
    public Color WaterC = Color.FromArgb(0, 0, 253);
    public Color OcColor = Color.FromArgb(41, 140, 233);
    public int hR = 10; //hill radius
    public int Dph = 25; //depth
    Random Rnd = new Random();
    //public Paloma.TargaImage tgaImage;
    FileStream FS;
    StreamWriter SW;
    //BinaryWriter BW;

    public void GenHgtsfrompath()
    {
        Bitmap Bmp = new Bitmap(hmapfrompath);
        int i, j;
        myHgtClr = new Color[2 * W + 1, 2 * H + 1];
        myHgtData = new int[2 * W + 1, 2 * H + 1];
        for (i = 0; i < hmapfrompath.Width; i++)
        {
            for (j = 0; j < hmapfrompath.Height; j++)
            {
                myHgtClr[i, j] = Bmp.GetPixel(i, j);
                if (myHgtClr[i, j] == WaterC)
                {
                    myHgtData[i, j] = 0;
                }
                else
                {
                    myHgtData[i, j] = (myHgtClr[i, j].R + myHgtClr[i, j].G + myHgtClr[i, j].B) / 3;
                }
            }
        }
        RegHgtData = new double[W, H];
        double m;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                m = ((double)myHgtData[2 * i, 2 * j] + myHgtData[2 * i + 1, 2 * j] +
                    myHgtData[2 * i, 2 * j + 1] + myHgtData[2 * i + 1, 2 * j + 1]) / 4 / 255;
                if (m >= 0)
                {
                    //RegHgtData[i, j] = Math.Pow(m, 0.5);
                    RegHgtData[i, j] = m;
                }
            }
        }
    }

    public void GenHgts1(int W, int H, ref int[,] A)
    {
        int rr = r; // smooth radius
        int sa = Dph; // sea areas percent
        int[,] a = new int[W, H];
        int i, j, k, l;
        //hgt map
        double[,] HgtMap = new double[W, H];
        int[,] hm = new int[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                HgtMap[i, j] = Rnd.Next(1000);
            }
        }
        //island
        if (land_type == 1)
        {
            double ik, jk;
            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    ik = (1.0 - Math.Sqrt(Math.Pow(((1.0 * i - 1.0 * W / 2) / W), 6)));    //odd power - mb like china, europe
                    jk = (1.0 - Math.Sqrt(Math.Pow(((1.0 * j - 1.0 * H / 2) / H), 6)));
                    ik = Math.Pow(ik, 0.9);
                    jk = Math.Pow(jk, 0.9);
                    HgtMap[i, j] = ((0.1 + 0.9 * ik) * HgtMap[i, j]);
                    HgtMap[i, j] = ((0.1 + 0.9 * jk) * HgtMap[i, j]);
                    if ((i <= 1) || (i >= W - 2)) HgtMap[i, j] = (0.2 * HgtMap[i, j]);
                    if ((j <= 1) || (j >= H - 2)) HgtMap[i, j] = (0.2 * HgtMap[i, j]);
                }
            }
        }
        if (land_type == 2)
        {
            double ik, jk;
            for (j = 0; j < H / 2; j++)
            {
                for (i = 0; i < W / 2; i++)
                {
                    ik = (1.0 * W / 2 - 1.0 * i) / W;
                    jk = (1.0 * H / 2 - 1.0 * j) / H;
                    ik = Math.Pow(ik, 0.5);
                    jk = Math.Pow(jk, 0.5);
                    double kk = ik + jk - Math.Sqrt(ik * jk);
                    HgtMap[i, j] = kk * HgtMap[i, j];
                    HgtMap[W - 1 - i, j] = kk * HgtMap[W - 1 - i, j];
                    HgtMap[i, H - 1 - j] = kk * HgtMap[i, H - 1 - j];
                    HgtMap[W - 1 - i, H - 1 - j] = kk * HgtMap[W - 1 - i, H - 1 - j];
                }
            }
        }
        if (land_type == 3)
        {
            double ik;
            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    ik = Math.Pow((1.0 * i) / W, 0.05);
                    HgtMap[i, j] = ((0.1 + 0.9 * ik) * HgtMap[i, j]);
                    if ((i <= 1)) HgtMap[i, j] = (0.2 * HgtMap[i, j]);
                }
            }
        }
        if (land_type == 4)
        {
            double ik;
            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    ik = Math.Pow((W - 1.0 * i) / W, 0.05);
                    HgtMap[i, j] = ((0.1 + 0.9 * ik) * HgtMap[i, j]);
                    if ((i >= W - 2)) HgtMap[i, j] = (0.2 * HgtMap[i, j]);
                }
            }
        }
        //smoothing
        int[,] kz = new int[W, H];
        double[,] aa = new double[W, H];
        kz[0, 0] = 0;
        int[] bfr = new int[rr + 1];
        i = 0; j = 0;
        for (l = 0; l <= rr; l++)
        {
            bfr[l] = 0;
            for (k = 0; k <= rr; k++)
            {
                if (k * k + l * l <= rr * rr)
                {
                    if ((k < W) && (l < H))
                    {
                        aa[0, 0] += HgtMap[k, l];
                        kz[0, 0]++;
                    }
                    bfr[l] = k;
                }
            }
        }

        for (j = 0; j < H; j++)
        {
            if (j > 0)
            {
                kz[0, j] = kz[0, j - 1];
                aa[0, j] = aa[0, j - 1];
                for (l = 0; l <= rr; l++)
                {
                    if ((j + bfr[l] < H) && (l < W))
                    {
                        aa[0, j] += HgtMap[l, j + bfr[l]];
                        kz[0, j]++;
                    }
                    if ((j - bfr[l] - 1 >= 0) && (l < W))
                    {
                        aa[0, j] -= HgtMap[l, j - bfr[l] - 1];
                        kz[0, j]--;
                    }
                }
            }

            for (i = 1; i < W; i++)
            {
                aa[i, j] = aa[i - 1, j];
                kz[i, j] = kz[i - 1, j];
                for (l = 0; l <= rr; l++)
                {
                    if ((i + bfr[l] < W) && (j + l < H))
                    {
                        aa[i, j] += HgtMap[i + bfr[l], j + l];
                        kz[i, j]++;
                    }
                    if ((i + bfr[l] < W) && (j - l >= 0) && (l > 0))
                    {
                        aa[i, j] += HgtMap[i + bfr[l], j - l];
                        kz[i, j]++;
                    }
                    if ((i - bfr[l] - 1 >= 0) && (j + l < H))
                    {
                        aa[i, j] -= HgtMap[i - bfr[l] - 1, j + l];
                        kz[i, j]--;
                    }
                    if ((i - bfr[l] - 1 >= 0) && (j - l >= 0) && (l > 0))
                    {
                        aa[i, j] -= HgtMap[i - bfr[l] - 1, j - l];
                        kz[i, j]--;
                    }
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                HgtMap[i, j] = aa[i, j] / kz[i, j];
            }
        }

        //adding seas
        double mind = 0, maxh = 0;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (HgtMap[i, j] > maxh)
                {
                    maxh = HgtMap[i, j];
                }
            }
        }
        int t = 0; //t/100
        int rsax = 0; double rsa = 0;
        while (rsa < 0.01 * sa)
        {
            t += 10;
            rsax = 0;
            for (i = 0; i < W; i++)
            {
                for (j = 0; j < H; j++)
                {
                    if (HgtMap[i, j] <= 0.01 * t * maxh)
                    {
                        rsax++;
                    }
                }
            }
            rsa = 1.0 * rsax / (W) / (H);
        }
        while (rsa > 0.01 * sa)
        {
            t -= 1;
            rsax = 0;
            for (i = 0; i < W; i++)
            {
                for (j = 0; j < H; j++)
                {
                    if (HgtMap[i, j] <= 0.01 * t * maxh)
                    {
                        rsax++;
                    }
                }
            }
            rsa = 1.0 * rsax / W / H;
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                HgtMap[i, j] -= maxh * t / 100;
            }
        }

        //deleting small islands and lakes
        for (i = 1; i < W - 1; i++)
        {
            for (j = 1; j < H - 1; j++)
            {
                kz[i, j] = 0;
                if (HgtMap[i - 1, j] > 0) kz[i, j]++;
                else kz[i, j]--;
                if (HgtMap[i + 1, j] > 0) kz[i, j]++;
                else kz[i, j]--;
                if (HgtMap[i, j - 1] > 0) kz[i, j]++;
                else kz[i, j]--;
                if (HgtMap[i, j + 1] > 0) kz[i, j]++;
                else kz[i, j]--;
            }
        }
        for (i = 1; i < W - 1; i++)
        {
            for (j = 1; j < H - 1; j++)
            {
                if (kz[i, j] * HgtMap[i, j] < 0)
                {
                    HgtMap[i, j] = (HgtMap[i, j] + HgtMap[i - 1, j] + HgtMap[i + 1, j] + HgtMap[i, j - 1] + HgtMap[i, j + 1]) / 5;
                }
            }
        }

        //valleyfication
        maxh = 0;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if ((HgtMap[i, j] > 0) && (HgtMap[i, j] > maxh))
                {
                    maxh = HgtMap[i, j];
                }
            }
        }
        double ii;
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                if (HgtMap[i, j] > 0)
                {
                    ii = Math.Pow((HgtMap[i, j] / maxh), pw);
                    HgtMap[i, j] = Math.Ceiling(maxh * ii);
                }

        //final normalization
        mind = 0; maxh = 0;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if ((HgtMap[i, j] < 0) && (HgtMap[i, j] < mind))
                {
                    mind = HgtMap[i, j];
                }
                if ((HgtMap[i, j] > 0) && (HgtMap[i, j] > maxh))
                {
                    maxh = HgtMap[i, j];
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (HgtMap[i, j] < 0)
                {
                    HgtMap[i, j] = -HgtMap[i, j] / mind * 255;
                }
                if (HgtMap[i, j] > 0)
                {
                    HgtMap[i, j] = HgtMap[i, j] / maxh * 255;
                }
            }
        }

        myHgtData = new int[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (HgtMap[i, j] < 0)
                {
                    myHgtData[i, j] = 0;
                }
                else
                {
                    myHgtData[i, j] = (int)HgtMap[i, j];
                    if (myHgtData[i, j] > 255) myHgtData[i, j] = 255;
                    if (myHgtData[i, j] < 1) myHgtData[i, j] = 1;
                }
            }
        }


        RegHgtData = new double[W / 2, H / 2];
        double m;
        for (i = 0; i < W / 2; i++)
        {
            for (j = 0; j < H / 2; j++)
            {
                m = ((double)myHgtData[2 * i, 2 * j] + myHgtData[2 * i + 1, 2 * j] +
                    myHgtData[2 * i, 2 * j + 1] + myHgtData[2 * i + 1, 2 * j + 1]) / 4 / 255;
                if (m >= 0)
                {
                    //RegHgtData[i, j] = Math.Pow(m, 0.5);
                    RegHgtData[i, j] = m;
                }
            }
        }
    }

    public void HgtToClr(int W, int H, int[,] A, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (j = 0; j < H; j++)
            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    if (A[i, j] > 0)
                    {
                        C[i, j] = Color.FromArgb(A[i, j], A[i, j], A[i, j]);
                    }
                    else
                    {
                        C[i, j] = WaterC;
                    }
                }
            }
    }

    public void HgtToGTpClr(int W, int H, int[,] A, ref Color[,] C)
    {
        //
        int i, j;
        C = new Color[W, H];
        for (j = 0; j < H; j++)
        {
            for (i = 0; i < W; i++)
            {
                if (A[i, j] > 120)
                {
                    C[i, j] = Color.FromArgb(196, 128, 128);
                }
                else if (A[i, j] > 80)
                {
                    C[i, j] = Color.FromArgb(98, 65, 65);
                }
                else if (A[i, j] > 25)
                {
                    //if (Rnd.Next(10) == 0)
                    //    C[i, j] = Color.FromArgb(0, 128, 0);
                    //else
                    C[i, j] = Color.FromArgb(128, 128, 64);
                }
                else if (A[i, j] > 0)
                {
                    //if (Rnd.Next(5) == 0)
                    //    C[i, j] = Color.FromArgb(0, 128, 0);
                    //else
                    C[i, j] = Color.FromArgb(0, 0, 0);
                }
                else
                {
                    C[i, j] = Color.FromArgb(128, 0, 0);
                }
            }
        }
        OcData = new int[W, H];
        // 0 - lake, 1 - ocean, 2 - sea
        int z = Math.Min(W, H) / 2;
        Color Oc = Color.FromArgb(64, 0, 0);
        //int bRR = 10;
        //for (i = 0; i < W; i++)
        //{
        //    for (j = 0; j < H; j++)
        //    {
        //        if ((W / 2 - i) * (W / 2 - i) + (H / 2 - j) * (H / 2 - j) >= (z - bRR) * (z - bRR))
        //        {
        //            //C[i, j] = Oc;
        //            OcData[i, j] = 1;
        //        }
        //    }
        //}
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                if (myHgtData[i, j] == 0)
                    if (cP)
                    {
                        OcData[i, j] = 2;
                    }
                    else
                    {
                        OcData[i, j] = 1;
                    }

        //lakes
        if (cP)
        {
            OcMM = new int[W, H];
            int m = 1;
            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    if (myHgtData[i, j] == 0)
                    {
                        if (OcMM[i, j] == 0)
                        {
                            if (i > 0)
                            {
                                if (OcMM[i - 1, j] > 0)
                                {
                                    OcFloodFill(i, j, OcMM[i - 1, j], 0);
                                }
                                else if (j > 0)
                                {
                                    if (OcMM[i, j - 1] > 0)
                                    {
                                        OcFloodFill(i, j, OcMM[i, j - 1], 0);
                                    }
                                    else
                                    {
                                        OcFloodFill(i, j, m, 0);
                                        m++;
                                    }
                                }
                                else
                                {
                                    OcFloodFill(i, j, m, 0);
                                    m++;
                                }
                            }
                            else
                            {
                                OcFloodFill(i, j, m, 0);
                                m++;
                            }
                        }
                    }
                }
            }
            int[] mL = new int[m];
            for (i = 0; i < W; i++)
            {
                for (j = 0; j < H; j++)
                {
                    if (OcMM[i, j] != 0)
                    {
                        mL[OcMM[i, j]]++;
                    }
                }
            }
            for (i = 0; i < W; i++)
            {
                for (j = 0; j < H; j++)
                {
                    if (mL[OcMM[i, j]] < maxLake)
                    {
                        OcData[i, j] = 1;
                    }
                }
            }
        }

        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myHgtData[i, j] == 0)
                {
                    if (OcData[i, j] < 2)
                    {
                        C[i, j] = Oc;
                    }
                }
            }
        }
    }

    public void OcFloodFill(int x, int y, int q, int k)
    {
        if (k < 1000)
        {
            OcMM[x, y] = q;
            if (x > 0)
            {
                if (myHgtData[x - 1, y] == 0)
                {
                    if (OcMM[x - 1, y] == 0)
                    {
                        OcFloodFill(x - 1, y, q, k + 1);
                    }
                }
            }
            if (y > 0)
            {
                if (myHgtData[x, y - 1] == 0)
                {
                    if (OcMM[x, y - 1] == 0)
                    {
                        OcFloodFill(x, y - 1, q, k + 1);
                    }
                }
            }
            if (x < 2 * W)
            {
                if (myHgtData[x + 1, y] == 0)
                {
                    if (OcMM[x + 1, y] == 0)
                    {
                        OcFloodFill(x + 1, y, q, k + 1);
                    }
                }
            }
            if (y < 2 * H)
            {
                if (myHgtData[x, y + 1] == 0)
                {
                    if (OcMM[x, y + 1] == 0)
                    {
                        OcFloodFill(x, y + 1, q, k + 1);
                    }
                }

            }
        }
    }

    public void GenDisClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(0, 0, 0);
    }

    public void GenFtrClr(int W, int H, ref Color[,] C)
    {
        int i, j, k, l, x, y, ii, jj; bool b, bb;
        double mw, kw, hw, aw;
        C = new Color[W, H];
        myFtrData = new int[W, H];
        System.Collections.Stack Rvr = new Stack();
        Point lXY, XY, nXY;
        // 0 - nothing, 1 - river, 2 - river source, 3 - protected area, 4, 5 - unavailable
        int[,] RvrData = new int[W, H];
        // 0 - nothing, 1 - this river, 2 - its source, 3 - protected area, 4, 5 - unavailable
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(0, 0, 0);
        //placing rivers
        k = 0;
        while (k < Rivers)
        {
            x = Rnd.Next(W);
            y = Rnd.Next(H);
            Rvr.Clear();
            if ((Rnd.NextDouble() < RegHgtData[x, y]) && (myFtrData[x, y] == 0))
            {
                b = true;
                bb = true;
                k++;
                l = 0;
                aw = 0;
                int maxL = 500;
                Rvr.Push(new Point(x, y));
                while ((b) && (bb) && (l < maxL))
                {
                    hw = RegHgtData[x, y];
                    mw = 100500;
                    ii = x;
                    jj = y;
                    if (x > 0)
                    {
                        if ((RvrData[x - 1, y] == 0) && ((myFtrData[x - 1, y] == 0) || (myFtrData[x - 1, y] == 3)))
                        {
                            kw = RegHgtData[x - 1, y] - aw;
                            if (kw < mw)
                            {
                                ii = x - 1;
                                jj = y;
                                mw = kw;
                            }
                        }
                    }
                    if (x < W - 1)
                    {
                        if ((RvrData[x + 1, y] == 0) && ((myFtrData[x + 1, y] == 0) || (myFtrData[x + 1, y] == 3)))
                        {
                            kw = RegHgtData[x + 1, y] - aw;
                            if (kw < mw)
                            {
                                ii = x + 1;
                                jj = y;
                                mw = kw;
                            }
                            else
                                if ((kw == mw) && (Rnd.NextDouble() < 0.5))
                                {
                                    ii = x + 1;
                                    jj = y;
                                    mw = kw;
                                }
                        }
                    }
                    if (y > 0)
                    {
                        if ((RvrData[x, y - 1] == 0) && ((myFtrData[x, y - 1] == 0) || (myFtrData[x, y - 1] == 3)))
                        {
                            kw = RegHgtData[x, y - 1] - aw;
                            if (kw < mw)
                            {
                                ii = x;
                                jj = y - 1;
                                mw = kw;
                            }
                            else
                                if ((kw == mw) && (Rnd.NextDouble() < 0.33))
                                {
                                    ii = x;
                                    jj = y - 1;
                                    mw = kw;
                                }
                        }
                    }
                    if (y < H - 1)
                    {
                        if ((RvrData[x, y + 1] == 0) && ((myFtrData[x, y + 1] == 0) || (myFtrData[x, y + 1] == 3)))
                        {
                            kw = RegHgtData[x, y + 1] - aw;
                            if (kw < mw)
                            {
                                ii = x;
                                jj = y + 1;
                                mw = kw;
                            }
                            else
                                if ((kw == mw) && (Rnd.NextDouble() < 0.25))
                                {
                                    ii = x;
                                    jj = y + 1;
                                    mw = kw;
                                }
                        }
                    }
                    if ((x == ii) && (y == jj))
                    {
                        if ((mw > RegHgtData[x, y]) && (mw != 100500))
                        {
                            aw += 1;
                        }
                        else
                        {
                            if (Rvr.Count > 2)
                            {
                                RvrData[ii, jj] = 5;
                                Rvr.Pop();
                                XY = (Point)Rvr.Peek();
                                x = XY.X;
                                y = XY.Y;
                                l--;
                                bb = false;
                            }
                            else
                            {
                                b = false;
                            }
                        }
                    }
                    else
                    {
                        aw = 0;
                        if ((myFtrData[ii, jj] == 0) || (myFtrData[ii, jj] == 3))
                        {
                            l++;
                            XY = (Point)Rvr.Pop();
                            if (Rvr.Count == 0)
                            {
                                lXY = XY;
                            }
                            else
                            {
                                lXY = (Point)Rvr.Peek();
                            }
                            Rvr.Push(XY);
                            x = ii;
                            y = jj;
                            nXY = new Point(x, y);
                            Rvr.Push(nXY);
                            RvrData[x, y] = 1;
                            if (myFtrData[x, y] == 3)
                            {
                                b = false;
                            }
                            if (RegHgtData[x, y] <= 0)
                            {
                                b = false;
                            }

                            if (Rvr.Count <= 2)
                            {
                                for (i = XY.X - 1; i <= XY.X + 1; i++)
                                {
                                    for (j = XY.Y - 1; j <= XY.Y + 1; j++)
                                    {
                                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                                        {
                                            if ((RvrData[i, j] == 0) || (RvrData[i, j] == 3))
                                            {
                                                RvrData[i, j] = 4;
                                            }
                                        }
                                    }
                                }
                            }

                            if ((nXY.X != lXY.X) && (nXY.Y != lXY.Y))
                            {
                                for (i = XY.X - 1; i <= XY.X + 1; i++)
                                {
                                    for (j = XY.Y - 1; j <= XY.Y + 1; j++)
                                    {
                                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                                        {
                                            if ((RvrData[i, j] == 0) || (RvrData[i, j] == 3))
                                            {
                                                if ((i == XY.X) || (j == XY.Y))
                                                {
                                                    RvrData[i, j] = 3;
                                                }
                                                else
                                                {
                                                    if ((i == lXY.X) || (j == lXY.Y) || ((i != nXY.X) && (j != nXY.Y)))
                                                    {
                                                        RvrData[i, j] = 4;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (i = XY.X - 1; i <= XY.X + 1; i++)
                                {
                                    for (j = XY.Y - 1; j <= XY.Y + 1; j++)
                                    {
                                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                                        {
                                            if ((lXY.X == i) || (lXY.Y == j))
                                                if (RvrData[i, j] == 0)
                                                {
                                                    RvrData[i, j] = 3;
                                                }
                                        }
                                    }
                                }
                            }
                            if ((x == 0) || (x == W - 1) || (y == 0) || (y == H - 1))
                            {
                                b = false;
                            }
                            if (!b)
                            {
                                for (i = XY.X - 1; i <= XY.X + 1; i++)
                                {
                                    for (j = XY.Y - 1; j <= XY.Y + 1; j++)
                                    {
                                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                                        {
                                            if ((RvrData[i, j] == 0) || (RvrData[i, j] == 3))
                                            {
                                                RvrData[i, j] = 4;
                                            }
                                        }
                                    }
                                }
                                for (i = x - 1; i <= x + 1; i++)
                                {
                                    for (j = y - 1; j <= y + 1; j++)
                                    {
                                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                                        {
                                            if ((RvrData[i, j] == 0) || (RvrData[i, j] == 3))
                                            {
                                                RvrData[i, j] = 4;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            RvrData[ii, jj] = 4;
                            l++;
                        }
                    }
                }

                if ((Rvr.Count >= 5) && (Rvr.Count < maxL) && (bb))
                {
                    while (Rvr.Count > 1)
                    {
                        XY = (Point)Rvr.Pop();
                    }
                    XY = (Point)Rvr.Pop();
                    myFtrData[XY.X, XY.Y] = 2;

                    for (i = 0; i < W; i++)
                    {
                        for (j = 0; j < H; j++)
                        {
                            if (RvrData[i, j] != 0)
                            {
                                //if (RegHgtData[i, j] > 0)
                                if (((myFtrData[i, j] != 1) && (myFtrData[i, j] != 2) && (RvrData[i, j] != 5)) || (RvrData[i, j] == 1))
                                {
                                    myFtrData[i, j] = RvrData[i, j];
                                }
                                RvrData[i, j] = 0;
                            }
                        }
                    }
                }
                else
                {
                    for (i = 0; i < W; i++)
                    {
                        for (j = 0; j < H; j++)
                        {
                            if (RvrData[i, j] != 0)
                            {
                                RvrData[i, j] = 0;
                            }
                        }
                    }
                }
            }
        }
        for (i = 1; i < W - 1; i++)
        {
            for (j = 1; j < H - 1; j++)
            {
                if (myFtrData[i, j] == 1)
                {
                    if (((myFtrData[i - 1, j] == 1) && (myFtrData[i + 1, j] == 1)
                        && (myFtrData[i, j - 1] != 1) && (myFtrData[i, j + 1] != 1))
                        || ((myFtrData[i - 1, j] != 1) && (myFtrData[i + 1, j] != 1)
                        && (myFtrData[i, j - 1] == 1) && (myFtrData[i, j + 1] == 1)))
                    {
                        if (Rnd.Next(10) < 3)
                        {
                            myFtrData[i, j] = 5;
                        }
                    }
                }
            }
        }
        //analyzing
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                switch (myFtrData[i, j])
                {
                    case 0:
                        myFtrClr[i, j] = Color.FromArgb(0, 0, 0);
                        break;
                    case 1:
                        myFtrClr[i, j] = Color.FromArgb(0, 0, 255);
                        break;
                    case 2:
                        myFtrClr[i, j] = Color.FromArgb(255, 255, 255);
                        break;
                    //case 3:
                    //    myFtrClr[i, j] = Color.FromArgb(0, 255, 0);
                    //    break;
                    //case 4:
                    //    myFtrClr[i, j] = Color.FromArgb(255, 0, 0);
                    //    break;
                    case 3:
                        myFtrClr[i, j] = Color.FromArgb(0, 0, 0);
                        break;
                    case 4:
                        myFtrClr[i, j] = Color.FromArgb(0, 0, 0);
                        break;
                    case 5:
                        myFtrClr[i, j] = Color.FromArgb(0, 255, 255);
                        break;
                }
            }
        }
    }

    public void GenWSfClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(0, 100, 250);
    }

    public void GenRghClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(0, 0, 0);
    }

    public void GenFogClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(255, 255, 255);
    }

    public void GenTRtClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(0, 0, 0);
    }

    public void GenHmdClr(int W, int H)
    {
        myHmdData = new int[W, H];
        myHmdClr = new Color[W, H];
        int i, j, k, l; double kk;
        //int DD = clm_hmd_DD; int D = clm_hmd_D; int d = clm_hmd_d;
        //init
        int[] DDw = clm_hmd_Dw;
        int[] DDe = clm_hmd_De;
        double[] Rw = new double[H];
        double[] Sw = new double[H];
        double[] Re = new double[H];
        double[] Se = new double[H];
        for (k = 0; k < 6; k++)
        {
            //west
            kk = (double)(DDw[k] - DDw[k + 1]) / (H / 6);
            for (j = 0; j < H / 6; j++)
            {
                Rw[j + k * H / 6] = (int)(DDw[k + 1] + kk * (H / 6 - j));
            }
            //east
            kk = (double)(DDe[k] - DDe[k + 1]) / (H / 6);
            for (j = 0; j < H / 6; j++)
            {
                Re[j + k * H / 6] = (int)(DDe[k + 1] + kk * (H / 6 - j));
            }
        }
        Rw[H - 1] = (int)(DDw[6]);
        Re[H - 1] = (int)(DDe[6]);
        //run
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                //west
                k = myHgtData[i, j];
                if (k < 1) k = 1;
                myHmdData[i, j] += (int)((double)Rw[j]) / 3;
                myHmdData[i, j] += (int)((double)Rw[j] * ((double)k / 255)) / 3;
                l = k;
                if (i > 0)
                    l = myHgtData[i - 1, j] - l;
                if (l < 1) l = 1;
                myHmdData[i, j] += (int)((double)Rw[j] * ((double)k / 255)) / 3;
                if (myHmdData[i, j] > 255)
                    myHmdData[i, j] = 255;
                if (myHgtData[i, j] > 0)
                {
                    Rw[j] *= Math.Pow(((double)(350 - myHmdData[i, j]) / 350), 0.03);
                }
                if (Rw[j] <= 1)
                    Rw[j] = 1;
                k = myHgtData[i, j];
                if ((k == 0) && (Rw[j] <= 255))
                    Rw[j] *= 1.005;
                //east
                k = myHgtData[W - 1 - i, j];
                if (k < 1) k = 1;
                myHmdData[W - 1 - i, j] += (int)((double)Re[j]) / 3;
                myHmdData[W - 1 - i, j] += (int)((double)Re[j] * ((double)k / 255)) / 3;
                l = k;
                if (i > 0)
                    l = myHgtData[W - i, j] - l;
                if (l < 1) l = 1;
                myHmdData[W - 1 - i, j] += (int)((double)Re[j] * ((double)k / 255)) / 3;
                if (myHmdData[W - 1 - i, j] > 255)
                    myHmdData[W - 1 - i, j] = 255;
                if (myHgtData[W - 1 - i, j] > 0)
                {
                    Re[j] *= Math.Pow(((double)(350 - myHmdData[i, j]) / 350), 0.03);
                }
                if (Re[j] <= 1)
                    Re[j] = 1;
                k = myHgtData[W - 1 - i, j];
                if ((k == 0) && (Re[j] <= 255))
                    Re[j] *= 1.005;
            }
            for (j = 0; j < H; j++)
            {
                //west
                Sw[j] = 0;
                l = 0;
                for (k = -5; k <= 5; k++)
                {
                    if ((j + k >= 0) && (j + k < H))
                    {
                        Sw[j] += Rw[j + k];
                        l++;
                    }
                }
                Sw[j] /= l;
                //east
                Se[j] = 0;
                l = 0;
                for (k = -10; k <= 10; k++)
                {
                    if ((j + k >= 0) && (j + k < H))
                    {
                        Se[j] += Re[j + k];
                        l++;
                    }
                }
                Se[j] /= l;
            }
            for (j = 1; j < H - 1; j++)
            {
                Rw[j] = Sw[j];
                Re[j] = Se[j];
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                k = myHmdData[i, j];
                myHmdClr[i, j] = Color.FromArgb(255 - k, 255, 255 - k);
                if (myHgtData[i, j] == 0)
                {
                    myHmdClr[i, j] = Color.FromArgb(125, 125, 255);
                }
            }
        }
    }

    public void GenTmpClr(int W, int H)
    {
        myTmpData = new int[W, H];
        myTmpClr = new Color[W, H];
        int i, j, k, m; double kk;
        int c = clm_tmp_c; // -20
        int C = clm_tmp_C; // 255
        kk = (double)(C - c) / H;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                myTmpData[i, j] = (int)(c + kk * j);
            }
        }
        m = 0;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myHmdData[i, j] > m)
                    m = myHmdData[i, j];
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                k = myTmpData[i, j];
                myTmpData[i, j] = (int)((double)(k - 100) * ((double)(3 * m - myHmdData[i, j]) / 3 / m)) + 100;
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                k = myTmpData[i, j];
                myTmpData[i, j] = (int)((double)(k + 100) * ((double)(255 - myHgtData[i, j]) / 255) - 100);
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if ((myTmpData[i, j] < 245) && (myHgtData[i, j] == 0))
                {
                    myTmpData[i, j] += 10;
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myTmpData[i, j] < 0)
                {
                    k = -myTmpData[i, j];
                    myTmpClr[i, j] = Color.FromArgb(Math.Max(255 - k, 0), Math.Max(255 - k, 0), 255);
                }
                else
                {
                    k = myTmpData[i, j];
                    myTmpClr[i, j] = Color.FromArgb(255, Math.Max(255 - k, 0), Math.Max(255 - k, 0));
                }
            }
        }
    }

    public void GenClmClr(int W, int H, ref Color[,] C)
    {
        GenHmdClr(W, H);
        GenTmpClr(W, H);

        int i, j;
        C = new Color[W, H];
        myClmData = new int[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                //C[i, j] = Color.FromArgb(236, 0, 140);
                myClmData[i, j] = 0;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myHgtData[i, j] == 0)
                {
                    myClmData[i, j] = 3;
                }
                else
                {
                    if (myTmpData[i, j] < -2)
                    {
                        myClmData[i, j] = 1;
                    }
                    else if (myTmpData[i, j] < 50)
                    {
                        if (myHmdData[i, j] > myTmpData[i, j])
                        {
                            myClmData[i, j] = 4;
                        }
                        else if (myHmdData[i, j] > 5)
                        {
                            myClmData[i, j] = 6;
                        }
                        else
                        {
                            myClmData[i, j] = 11;
                        }
                        if (myHgtData[i, j] > 70)
                        {
                            myClmData[i, j] = 2;
                        }
                    }
                    else if (myTmpData[i, j] < 105)
                    {
                        if (3 * (myHmdData[i, j] - 10 - 50) + 50 > myTmpData[i, j])// 50 - 100
                        {
                            myClmData[i, j] = 5;
                        }
                        else if (4 * (myHmdData[i, j] - 50) + 50 > myTmpData[i, j])
                        {
                            myClmData[i, j] = 7;
                        }
                        else if (myHmdData[i, j] > 15)
                        {
                            myClmData[i, j] = 6;
                        }
                        else if (myHmdData[i, j] > 10)
                        {
                            myClmData[i, j] = 11;
                        }
                        else
                        {
                            myClmData[i, j] = 10;
                        }
                        if (myHgtData[i, j] > 70)
                        {
                            myClmData[i, j] = 2;
                        }
                    }
                    else if (myTmpData[i, j] < 150)
                    {
                        if (5 * (myHmdData[i, j] - 50) + 50 > myTmpData[i, j])
                        {
                            myClmData[i, j] = 8;
                        }
                        else if (myHmdData[i, j] > 30)
                        {
                            myClmData[i, j] = 9;
                        }
                        else if (myHmdData[i, j] > 15)
                        {
                            myClmData[i, j] = 11;
                        }
                        else
                        {
                            myClmData[i, j] = 10;
                        }
                        if ((myHgtData[i, j] > 95) && (myHmdData[i, j] > 5))
                        {
                            myClmData[i, j] = 2;
                        }
                    }
                    else if (myTmpData[i, j] < 180)
                    {
                        if (myHmdData[i, j] > 70)
                        {
                            myClmData[i, j] = 0;
                        }
                        else
                            if (myHmdData[i, j] > 50)
                            {
                                myClmData[i, j] = 9;
                            }
                            else
                            {
                                myClmData[i, j] = 11;
                            }
                        if (myHgtData[i, j] > 95)
                        {
                            myClmData[i, j] = 9;
                        }
                        if (myHgtData[i, j] > 200)
                        {
                            myClmData[i, j] = 2;
                        }
                    }
                    else
                    {
                        if (myHmdData[i, j] > 70)
                        {
                            myClmData[i, j] = 0;
                        }
                        else
                            if (myHmdData[i, j] > 60)
                            {
                                myClmData[i, j] = 9;
                            }
                                else
                                if (myHmdData[i, j] > 30)
                                {
                                    myClmData[i, j] = 11;
                                }
                                else
                                {
                                    myClmData[i, j] = 10;
                                }
                        if (myHgtData[i, j] > 70)
                        {
                            myClmData[i, j] = 11;
                        }
                    }
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                C[i, j] = (Color)Climates[myClmData[i, j]];
            }
        }
    }

    public void GenForests(int W, int H)
    {
        int i, j, k;
        //forests
        int x, y, fr, d;
        Forests = W * H / 250;
        Color Fr = Color.FromArgb(0, 128, 0);
        Color Wl = Color.FromArgb(0, 0, 0);
        Color Hl = Color.FromArgb(128, 128, 64);
        Color Frt3 = Color.FromArgb(101, 124, 0);
        Color Frt2 = Color.FromArgb(96, 160, 64);
        Color Frt1 = Color.FromArgb(0, 128, 128);
        Color Frt0 = Color.FromArgb(0, 0, 0);
        k = 0;
        while (k < Forests)
        {
            x = Rnd.Next(W);
            y = Rnd.Next(H);
            if ((myGTpClr[x, y] == Wl) || (myGTpClr[x, y] == Hl))
            {
                if (Rnd.NextDouble() < forest_vers[myClmData[x, y]])
                {
                    k++;
                    fr = Rnd.Next(15);
                    for (i = x - fr; i <= x + fr; i++)
                    {
                        for (j = y - fr; j <= y + fr; j++)
                        {
                            if ((i > 0) && (j > 0) && (i < W) && (j < H))
                            {
                                if ((myGTpClr[i, j] == Wl) || (myGTpClr[i, j] == Hl))
                                {
                                    d = (i - x) * (i - x) + (j - y) * (j - y);
                                    if (Rnd.Next(d) < fr * fr / 5)
                                    {
                                        if (Rnd.NextDouble() < forest_vers[myClmData[x, y]])
                                        {
                                            myGTpClr[i, j] = Fr;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        for (i = 0; i < W - 1; i++)
        {
            for (j = 0; j < H - 1; j++)
            {
                //if (myFtrData[i / 2, j / 2] != 0)
                //{
                //    if (myGTpClr[i, j] == Frt0)
                //    {
                //        myGTpClr[i, j] = Frt2;
                //    }
                //}
                if ((myFtrData[i / 2, j / 2] == 1) || (myFtrData[i / 2, j / 2] == 5))
                {
                    if (myGTpClr[i, j] == Frt0)
                    {
                        myGTpClr[i, j] = Frt3;
                    }
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                switch (myClmData[i, j])
                {
                    case 11:
                        if (myGTpClr[i, j] == Frt0)
                        {
                            myGTpClr[i, j] = Frt0;
                        }
                        break;
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 7:
                    case 8:
                        if (myGTpClr[i, j] == Frt0)
                        {
                            myGTpClr[i, j] = Frt2;
                        }
                        break;
                }
            }
        }
    }

    public void GenRadarMaps(int W, int H)
    {
        int i, j, k1, k2, k3;
        myRdrClr1 = new Color[W, H];
        myRdrClr2 = new Color[2 * W, 2 * H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myHgtData[1 + 2 * i, 1 + 2 * j] <= 0)
                {
                    myRdrClr1[i, j] = Color.FromArgb(0, 155, 255);
                }
                else
                {
                    k1 = myHgtData[1 + 2 * i, 1 + 2 * j];
                    k2 = k1; k3 = k1;
                    if (myGTpClr[1 + 2 * i, 1 + 2 * j] == Color.FromArgb(0, 128, 0))
                    {
                        k2 = (128 + k2) / 2;
                    }
                    else if (myGTpClr[1 + 2 * i, 1 + 2 * j] == Color.FromArgb(96, 160, 64))
                    {
                        k1 = (96 + k1) / 2; k2 = (160 + k2) / 2; k3 = (64 + k3) / 2;
                    }
                    else if ((myGTpClr[1 + 2 * i, 1 + 2 * j] == Color.FromArgb(98, 65, 65)) || (myGTpClr[1 + 2 * i, 1 + 2 * j] == Color.FromArgb(196, 128, 128)))
                    {
                        k1 = (64 + k1) / 2; k2 = (128 + k2) / 2; k3 = (32 + k3) / 2;
                    }
                    else
                    {
                        if ((myClmClr[1 + 2 * i, 1 + 2 * j] == SandyDesert) || (myClmClr[1 + 2 * i, 1 + 2 * j] == RockyDesert))
                        {
                            k1 = (245 + k1) / 2; k2 = (228 + k2) / 2; k3 = (156 + k3) / 2;
                        }
                        else if (myClmClr[1 + 2 * i, 1 + 2 * j] == SemiArid)
                        {
                            k1 = (195 + k1) / 2; k2 = (208 + k2) / 2; k3 = (127 + k3) / 2;
                        }
                        else if (myClmClr[1 + 2 * i, 1 + 2 * j] == GrasslandInfertile)
                        {
                            k1 = (145 + k1) / 2; k2 = (188 + k2) / 2; k3 = (101 + k3) / 2;
                        }
                        else
                        {
                            k1 = (96 + k1) / 2; k2 = (160 + k2) / 2; k3 = (64 + k3) / 2;
                        }
                    }
                    myRdrClr1[i, j] = Color.FromArgb(k1, k2, k3);
                }
            }
        }
        for (i = 0; i < 2 * W; i++)
        {
            for (j = 0; j < 2 * H; j++)
            {
                if (myHgtData[i, j] <= 0)
                {
                    myRdrClr2[i, j] = Color.FromArgb(0, 155, 255);
                }
                else
                {
                    k1 = myHgtData[i, j];
                    k2 = k1; k3 = k1;
                    if (myGTpClr[i, j] == Color.FromArgb(0, 128, 0))
                    {
                        k2 = (128 + k2) / 2;
                    }
                    else if (myGTpClr[i, j] == Color.FromArgb(96, 160, 64))
                    {
                        k1 = (96 + k1) / 2; k2 = (160 + k2) / 2; k3 = (64 + k3) / 2;
                    }
                    else if ((myGTpClr[i, j] == Color.FromArgb(98, 65, 65)) || (myGTpClr[i, j] == Color.FromArgb(196, 128, 128)))
                    {
                        k1 = (64 + k1) / 2; k2 = (128 + k2) / 2; k3 = (32 + k3) / 2;
                    }
                    else
                    {
                        if ((myClmClr[i, j] == SandyDesert) || (myClmClr[i, j] == RockyDesert))
                        {
                            k1 = (245 + k1) / 2; k2 = (228 + k2) / 2; k3 = (156 + k3) / 2;
                        }
                        else if (myClmClr[i, j] == SemiArid)
                        {
                            k1 = (195 + k1) / 2; k2 = (208 + k2) / 2; k3 = (127 + k3) / 2;
                        }
                        else if (myClmClr[i, j] == GrasslandInfertile)
                        {
                            k1 = (145 + k1) / 2; k2 = (188 + k2) / 2; k3 = (101 + k3) / 2;
                        }
                        else
                        {
                            k1 = (96 + k1) / 2; k2 = (160 + k2) / 2; k3 = (64 + k3) / 2;
                        }
                    }
                    myRdrClr2[i, j] = Color.FromArgb(k1, k2, k3);
                }
            }
        }
    }

    public void GenFEClr(int W, int H)
    {
        int i, j;
        myFEClr = new Color[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if ((myFacClr[i, j] != Color.FromArgb(0, 100, 255)) && (myFacClr[i, j] != Color.FromArgb(0, 0, 0)))
                {
                    myFEClr[i, j] = myFacClr[i, j];
                }
                else
                {
                    myFEClr[i, j] = myRdrClr1[i, j];
                }
            }
        }
    }

    public void CreateDscTrn(int W, int H, string ss)
    {
        FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
        SW = new StreamWriter(FS);
        SW.WriteLine("dimensions" + System.Environment.NewLine + "{");
        SW.WriteLine("\twidth  " + W.ToString());
        SW.WriteLine("\theight  " + H.ToString());
        SW.WriteLine("}" + System.Environment.NewLine + "heights" + System.Environment.NewLine
            + "{" + System.Environment.NewLine + "\tmin_sea_height  -5122.256" + System.Environment.NewLine + "\tmax_land_height  9511.272"
            + System.Environment.NewLine + "}" + System.Environment.NewLine + "roughness" + System.Environment.NewLine
            + "{" + System.Environment.NewLine + "\tmin  50.000" + System.Environment.NewLine + "\tmax  200.000"
            + System.Environment.NewLine + "}" + System.Environment.NewLine + "fractal" + System.Environment.NewLine
            + "{" + System.Environment.NewLine + "\tmultiplier  0.500" + System.Environment.NewLine + "}"
            + System.Environment.NewLine + "lattitude" + System.Environment.NewLine + "{" + System.Environment.NewLine
            + "\tmin  22.000" + System.Environment.NewLine + "\tmax  56.000" + System.Environment.NewLine + "}");
        SW.Close();
        FS.Close();
    }

    public void GenRegions(int W, int H, int RN, ref TWReg[] A, ref Color[,] C)
    {
        int i, j, k, x, y; bool b, bb; //int ii;
        A = new TWReg[RN];
        C = new Color[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                C[i, j] = Color.FromArgb(41, 140, 233);
        for (i = 0; i < RN; i++)
        {
            A[i].C = Color.FromArgb(Rnd.Next(256), Rnd.Next(256), i);
        }

        for (i = 0; i < RN; i++)
        {
            A[i].Is = false;
            A[i].Port.Is = false;
            j = 0; bb = true;
            while (//(j < 1000)&&
                (bb))
            {
                x = 1 + Rnd.Next(W - 2);
                y = 1 + Rnd.Next(H - 2);
                b = true;
                for (k = 0; k < i; k++)
                {
                    if ((Math.Abs(A[k].X - x) < 3) && (Math.Abs(A[k].Y - y) < 3))
                        b = false;
                }
                if (Rnd.NextDouble() > vers[myClmData[2 * x, 2 * y]])
                {
                    b = false;
                }
                if (b)
                {
                    if ((myHgtData[2 * x, 2 * y] <= 0) || (myHgtData[2 * x + 1, 2 * y] <= 0) || (myHgtData[2 * x, 2 * y + 1] <= 0) || (myHgtData[2 * x + 1, 2 * y + 1] <= 0))
                    {
                        b = false;
                    }
                }
                if (b)
                {
                    Color HMnC = Color.FromArgb(196, 128, 128);
                    Color MnC = Color.FromArgb(98, 65, 65);
                    if ((myGTpClr[2 * x, 2 * y] == HMnC) || (myGTpClr[2 * x + 1, 2 * y] == HMnC) ||
                        (myGTpClr[2 * x, 2 * y + 1] == HMnC) || (myGTpClr[2 * x + 1, 2 * y + 1] == HMnC) ||
                        (myGTpClr[2 * x, 2 * y] == MnC) || (myGTpClr[2 * x + 1, 2 * y] == MnC) ||
                        (myGTpClr[2 * x, 2 * y + 1] == MnC) || (myGTpClr[2 * x + 1, 2 * y + 1] == MnC))
                    {
                        b = false;//here also path to ocean check make !
                    }
                }
                if (b)
                {
                    if (!(AvMap[x, y] == 2))
                    {
                        b = false;
                    }
                }
                if (b)
                {
                    A[i].X = x;
                    A[i].Y = y;
                    A[i].Is = true;
                    bb = false;
                }
                j++;
            }
        }

        //mb regions analizing
        RegNum = new int[W, H];
        RegVal = new double[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                RegNum[i, j] = -1;
                RegVal[i, j] = 1000000;
            }
        }

        ArrayList RegArLA = new ArrayList();
        ArrayList RegArLB = new ArrayList();
        int[,] RegDone = new int[W, H];
        record rec = new record();
        record rec2 = new record();
        for (i = 0; i < RN; i++)
        {
            rec.x = Regs[i].X; rec.y = Regs[i].Y;
            rec.val = 0; rec.k = 0; rec.num = i; rec.c = myClmData[1 + 2 * Regs[i].X, 1 + 2 * Regs[i].Y];
            RegArLB.Add(rec);
        }
        RegArLA = RegArLB;
        RegArLB = new ArrayList();
        double d, m;
        double[,] RndAr = new double[W, H];
        for (i = 0; i < W; i++)
            for (j = 0; j < H; j++)
                RndAr[i, j] = Rnd.NextDouble();
        while (RegArLA.Count > 0)
        {
            k = 0;
            while (k < RegArLA.Count)
            {
                rec = (record)RegArLA[k];
                k++;
                if ((myFtrData[rec.x, rec.y] != 1) && (rec.k < 150))
                {
                    if (((rec.val < RegVal[rec.x, rec.y]) && ((RegDone[rec.x, rec.y] == 0) || (RegNum[rec.x, rec.y] != rec.num)))
                        || ((rec.val + 5 < RegVal[rec.x, rec.y]) && (AvMap[rec.x, rec.y] != 1)))
                    {
                        d = 0;
                        if (myFtrData[rec.x, rec.y] == 5) d = 5;
                        RegVal[rec.x, rec.y] = rec.val;
                        RegNum[rec.x, rec.y] = rec.num;
                        //double l = (double)(lim - k) / lim; // lim = 90;
                        m = rec.val + 0.2 + d + RegHgtData[rec.x, rec.y];
                        if (myClmData[1 + 2 * rec.x, 1 + 2 * rec.y] != rec.c) m += 5;
                        if (myClmData[1 + 2 * rec.x, 1 + 2 * rec.y] != myClmData[1 + 2 * Regs[rec.num].X, 1 + 2 * Regs[rec.num].Y])
                            m += 0.2;
                        if (AvMap[rec.x, rec.y] == 1) RegNum[rec.x, rec.y] = -1;
                        if (rec.x - 1 >= 0)
                        {
                            rec2.x = rec.x - 1; rec2.y = rec.y;
                            rec2.val = m + 2.0 * RndAr[rec2.x + 1, rec2.y] * rec.k / 100; if (AvMap[rec2.x, rec2.y] == 1) rec2.val += 50;
                            rec2.num = rec.num; rec2.k = rec.k + 1; rec2.c = myClmData[1 + 2 * rec.x, 1 + 2 * rec.y];
                            RegArLB.Add(rec2);
                        }
                        if (rec.x + 1 < W)
                        {
                            rec2.x = rec.x + 1; rec2.y = rec.y;
                            rec2.val = m + 2.0 * RndAr[rec2.x - 1, rec2.y] * rec.k / 100; if (AvMap[rec2.x, rec2.y] == 1) rec2.val += 50;
                            rec2.num = rec.num; rec2.k = rec.k + 1; rec2.c = myClmData[1 + 2 * rec.x, 1 + 2 * rec.y];
                            RegArLB.Add(rec2);
                        }
                        if (rec.y - 1 >= 0)
                        {
                            rec2.x = rec.x; rec2.y = rec.y - 1;
                            rec2.val = m + 2.0 * RndAr[rec2.x, rec2.y + 1] * rec.k / 100; if (AvMap[rec2.x, rec2.y] == 1) rec2.val += 50;
                            rec2.num = rec.num; rec2.k = rec.k + 1; rec2.c = myClmData[1 + 2 * rec.x, 1 + 2 * rec.y];
                            RegArLB.Add(rec2);
                        }
                        if (rec.y + 1 < H)
                        {
                            rec2.x = rec.x; rec2.y = rec.y + 1;
                            rec2.val = m + 2.0 * RndAr[rec2.x, rec2.y - 1] * rec.k / 100; if (AvMap[rec2.x, rec2.y] == 1) rec2.val += 50;
                            rec2.num = rec.num; rec2.k = rec.k + 1; rec2.c = myClmData[1 + 2 * rec.x, 1 + 2 * rec.y];
                            RegArLB.Add(rec2);
                        }
                        RegDone[rec.x, rec.y] = 1;
                    }
                }
            }
            RegArLA = RegArLB;
            RegArLB = new ArrayList();
        }

        //val to clr
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (RegNum[i, j] == -1)
                {
                    myRegClr[i, j] = OcColor;
                }
                else
                {
                    myRegClr[i, j] = A[RegNum[i, j]].C;
                }
            }
        }

        //ports
        int ii;
        bool bbb, bbbb;
        if (cP)
        {
            for (k = 0; k < RN; k++)
            {
                for (i = A[k].X - 4; i <= A[k].X + 4; i++)
                {
                    for (j = A[k].Y - 4; j <= A[k].Y + 4; j++)
                    {
                        if (((A[k].X != i) || (A[k].Y != j)) && (!A[k].Port.Is))
                        {
                            bbb = false;
                            if ((i >= 0) && (i < W) && (j >= 0) && (j < H))
                            //if ((AvMap2[i, j]) && (AvMap[i, j] != 0))//((AvMap1[i, j] ==2)||(AvMap1[i, j] ==1))//(bAv)//(AvMap[i, j] == 3)
                            {
                                bbb = true;
                                for (ii = 0; ii < RN; ii++)
                                {
                                    if (ii != k)
                                    {
                                        if ((Math.Abs(A[ii].X - i) < 3) && (Math.Abs(A[ii].Y - j) < 3))
                                            bbb = false;
                                        if (A[ii].Port.Is)
                                        {
                                            if ((Math.Abs(A[ii].Port.X - i) < 3) && (Math.Abs(A[ii].Port.Y - j) < 3))
                                                bbb = false;
                                        }
                                    }
                                }
                                if (AvMap[i, j] != 2)
                                {
                                    bbb = false;
                                }
                                if (myRegClr[i, j] != A[k].C)
                                {
                                    bbb = false;
                                }
                                bbbb = false;
                                if (i > 0)
                                {
                                    if ((AvMap[i - 1, j] == 1) && (OcData[1 + 2 * (i - 1), 1 + 2 * (j)] == 2))
                                    {
                                        bbbb = true;
                                    }
                                }
                                if (i < W - 1)
                                {
                                    if ((AvMap[i + 1, j] == 1) && (OcData[1 + 2 * (i + 1), 1 + 2 * (j)] == 2))
                                    {
                                        bbbb = true;
                                    }
                                }
                                if (j > 0)
                                {
                                    if ((AvMap[i, j - 1] == 1) && (OcData[1 + 2 * (i), 1 + 2 * (j - 1)] == 2))
                                    {
                                        bbbb = true;
                                    }
                                }
                                if (j > H - 1)
                                {
                                    if ((AvMap[i, j + 1] == 1) && (OcData[1 + 2 * (i), 1 + 2 * (j + 1)] == 2))
                                    {
                                        bbbb = true;
                                    }
                                }
                                if (!bbbb)
                                {
                                    bbb = false;
                                }
                            }
                            if (bbb)
                            {
                                A[k].Port.Is = true;
                                A[k].Port.X = i;
                                A[k].Port.Y = j;
                                break;
                            }
                        }
                    }
                }
            }
        }

        for (k = 0; k < RN; k++)
        {
            if (A[k].Is)
            {
                for (i = A[k].X - 1; i <= A[k].X + 1; i++)
                {
                    for (j = A[k].Y - 1; j <= A[k].Y + 1; j++)
                    {
                        if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                        {
                            C[i, j] = A[k].C;
                        }
                    }
                }
                C[A[k].X, A[k].Y] = Color.FromArgb(0, 0, 0);
                if (A[k].Port.Is)
                {
                    for (i = A[k].Port.X - 1; i <= A[k].Port.X + 1; i++)
                    {
                        for (j = A[k].Port.Y - 1; j <= A[k].Port.Y + 1; j++)
                        {
                            if ((i >= 0) && (j >= 0) && (i < W) && (j < H))
                            {
                                if ((A[k].X != i) || (A[k].Y != j))
                                {
                                    if (AvMap[i, j] != 1)
                                    {
                                        C[i, j] = A[k].C;
                                    }
                                }
                            }
                        }
                    }
                    C[A[k].Port.X, A[k].Port.Y] = Color.FromArgb(255, 255, 255);
                }
            }
        }

        //climates of regions
        ClmOfRegs = new int[RN, 13];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                k = RegNum[i, j];
                if (k >= 0)
                {
                    ii = myClmData[1 + 2 * i, 1 + 2 * j];
                    ClmOfRegs[k, ii]++;
                    ClmOfRegs[k, 12]++;
                }
            }
        }

        MainClmOfRegs = new int[RN];
        int mx;
        for (i = 0; i < RN; i++)
        {
            mx = 0;
            for (j = 0; j < 12; j++)
            {
                if (ClmOfRegs[i, j] > mx)
                {
                    mx = ClmOfRegs[i, j];
                    MainClmOfRegs[i] = j;
                }
            }
        }

        for (i = 0; i < RN; i++)
        {
            for (j = 0; j < 12; j++)
            {
                if ((double)ClmOfRegs[i, j] / ClmOfRegs[i, 12] > 0.5)
                {
                    ClmOfRegs[i, j] = 2;
                }
                else if ((double)ClmOfRegs[i, j] / ClmOfRegs[i, 12] > 0.01)
                {
                    ClmOfRegs[i, j] = 1;
                }
            }
        }

        //map of main climates of regions
        MainClmClr = new Color[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (RegNum[i, j] == -1)
                {
                    MainClmClr[i, j] = Color.FromArgb(255, 255, 255);
                }
                else
                {
                    MainClmClr[i, j] = Climates[MainClmOfRegs[RegNum[i, j]]];
                }
            }
        }

        //regions of factions
        RegsOfFacs = new int[NFA];
        FacsOfRegs = new int[RN];
        if (!hist)
        {
            for (i = 0; i < NFA; i++)
            {
                if (FIs[i])
                {
                    RegsOfFacs[i] = i;
                    FacsOfRegs[i] = i;
                }
            }
            for (i = NFA; i < RN; i++)
            {
                FacsOfRegs[i] = -1;
            }
        }
        else
        {
            bool[] B = new bool[NFA];
            for (i = 0; i < NFA; i++)
            {
                RegsOfFacs[i] = -1;
            }
            for (i = 0; i < RN; i++)
            {
                FacsOfRegs[i] = -1;
            }
            for (i = 0; i < NFA; i++)
            {
                if (FIs[i]) B[i] = false; else B[i] = true;
            }
            int[] cl1 = { 2, 2, 2, 1, 1, 0, 0, 0 };
            int[] cl2 = { 2, 1, 0, 2, 1, 2, 1, 0 };
            for (j = 0; j < 8; j++)
            {
                for (i = 0; i < NFA; i++)
                {
                    if (!B[i])
                    {
                        k = 0;
                        b = true;
                        while ((b) && (k < RN))
                        {
                            if ((ClmOfRegs[k, def_clms[i]] == cl1[j]) && (ClmOfRegs[k, def_clms2[i]] == cl2[j]))
                            {
                                if (FacsOfRegs[k] == -1)
                                {
                                    RegsOfFacs[i] = k;
                                    FacsOfRegs[k] = i;
                                    b = false;
                                    B[i] = true;
                                }
                                else
                                    k++;
                            }
                            else
                                k++;
                        }
                    }
                }
            }
        }


        for (j = 0; j < RFN - 1; j++)
        {
            //
            bool[] B = new bool[NFA];
            for (i = 0; i < NFA; i++)
            {
                if (FIs[i]) B[i] = false; else B[i] = true;
            }
            int[] cl1 = { 2, 2, 2, 1, 1, 0, 0, 0 };
            int[] cl2 = { 2, 1, 0, 2, 1, 2, 1, 0 };
            for (i = 0; i < NFA; i++)
            {
                if (!B[i])
                {
                    int nrstReg = -1;
                    double nrstDist = 100500;
                    double newDist;
                    k = 0;
                    b = true;
                    for (k = 0; k < RN; k++)
                    {
                        if (FacsOfRegs[k] == -1)
                        {
                            newDist = Math.Abs(A[k].X - A[RegsOfFacs[i]].X) + Math.Abs(A[k].Y - A[RegsOfFacs[i]].Y);
                            if (ClmOfRegs[k, def_clms[i]] == 2)
                            {
                                newDist /= 3;
                            }
                            if (ClmOfRegs[k, def_clms[i]] == 1)
                            {
                                newDist /= 2;
                            }
                            if (newDist < nrstDist)
                            {
                                nrstReg = k;
                                nrstDist = newDist;
                            }
                        }
                    }
                    if (nrstReg != -1)
                    {
                        B[i] = true;
                        FacsOfRegs[nrstReg] = i;
                    }
                }
            }
        }

        //populace // 2000 + 8000(gtp, clm)
        PopOfRegs = new double[RN];
        FerOfRegs = new int[RN];
        double[] na = new double[RN];
        double[] nb = new double[RN];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (RegNum[i, j] >= 0)
                {
                    na[RegNum[i, j]] += clmpopkf[myClmData[1 + 2 * i, 1 + 2 * j]];
                    nb[RegNum[i, j]]++;
                }
            }
        }
        for (i = 0; i < RN; i++)
        {
            PopOfRegs[i] = 2000 + na[i] / nb[i] * 8000;
        }
        //map of population
        myPopClr = new Color[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (RegNum[i, j] == -1)
                {
                    myPopClr[i, j] = Color.FromArgb(255, 255, 255);
                }
                else
                {
                    k = (int)(255.0 * (PopOfRegs[RegNum[i, j]] - 2000) / 8000);
                    if (k < 0) k = 0;
                    if (k > 255) k = 255;
                    myPopClr[i, j] = Color.FromArgb(k, 0, 0);
                }
            }
        }
    }

    public void GenFacClr(int W, int H, ref Color[,] C)
    {
        int i, j;
        C = new Color[W, H];
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (RegNum[i, j] == -1)
                {
                    C[i, j] = Color.FromArgb(0, 100, 255);
                }
                else
                {
                    C[i, j] = Color.FromArgb(0, 0, 0);
                    if ((FacsOfRegs[RegNum[i, j]] > -1) && (FIs[FacsOfRegs[RegNum[i, j]]]))
                    {
                        C[i, j] = FacClrs[FacsOfRegs[RegNum[i, j]]];
                    }
                }
                
            }
        }
        
    }

    public int RecRegAnal(int x, int y, double val, int num, int k, int c)
    {
        int lim = 90;
        if (k == lim) return 0;
        if (val > 550) return 0;
        double d = 0;
        //if ((AvMap[x, y] == 2) || (AvMap[x, y] == 0) && (myFtrData[x, y] != 1))
        if ((myFtrData[x, y] != 1))
        {
            if (val < RegVal[x, y])
            {
                if (myFtrData[x, y] == 5) d = 5;
                RegVal[x, y] = val;
                RegNum[x, y] = num;
                double l = (double)(lim - k) / lim;
                double m = val + 0.2 + d + RegHgtData[x, y];
                if (myClmData[1 + 2 * x, 1 + 2 * y] != c)
                {
                    m += 1;
                }
                if (AvMap[x, y] == 1)
                {
                    RegNum[x, y] = -1;
                }
                if (x - 1 >= 0)
                {
                    if (AvMap[x - 1, y] != 1)
                    {
                        RecRegAnal(x - 1, y, m + 0.2 * l * Rnd.NextDouble(), num, k + 1, c);
                    }
                    else
                    {
                        RecRegAnal(x - 1, y, val + 100, num, k + 1, c);
                    }
                }
                if (x + 1 < W)
                {
                    if (AvMap[x + 1, y] != 1)
                    {
                        RecRegAnal(x + 1, y, m + 0.2 * l * Rnd.NextDouble(), num, k + 1, c);
                    }
                    else
                    {
                        RecRegAnal(x + 1, y, val + 100, num, k + 1, c);
                    }
                }
                if (y - 1 >= 0)
                {
                    if (AvMap[x, y - 1] != 1)
                    {
                        RecRegAnal(x, y - 1, m + 0.2 * l * Rnd.NextDouble(), num, k + 1, c);
                    }
                    else
                    {
                        RecRegAnal(x, y - 1, val + 100, num, k + 1, c);
                    }
                }
                if (y + 1 < H)
                {
                    if (AvMap[x, y + 1] != 1)
                    {
                        RecRegAnal(x, y + 1, m + 0.2 * l * Rnd.NextDouble(), num, k + 1, c);
                    }
                    else
                    {
                        RecRegAnal(x, y + 1, val + 100, num, k + 1, c);
                    }
                }
            }
        }
        return 0;
    }

    public void GenRess(int RN, TWReg[] Regs)
    {
        int i, j, k, l;
        ressMap = new int[W, H];
        ress_Regs = new bool[RN, nRess];
        int clm; double mx, nmx;
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                clm = myClmData[1 + 2 * i, 1 + 2 * j];
                mx = -1.0;
                for (k = 0; k < nRess; k++)
                {
                    for (l = 0; l <= 2; l++)
                    {
                        if (ress_Clm[k, l] == clm)
                        {
                            if (Rnd.NextDouble() < 0.005)
                            {
                                nmx = Rnd.NextDouble();
                                if (nmx > mx)
                                {
                                    ressMap[i, j] = k;
                                    mx = nmx;
                                }
                            }
                        }
                    }
                }
                if (mx > 0)
                {
                    if ((RegNum[i, j] >= 0) && (RegNum[i, j] < RN))
                        ress_Regs[RegNum[i, j], ressMap[i, j]] = true;
                }
                else
                {
                    ressMap[i, j] = -1;
                }
            }
        }
    }

    public void CreateDscReg(int RN, TWReg[] Regs, string ss)
    {
        GenRess(RN, Regs);
        FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
        SW = new StreamWriter(FS);
        int i, j, l;
        SW.WriteLine(";");
        SW.WriteLine("; regions list");
        SW.WriteLine(";");
        SW.WriteLine();
        string deffac;
        string ress;
        int fl;
        for (i = 0; i < RN; i++)
        {
            while (!FIs[j = Rnd.Next(NFA)]) { }
            deffac = Facs[j];
            {
                ress = "";
                for (l = 0; l < nRess; l++)
                {
                    if (ress_Regs[i, l])
                    {
                        if (ress != "") ress += ", ";
                        ress += ress_Names[l];
                    }
                }
                if (ress == "") ress = "slaves";
                //ress = "wild_animals";
                fl = (int)(PopOfRegs[i] / 1000); 
                SW.WriteLine("P" + i.ToString() + Environment.NewLine
                + "\tT" + i.ToString() + Environment.NewLine
                + "\t" + deffac + Environment.NewLine
                + "\tNative_Rebels" + Environment.NewLine
                + "\t" + Regs[i].C.R + " " + Regs[i].C.G + " " + Regs[i].C.B + Environment.NewLine
                + "\t" + ress + Environment.NewLine
                + "\t5" + Environment.NewLine + "\t" + fl + Environment.NewLine +
                "religions { catholic 0 orthodox 0 islam 0 pagan 100 heretic 0 }" + Environment.NewLine
                );
            }
        }
        SW.Close();
        FS.Close();
    }

    public void CreateDscStrat(int H, int RN, int FN, TWReg[] Regs, string ss)
    {
        int i, j;
        FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
        SW = new StreamWriter(FS);
        SW.WriteLine("; Custom campaign script generated by Aurun`s M2TW Random Map Generator");
        SW.WriteLine(Environment.NewLine + "campaign\t\timperial_campaign");
        SW.WriteLine("playable");
        for (i = 0; i < NFA; i++)
        {
            if (FIs[i])
            {
                SW.WriteLine("\t" + Facs[i]);
            }
        }
        SW.WriteLine("end" + Environment.NewLine + "unlockable" + Environment.NewLine);
        SW.WriteLine("end" + Environment.NewLine + "nonplayable" + Environment.NewLine
            + "\t slave" + Environment.NewLine + "end" + Environment.NewLine + Environment.NewLine);

        SW.WriteLine("start_date\t1 summer" + Environment.NewLine + "end_date\t500 winter" + Environment.NewLine + 
            "timescale\t2.00" + Environment.NewLine + Environment.NewLine);

        SW.WriteLine("marian_reforms_disabled" + Environment.NewLine
            + "rebelling_characters_active" + Environment.NewLine
            + "gladiator_uprising_disabled" + Environment.NewLine
            + "night_battles_enabled" + Environment.NewLine
            + "show_date_as_turns" + Environment.NewLine
            + "brigand_spawn_value 10" + Environment.NewLine
            + "pirate_spawn_value 28" + Environment.NewLine + Environment.NewLine);
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (ressMap[i, j] >= 0)
                {
                    bool bl = true;
                    for (int k = 0; k < RN; k++)
                    {
                        if ((Regs[k].X == i) && (Regs[k].Y == j)) bl = false;
                        if (Regs[k].Port.Is)
                            if ((Regs[k].Port.X == i) && (Regs[k].Port.Y == j)) bl = false;
                    }
                    if (bl)
                        SW.WriteLine("resource\t" + ress_Names[ressMap[i, j]] + ",\t" + i + ",\t" + (H - 1 - j));
                }
            }
        }

        //double[] na = new double[RN];
        //double[] nb = new double[RN];
        //for (i = 0; i < RN; i++)
        //{
        //    for (j = 0; j < nRess; j++)
        //    {
        //        if (ress_Regs[i, j])
        //        {
        //            na[i]++;
        //        }
        //        nb[i]++;
        //    }
        //    PopOfRegs[i] += na[i] / nb[i] * 4000;
        //}

        string[] facbiatt = { "balanced", "religious", "trader", "comfortable", 
                                "bureaucrat", "craftsman", "sailor", "fortified" };
        string[] facunatt = { "genghis", "caesar", "napoleon", "mao", "stalin", "smith", "henry" };
        string facatts;
        for (i = 0; i < NFA; i++)
        {
            if (FIs[i])
            {
                facatts = "" + facbiatt[Rnd.Next(8)] + " " + facunatt[Rnd.Next(7)];
                SW.WriteLine("faction\t" + Facs[i] + ", " + facatts);
                SW.WriteLine("ai_label\tdefault ");
                SW.WriteLine("denari\t" + money + Environment.NewLine);
                SW.WriteLine("denari_kings_purse\t2500" + Environment.NewLine);
                SW.Write("settlement" + Environment.NewLine + "{" + Environment.NewLine
                    + "\tlevel ");
                if (PopOfRegs[RegsOfFacs[i]] < 6001)
                {
                    SW.Write("large_town");
                }
                else if (PopOfRegs[RegsOfFacs[i]] < 12001)
                {
                    SW.Write("city");
                }
                else
                {
                    SW.Write("large_city");
                }
                SW.Write(Environment.NewLine + "\tregion P" + RegsOfFacs[i].ToString() + Environment.NewLine
                    + "\tyear_founded 0" + Environment.NewLine + "\tpopulation ");
                SW.Write("" + (int)PopOfRegs[RegsOfFacs[i]]);
                SW.Write(Environment.NewLine
                    + "\tplan_set default_set" + Environment.NewLine + "\tfaction_creator " + Facs[i]
                    
                    + Environment.NewLine + "\tbuilding" + Environment.NewLine + "\t{" + Environment.NewLine
                    + "\t\ttype core_building ");
                if (PopOfRegs[RegsOfFacs[i]] < 6001)
                {
                    SW.Write("wooden_wall");
                }
                else if (PopOfRegs[RegsOfFacs[i]] < 12001)
                {
                    SW.Write("stone_wall");
                }
                else
                {
                    SW.Write("large_stone_wall");
                }
                SW.Write(Environment.NewLine + "\t}"
                    + Environment.NewLine + "}" + Environment.NewLine);
                SW.WriteLine();

                for (j = 0; j < RN; j++)
                {
                    if ((FacsOfRegs[j] == i) && (RegsOfFacs[i] != j))
                    {
                        SW.Write("settlement" + Environment.NewLine + "{" + Environment.NewLine
                    + "\tlevel ");
                        if (PopOfRegs[j] < 6001)
                        {
                            SW.Write("large_town");
                        }
                        else if (PopOfRegs[j] < 12001)
                        {
                            SW.Write("city");
                        }
                        else
                        {
                            SW.Write("large_city");
                        }
                        SW.Write(Environment.NewLine + "\tregion P" + j.ToString() + Environment.NewLine
                            + "\tyear_founded 0" + Environment.NewLine + "\tpopulation ");
                        SW.Write("" + (int)PopOfRegs[j]);
                        SW.Write(Environment.NewLine
                            + "\tplan_set default_set" + Environment.NewLine + "\tfaction_creator " + Facs[i]

                            + Environment.NewLine + "\tbuilding" + Environment.NewLine + "\t{" + Environment.NewLine
                            + "\t\ttype core_building ");
                        if (PopOfRegs[j] < 6001)
                        {
                            SW.Write("wooden_wall");
                        }
                        else if (PopOfRegs[j] < 12001)
                        {
                            SW.Write("stone_wall");
                        }
                        else
                        {
                            SW.Write("large_stone_wall");
                        }
                        SW.Write(Environment.NewLine + "\t}"
                            + Environment.NewLine + "}" + Environment.NewLine);
                        SW.WriteLine();
                    }
                }

                SW.WriteLine("character\t" + Names[i] + ", named character,male, leader, age 20, , x "
                    + Regs[RegsOfFacs[i]].X + ", y " + (H - 1 - Regs[RegsOfFacs[i]].Y));
                SW.WriteLine("traits GoodCommander 5 ,GoodAdministrator 3 ,GoodTrader 3, PublicFaith 4" + 
                    Environment.NewLine + "ancillaries" + Environment.NewLine + "army");
                SW.WriteLine("unit\t\t" + Generals[i] + "\t\t\t\texp 1 armour 0 weapon_lvl 0");
                SW.WriteLine(Environment.NewLine);
            }
        }

        SW.WriteLine("faction\tslave, balanced caesar");
        SW.WriteLine("ai_label\tslave_faction ");
        SW.WriteLine("denari\t" + money * 10);
        SW.WriteLine();
        string deffac;
        for (i = 0; i < RN; i++)
        {
            //if (i < NFA) { if (!FIs[i]) b = true; else b = false; }
            if (FacsOfRegs[i] == -1)
            {
                // here climates ~ factions add!
                while (!FIs[j = Rnd.Next(NFA)]) { }
                deffac = Facs[j];
                SW.Write("settlement" + Environment.NewLine + "{" + Environment.NewLine
                    + "\tlevel ");
                if (PopOfRegs[i] < 6001)
                {
                    SW.Write("large_town");
                }
                else if (PopOfRegs[i] < 12001)
                {
                    SW.Write("city");
                }
                else
                {
                    SW.Write("large_city");
                }
                SW.Write(Environment.NewLine + "\tregion P" + i.ToString() + Environment.NewLine
                    + "\tyear_founded 0" + Environment.NewLine + "\tpopulation ");
                SW.Write("" + (int)PopOfRegs[i]);
                SW.WriteLine(Environment.NewLine
                    + "\tplan_set default_set" + Environment.NewLine + "\tfaction_creator " + deffac);

                SW.Write("\tbuilding" + Environment.NewLine + "\t{" + Environment.NewLine
                + "\t\ttype core_building ");
                if (PopOfRegs[i] < 6001)
                {
                    SW.Write("wooden_wall");
                }
                else if (PopOfRegs[i] < 12001)
                {
                    SW.Write("stone_wall");
                }
                else
                {
                    SW.Write("large_stone_wall");
                }
                SW.Write(Environment.NewLine + "\t}"
                    + Environment.NewLine + "}" + Environment.NewLine);
                SW.WriteLine();
            }
        }

        for (i = 0; i < NFA; i++)
        {
            if (FIs[i])
            {
                SW.WriteLine("faction_relationships\t" + Facs[i] + ",\t\t600\t\tslave");
            }
        }
        SW.Close();
        FS.Close();
    }

    public void CreateRegsTxt(int RN, string ss)
    {
        FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
        SW = new StreamWriter(FS, Encoding.Unicode);
        int i;
        string sss;
        SW.WriteLine("¬ Names for regions and settlements in the imperial campaign");
        //SW.WriteLine("{Britannia_Inferior}Нижняя Британия");
        //SW.WriteLine("{Tribus_Saxones}Земля саксов");
        for (i = 0; i < RN; i++)
        {
            sss = RndName();
            SW.WriteLine("{P" + i.ToString() + "}" + sss);
            SW.WriteLine("{T" + i.ToString() + "}" + sss);
        }
        SW.Close();
        FS.Close();
    }

    public void CreateRegsLookup(int RN, string ss)
    {
        FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
        SW = new StreamWriter(FS);
        int i;
        for (i = 0; i < RN; i++)
        {
            SW.WriteLine("P" + i.ToString());
            SW.WriteLine("T" + i.ToString());
        }
        SW.Close();
        FS.Close();
    }

    public void GenAvMap(int W, int H, Color[,] C, ref int[,] A)
    {
        int i, j;
        A = new int[W, H];
        // AvMap:
        // 0 - UnAvaible; 1 - Sea; 2 - Avaible, 3 - Coast
        Color HMn = Color.FromArgb(196, 128, 128);
        Color Mn = Color.FromArgb(98, 65, 65);
        Color Fr = Color.FromArgb(0, 64, 0);
        Color Oc = Color.FromArgb(64, 0, 0);
        Color DSe = Color.FromArgb(128, 0, 0);
        Color Se = Color.FromArgb(196, 0, 0);
        Hashtable SHsh = new Hashtable();
        Hashtable UHsh = new Hashtable();
        UHsh.Add("HMn", HMn.ToArgb()); UHsh.Add("Mn", Mn.ToArgb()); UHsh.Add("Fr", Fr.ToArgb());
        SHsh.Add("Oc", Oc.ToArgb()); SHsh.Add("DSe", DSe.ToArgb()); SHsh.Add("Se", Se.ToArgb());
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myHgtData[1 + 2 * i, 1 + 2 * j] == 0)
                {
                    AvMap[i, j] = 1;
                }
                else if ((myGTpClr[1 + 2 * i, 1 + 2 * j] == HMn) || (myGTpClr[1 + 2 * i, 1 + 2 * j] == HMn))
                {
                    AvMap[i, j] = 0;
                }
                else
                {
                    AvMap[i, j] = 2;
                }
            }
        }
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                if (myFtrClr[i, j] != Color.FromArgb(0, 0, 0))
                {
                    AvMap[i, j] = 0;
                }
            }
        }
        myAvClr = new Color[W, H];
        // 0 - UnAvaible; 1 - Sea; 2 - Avaible, 3 - Coast
        for (i = 0; i < W; i++)
        {
            for (j = 0; j < H; j++)
            {
                switch (AvMap[i, j])
                {
                    case 0:
                        myAvClr[i, j] = Color.FromArgb(0, 0, 0);
                        break;
                    case 1:
                        myAvClr[i, j] = Color.FromArgb(0, 0, 255);
                        break;
                    case 2:
                        myAvClr[i, j] = Color.FromArgb(0, 255, 0);
                        break;
                    case 3:
                        myAvClr[i, j] = Color.FromArgb(255, 255, 0);
                        break;
                }
            }
        }
    }

    public string RndName_old()
    {
        string ss = ""; int i, j;
        if (fb == null)
        {
            string[] nfb = {"qw", "tr", "c", "pr", "ps", 
                        "sr", "st", "str", "sp", "sv", "sk", "skl", "sl", "sx", "sv", "sn", "sm", 
                        "dr", "dl", "dz", "dv", "fr", "fl", "gw", "gr", "gl", "j", 
                        "kr", "kt", "ks", "kl", "kv", "kš", 
                        "zr", "zl", "xw", "xr", "xt", "vr", "vl", "vz", "bl", "ml", "mb", 
                        "šr", "št", "štr", "šp", "špr", "šk", "škl", "šv", "šn", "šm", 
                        "žd"
                      };
            fb = nfb;
        }
        if (lb == null)
        {
            string[] nlb = { "wt", "wl", "rt", "rp", "rs", "rd", "rg", "rk", "rz", "rx", "rš", 
                        "yt", "yp", "yk", "yn", "ym", "pt", "ps",
                        "st", "sp", "ss", "sk", "ft", "gr", "hr", "ht", "hš", 
                        "kt", "ks", "kx", "kš", "lt", "lp", "ld", 
                        "zd", "zg", "xr", "xt", "xš", "ŋ", "ŋ", "nt", "ns", "nd", "nj", "mb", 
                        "št", "šp", "šk", "žd", "θ"
                      };
            lb = nlb;
        }
        if (bb == null)
        {
            string[] nbb = { "q", "w", "r", "t", "y", "p", 
                        "s", "d", "f", "g", "h", "j", "k", "l", 
                        "z", "x", "v", "b", "n", "m", 
                        "č", "š", "ž", "θ", "đ", 
                    "w", "r", "t", "y", "p", 
                        "s", "d", "f", "g", "h", "j", "k", "l", 
                        "z", "x", "v", "b", "n", "m", 
                        "č", "š",  
                    "r", "t", "y", "p", 
                        "s", "d", "g", "h", "j", "k", "l", 
                        "z", "c", "v", "b", "n", "m", 
                        "č", "š", 
                    "t", "y", "p", 
                        "s", "d", "k", "l", 
                        "v", "b", "n", "m", 
                        "š",
                    "t", "p", 
                        "s", "d", "k",
                        "n", "m", };
            bb = nbb;
        }
        if (aa == null)
        {
            string[] naa = { "a", "e", "o", "ö", "u", "ü", "ı", "i",
                    "a", "a", "a", "e", "e", "o", "o", "u", "i", "i", "i"};
            aa = naa;
        }
        switch (Rnd.Next(20))
        {
            case 1:
                j = 1;
                break;
            case 19:
            case 18:
                j = 4;
                break;
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                j = 3;
                break;
            default:
                j = 2;
                break;
        }
        //j = 1 + Rnd.Next(3);
        for (i = 0; i < j; i++)
        {
            if (Rnd.Next(10) < 2)
            {
                ss += fb[Rnd.Next(fb.Length)];
            }
            else
                if (Rnd.Next(10) < 6)
                {
                    ss += bb[Rnd.Next(bb.Length)];
                }
            {
                ss += aa[Rnd.Next(aa.Length)];
            }
            if (Rnd.Next(10) < 6)
            {
                ss += bb[Rnd.Next(bb.Length)];
            }
            else
                if (Rnd.Next(10) < 4)
                {
                    ss += lb[Rnd.Next(lb.Length)];
                }
        }
        return ss;
    }

    public string RndName()
    {
        string ss = "", ss1 = ""; int i, j, k;
        if (!lettersloaded)
        {
            ss = RndName_old();
        }
        else
        {
            ss = Letters[0][0][0];
            for (i = 0; i < Letters.GetLength(0); i++)
            {
                ss1 = "";
                for (j = 0; j < ss.Length; j++)
                {
                    for (k = 0; k < Letters[i].GetLength(0); k++)
                    {
                        if (Letters[i][k][0] == ss[j].ToString())
                        {
                            ss1 += Letters[i][k][1 + Rnd.Next(Letters[i][k].GetLength(0) - 1)];
                        }
                    }
                }
                ss = ss1;
            }
        }
        if (Letters[0][0][0] == "+")
        {
            char c = ss[0];
            ss = (c.ToString()).ToUpper() + ss.Remove(0, 1);
        }
        return ss;
    }

}