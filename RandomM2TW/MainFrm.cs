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

namespace RandomM2TW
{
    public partial class MainFrm : Form
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

        struct TWPort
        {
            public int X, Y;
            public bool Is;
        };

        struct TWReg
        {
            public Color C;
            public int X, Y;
            public bool Is;
            public TWPort Port;
        };

        string IntDir;
        string UnitDir;
        public StratMap SMap = new StratMap();
        string hmappath = "";

        int nN = 26;

        Random Rnd = new Random();
        FileStream FS;
        StreamWriter SW;
        BinaryWriter BW;
        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OFDlg.ShowDialog();
            //OFDlg_FileOk(object sender, CancelEventArgs e)
            //{
            //    string ss = OFDlg.FileName;
            //    tgaImage = new Paloma.TargaImage(ss);
            //    this.pbx1.Image = tgaImage.Image;
            //    this.Width = pbx1.Image.Width+this.Width-pbx1.Width;
            //    this.Height = pbx1.Image.Height + this.Height - pbx1.Height;
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SFDlg.ShowDialog();
        }

        int TGAColorToInt(Color C)
        {
            return (C.B << 16) + (C.G) + (C.R << 8);
        }

        public Image LoadTGAImage(string path)
        {
            Image img = null;
            FileStream fs = null;
            int Width, Height, bpp, bytesPerPixel, stride;
            byte[] header = new byte[18]; // header
            // ----
            fs = new FileStream(path, FileMode.Open);
            fs.Read(header, 0, 18); // Маркер сдвинут на 18 байтов, так как произошло чтение;
            // ----
            // Определяем ширину TGA (старший байт * 256 + младший байт);
            Width = (header[13] * 256) + header[12];
            // Определяем высоту TGA (старший байт * 256 + младший байт);
            Height = (header[15] * 256) + header[14];
            // ----
            // Получаем TGA бит/пиксель (24 or 32);
            bpp = header[16];
            // Делим на 8 для получения байт/пиксель;
            bytesPerPixel = bpp / 8;
            // Вычисляем количество байт на строке;
            stride = Width * bytesPerPixel;
            // ----
            int x;
            byte temp;
            byte[] line = new byte[stride];
            Bitmap bm = new Bitmap(Width, Height);
            // ----
            for (int y = Height - 1; y >= 0; y--)  // проход по всем строкам
            {
                fs.Read(line, 0, stride);
                x = 0;
                for (int pt = 0; pt < stride; pt += bytesPerPixel)
                {
                    temp = line[pt];
                    line[pt] = line[pt + 2];
                    line[pt + 2] = temp;
                    bm.SetPixel(x, y, Color.FromArgb(line[pt], line[pt + 1], line[pt + 2]));
                    x++;
                }
            }
            // ----
            img = bm;
            return img;
        }

        private void SFDlg_FileOk(object sender, CancelEventArgs e)
        {
            //string ss = SFDlg.FileName;
            //WriteTGA(tgaImage, ss);
            //SW.Close();
            //BW.Close();
            //FS.Close();
        }

        void WriteNewTGA(int W, int H, Color[,] clrData, string ss)
        {
            FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
            BW = new BinaryWriter(FS);

            FS.WriteByte(0);
            FS.WriteByte(0);
            FS.WriteByte(2); //(byte)Paloma.ImageType.UNCOMPRESSED_TRUE_COLOR);

            BW.Write((UInt16)0);
            BW.Write((UInt16)0);
            FS.WriteByte(0);

            BW.Write((UInt16)0);
            BW.Write((UInt16)0);
            BW.Write((UInt16)W);
            BW.Write((UInt16)H);
            FS.WriteByte(24);
            FS.WriteByte(0);

            int i, j;
            UInt24 i24;
            int colorInt;
            Color clr;
            for (j = H - 1; j >= 0; j--)
            {
                for (i = 0; i < W; i++)
                {
                    clr = clrData[i, j];
                    colorInt = TGAColorToInt(clr);
                    i24 = (UInt24)colorInt;
                    BW.Write(i24._mostSignificant);
                    BW.Write(i24._leastSignificant);
                }
            }
            FS.Close();
            BW.Close();
        }

        void CreateEmptyTxtFile(string ss)
        {
            FS = new FileStream(ss, FileMode.Create, FileAccess.Write);
            FS.Close();
        }

        void GenNames()
        {
            int i, j;
            FS = new FileStream(IntDir + "//data//text//names.txt", FileMode.Create, FileAccess.Write);
            SW = new StreamWriter(FS, Encoding.Unicode);
            SW.WriteLine("¬ Names for characters");
            for (i = 0; i < SMap.NFA; i++)
            {
                SMap.Names[i] = "" + (char)((int)'a' + i) + (char)((int)'a' + i) + "a";
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("{" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j) + "}" + SMap.RndName());
                }
                SW.WriteLine("{x" + (char)((int)'a' + i) + (char)((int)'a' + i) + "}" + " ");
                //SW.WriteLine("{w" + (char)((int)'a' + i) + (char)((int)'a' + i) + "}" + "A");
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("{w" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j) + "}" + SMap.RndName());
                }
            }
            SW.WriteLine();
            SW.Close();
            FS.Close();

            FS = new FileStream(IntDir + "//data//descr_names.txt", FileMode.Create, FileAccess.Write);
            SW = new StreamWriter(FS);
            for (i = 0; i < SMap.NFA; i++)
            {
                SW.WriteLine("faction: " + SMap.Facs[i] + Environment.NewLine);
                SW.WriteLine("\tcharacters");
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("\t\t" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j));
                }
                SW.WriteLine();
                SW.WriteLine("\tsurnames");
                SW.WriteLine("\t\tx" + (char)((int)'a' + i) + (char)((int)'a' + i));
                SW.WriteLine();
                SW.WriteLine("\twomen");
                //SW.WriteLine("\t\tw" + (char)((int)'a' + i) + (char)((int)'a' + i));
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("\t\tw" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j));
                }
                SW.WriteLine();
            }
            SW.Close();
            FS.Close();

            FS = new FileStream(IntDir + "//data//descr_names_lookup.txt", FileMode.Create, FileAccess.Write);
            SW = new StreamWriter(FS);
            for (i = 0; i < SMap.NFA; i++)
            {
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j));
                }
                SW.WriteLine("x" + (char)((int)'a' + i) + (char)((int)'a' + i));
                //SW.WriteLine("w" + (char)((int)'a' + i) + (char)((int)'a' + i));
                for (j = 0; j < nN; j++)
                {
                    SW.WriteLine("w" + (char)((int)'a' + i) + (char)((int)'a' + i) + (char)((int)'a' + j));
                }
            }
            SW.Close();
            FS.Close();

            //factions
            //if (false)
            //{
            //    for (i = 0; i < SMap.NFA; i++)
            //    {
            //        SMap.FacsNames[i] = SMap.RndName();
            //    }

            //    if (File.Exists(UnitDir + "//expanded_bi.txt"))
            //    {
            //        string sfs;
            //        FS = new FileStream(IntDir + "//data//text//expanded_bi.txt", FileMode.Create, FileAccess.Write);
            //        SW = new StreamWriter(FS, Encoding.Unicode);
            //        FileStream FSR = new FileStream(UnitDir + "//expanded_bi.txt", FileMode.Open, FileAccess.Read);
            //        StreamReader SR = new StreamReader(FSR);
            //        while (!SR.EndOfStream)
            //        {
            //            sfs = SR.ReadLine();
            //            for (i = 0; i < SMap.NFA; i++)
            //            {
            //                sfs = sfs.Replace("" + (char)((int)'A' + i) + (char)((int)'A' + i) + (char)((int)'A' + i), SMap.FacsNames[i]);
            //            }
            //            SW.WriteLine(sfs);
            //        }
            //        SR.Close();
            //        FSR.Close();
            //        SW.Close();
            //        FS.Close();
            //    }
            //}
        }

        void CopyFolder(string begin_dir, string end_dir)
        {
            if (Directory.Exists(end_dir) != true)
            {
                Directory.CreateDirectory(end_dir);
            }
            DirectoryInfo dir_inf = new DirectoryInfo(begin_dir);
            foreach (DirectoryInfo dir in dir_inf.GetDirectories())
            {
                if (Directory.Exists(end_dir + "\\" + dir.Name) != true)
                {
                    Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }
                CopyFolder(dir.FullName, end_dir + "\\" + dir.Name);
            }
            foreach (string file in Directory.GetFiles(begin_dir))
            {
                string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                File.Copy(file, end_dir + "\\" + filik, true);
            }
        }

        void CopyFiles(string dir)
        {
            int i, m; string s;
            if (Directory.Exists(dir))
            {
                string[] allfiles = Directory.GetFiles(dir);
                for (i = 0; i < allfiles.Length; i++)
                {
                    m = allfiles[i].LastIndexOf("\\");
                    s = allfiles[i].Substring(m, allfiles[i].Length - m);
                    s = s.Replace("\\", "");
                    if (s.StartsWith("~"))
                    {
                        File.Copy(allfiles[i], IntDir + "//data//" + s.Substring(1, s.Length - 1), true);
                    }
                }
                allfiles = Directory.GetDirectories(dir);
                for (i = 0; i < allfiles.Length; i++)
                {
                    m = allfiles[i].LastIndexOf("\\");
                    s = allfiles[i].Substring(m, allfiles[i].Length - m);
                    s = s.Replace("\\", "");
                    if (s.StartsWith("~"))
                    {
                        CopyFolder(allfiles[i], IntDir + "//data//" + s.Substring(1, s.Length - 1));
                    }
                }
            }
        }

        bool LoadLetters()
        {
            int i, j, k, l; string s;
            ArrayList AL = new ArrayList();
            ArrayList Al = new ArrayList();
            ArrayList al = new ArrayList();
            if (File.Exists(UnitDir + "//letters.txt"))
            {
                FileStream FS = new FileStream(UnitDir + "//letters.txt", FileMode.Open);
                StreamReader SR = new StreamReader(FS);
                while (!SR.EndOfStream)
                {
                    AL.Add(SR.ReadLine());
                }
                for (i = AL.Count - 1; i >= 0; i--)
                {
                    if (!((string)AL[i]).StartsWith("["))
                    {
                        AL.RemoveAt(i);
                    }
                }
                if (AL.Count < 1) return false;
                SMap.Letters = new string[AL.Count][][];

                for (i = 0; i < AL.Count; i++)
                {
                    k = 0; l = 0;
                    for (j = 0; j < ((string)AL[i]).Length; j++)
                    {
                        if (((string)AL[i])[j] == '[') k++;
                        if (((string)AL[i])[j] == ']') l++;
                    }
                    if (k != l) return false;
                    SMap.Letters[i] = new string[k][];

                    Al = new ArrayList();
                    s = "";
                    for (j = 0; j < ((string)AL[i]).Length; j++)
                    {
                        if (((string)AL[i])[j] == ' ')
                        {
                            if (s != "")
                            {
                                Al.Add(s);
                                s = "";
                            }
                        }
                        else
                        {
                            s += ((string)AL[i])[j];
                        }
                    }
                    Al.Add(s);

                    k = 0;
                    for (j = 0; j < Al.Count; j++)
                    {
                        if (((string)Al[j]).StartsWith("["))
                        {
                            if (j > 0)
                            {
                                SMap.Letters[i][k] = new string[al.Count];
                                SMap.Letters[i][k][0] = ((string)al[0]).Substring(1, ((string)al[0]).Length - 2);
                                for (l = 1; l < al.Count; l++)
                                {
                                    SMap.Letters[i][k][l] = (string)al[l];
                                }
                                k++;
                            }
                            al = new ArrayList();
                            al.Add(Al[j]);
                        }
                        else
                        {
                            al.Add(Al[j]);
                        }
                    }
                    SMap.Letters[i][k] = new string[al.Count];
                    SMap.Letters[i][k][0] = ((string)al[0]).Substring(1, ((string)al[0]).Length - 2);
                    for (l = 1; l < al.Count; l++)
                    {
                        SMap.Letters[i][k][l] = (string)al[l];
                    }
                }

                SR.Close();
                FS.Close();
                return true;
            }
            return false;
        }

        int[] StringToIntArray(string s, int k)
        {
            string ss = s, js; int j = 0;
            bool b = true;
            ArrayList A = new ArrayList();
            int i;
            while (ss != "")
            {
                ss = ss.Trim();
                i = ss.IndexOf(" ");
                if (i == -1)
                {
                    js = ss;
                    ss = "";
                }
                else
                {
                    js = ss.Substring(0, i);
                    ss = ss.Substring(i, ss.Length - i);
                }
                b = b && int.TryParse(js, out j);
                if (b) A.Add(j);
            }
            int[] Res = new int[SMap.NFA];
            if (b)
            {
                for (i = 0; i < SMap.NFA; i++)
                {
                    Res[i] = (int)A[i];
                }
            }
            else
            {
                if (k == 0)
                    return SMap.def_clms;
                else
                    return SMap.def_clms2;
            }
            return Res;
        }

        double[] StringToDoubleArray(string s, int k)
        {
            string ss = s, js; double j = 0;
            bool b = true, bb = true;
            ArrayList A = new ArrayList();
            int i;
            while (ss != "")
            {
                ss = ss.Trim();
                i = ss.IndexOf(" ");
                if (i == -1)
                {
                    js = ss;
                    ss = "";
                }
                else
                {
                    js = ss.Substring(0, i);
                    ss = ss.Substring(i, ss.Length - i);
                }
                bb = double.TryParse(js, out j);
                if (!bb) bb = double.TryParse(js.Replace('.', ','), out j);
                b = b && bb;
                if (b) A.Add(j);
            }
            double[] Res = new double[12];
            if (b)
            {
                for (i = 0; i < 12; i++)
                {
                    Res[i] = (double)A[i];
                }
            }
            else
            {
                if (k == 2)
                    return SMap.vers;
                else if (k == 3)
                    return SMap.forest_vers;
                else
                    return SMap.clmpopkf;
            }
            return Res;
        }

        void ReadGeographyInf()
        {
            string ss;
            int i = 0;
            SMap.def_clms = new int[SMap.NFA];
            SMap.def_clms2 = new int[SMap.NFA];
            FS = new FileStream(UnitDir + "//geography_inf.txt", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            while (!SR.EndOfStream)
            {
                ss = SR.ReadLine();
                if ((ss != "") && (!ss.StartsWith("//")))
                {
                    switch (i)
                    {
                        case 0: SMap.def_clms = StringToIntArray(ss, i); i++; break;
                        case 1: SMap.def_clms2 = StringToIntArray(ss, i); i++; break;
                        case 2: SMap.vers = StringToDoubleArray(ss, i); i++; break;
                        case 3: SMap.forest_vers = StringToDoubleArray(ss, i); i++; break;
                        case 4: SMap.clmpopkf = StringToDoubleArray(ss, i); i++; break;
                        default: break;
                    }
                }
            }
            SR.Close();
            FS.Close();
        }

        void CreateBatCfg()
        {
            FS = new FileStream(IntDir + "//" + IntDir + ".bat", FileMode.Create, FileAccess.Write);
            StreamWriter SW = new StreamWriter(FS);
            SW.WriteLine("cd ..\\..");
            SW.WriteLine("kingdoms.exe @mods\\" + IntDir + "\\" + IntDir + ".cfg");
            SW.Close();
            FS.Close();
            FS = new FileStream(IntDir + "//" + IntDir + ".cfg", FileMode.Create, FileAccess.Write);
            SW = new StreamWriter(FS);
            SW.Write("[features]" + Environment.NewLine +
"## enable battle editor" + Environment.NewLine +
"editor		= true" + Environment.NewLine +
"## relative mod path" + Environment.NewLine +
"mod			= mods/" + IntDir + Environment.NewLine +
    "" + Environment.NewLine +
"[misc]" + Environment.NewLine +
"## display date and season on campaign hud" + Environment.NewLine +
"show_hud_date	= true" + Environment.NewLine +
"" + Environment.NewLine +
"[game]" + Environment.NewLine +
"## allow an unlimited number of men on the battlefield for campaign battles" + Environment.NewLine +
"unlimited_men_on_battlefield	= true" + Environment.NewLine +
"" + Environment.NewLine +
"[log]" + Environment.NewLine +
"## log potentially critical errors for debugging" + Environment.NewLine +
"# to			= logs/system.log.txt" + Environment.NewLine +
"# level			= * trace" + Environment.NewLine +
"" + Environment.NewLine +
"[video]" + Environment.NewLine +
"## run game in windowed mode" + Environment.NewLine +
"# windowed			= true" + Environment.NewLine +
"## disable movies" + Environment.NewLine +
"# movies			= false" + Environment.NewLine +
"" + Environment.NewLine +
"[hotseat]" + Environment.NewLine +
"## disable start turn scroll in hotseat campaign" + Environment.NewLine +
"# scroll	= false" + Environment.NewLine +
"## disable forced separate human faction turns (including diplomacy) in hotseat campaign" + Environment.NewLine +
"# turns	= false" + Environment.NewLine +
"## enable cheat console in hotseat campaign." + Environment.NewLine +
"# disable_console			= false" + Environment.NewLine +
"## specify password for administrator access to the dev console when console disabled. Change 'password' to a suitable password" + Environment.NewLine +
"# admin_password			= password" + Environment.NewLine +
"## enable camera updates during ai turn in hotseat campaign" + Environment.NewLine +
"# update_ai_camera		= true" + Environment.NewLine +
"## enable voting in papal elections in hotseat campaign (only first valid human faction votes)" + Environment.NewLine +
"# disable_papal_elections	= false" + Environment.NewLine +
"## disable forced autoresolve all battles in hotseat campaign" + Environment.NewLine +
"# autoresolve_battles		= false" + Environment.NewLine +
"## disable diplomacy validation for incoming propositions" + Environment.NewLine +
"# validate_diplomacy		= false" + Environment.NewLine +
"## disable forced relevant hotseat options to be saved with game." + Environment.NewLine +
"# save_prefs			= false" + Environment.NewLine +
"## autosave hotseat game at start of players turn" + Environment.NewLine +
"# autosave				= true" + Environment.NewLine +
"## save config file in save dir containing information about next players turn" + Environment.NewLine +
"# save_config			= true" + Environment.NewLine +
"## close medieval II directly after a hotseat autosave" + Environment.NewLine +
"# close_after_save		= true" + Environment.NewLine +
"## sub directory name for hotseat save games" + Environment.NewLine +
"# gamename				= hotseat_gamename" + Environment.NewLine +
"## ensure game data files used in previous save match current campaign data files." + Environment.NewLine +
"# validate_data			= true" + Environment.NewLine +
"## prevent game to load if savegame or data validations fail" + Environment.NewLine +
"# allow_validation_failures	= false" + Environment.NewLine
);
            SW.Close();
            FS.Close();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            tlStrSttLbl.Visible = true; tlStrPrgrssBr.Width = this.Width * 2 / 3;
            tlStrPrgrssBr.Visible = true;
            sttStr.Show(); //sttbar max = 100;
            tlStrSttLbl.Text = "initialization"; Application.DoEvents(); tlStrPrgrssBr.Value = 0;
            if (chbx_hist.Checked) SMap.hist = true; else SMap.hist = false;
            //if (chbx_Pangea.Checked) SMap.island = true; else SMap.island = false;
            SMap.land_type = cbx_land.SelectedIndex;
            if (chbx_names.Checked) SMap.new_names = true; else SMap.new_names = false;
            SMap.garrs = true;
            //this.Cursor = Cursors.WaitCursor;
            SMap.pw = double.Parse(tbx_1_pw.Text);
            SMap.r = int.Parse(tbx_1_r.Text);
            SMap.cP = true; // chbx_P.Checked;
            SMap.Rivers = int.Parse(tbx_rvr.Text);
            IntDir = tbx_Name.Text;
            UnitDir = tbxUnits.Text;
            int W = int.Parse(tbx_x.Text);
            int H = int.Parse(tbx_y.Text);
            SMap.RFN = int.Parse(tbx_pf.Text);
            SMap.W = W;
            SMap.H = H;
            SMap.RN = int.Parse(tbx_Regs.Text);
            SMap.FN = int.Parse(tbx_F.Text);
            SMap.Dph = int.Parse(tbx_Dph.Text);
            SMap.money = int.Parse(tbx_money.Text);
            if (File.Exists(UnitDir + "//geography_inf.txt"))
            {
                ReadGeographyInf();
            }
            tlStrSttLbl.Text = "creating directories"; Application.DoEvents(); tlStrPrgrssBr.Value = 5;
            if (hmappath != "")
            {
                SMap.hmapfrompath = LoadTGAImage(hmappath);
                W = (SMap.hmapfrompath.Width - 1) / 2;
                H = (SMap.hmapfrompath.Height - 1) / 2;
                SMap.W = W;
                SMap.H = H;
                SMap.GenHgtsfrompath();
            }
            if (!Directory.Exists(IntDir))
            {
                Directory.CreateDirectory(IntDir);
            }
            if (!Directory.Exists(IntDir + "//data"))
            {
                Directory.CreateDirectory(IntDir + "//data");
            }
            if (!Directory.Exists(IntDir + "//data//text"))
            {
                Directory.CreateDirectory(IntDir + "//data//text");
            }
            if (!Directory.Exists(IntDir + "//data//world"))
            {
                Directory.CreateDirectory(IntDir + "//data//world");
            }
            if (!Directory.Exists(IntDir + "//data//world//maps"))
            {
                Directory.CreateDirectory(IntDir + "//data//world//maps");
            }
            if (!Directory.Exists(IntDir + "//data//world//maps//base"))
            {
                Directory.CreateDirectory(IntDir + "//data//world//maps//base");
            }
            if (!Directory.Exists(IntDir + "//data//world//maps//campaign"))
            {
                Directory.CreateDirectory(IntDir + "//data//world//maps//campaign");
            }
            if (!Directory.Exists(IntDir + "//data//world//maps//campaign//imperial_campaign"))
            {
                Directory.CreateDirectory(IntDir + "//data//world//maps//campaign//imperial_campaign");
            }
            if (Directory.Exists(IntDir + "//data//ui"))
            {
                Directory.Delete(IntDir + "//data//ui", true);
            }
            CopyFiles(UnitDir);
            CreateBatCfg();

            //maps
            tlStrSttLbl.Text = "heights"; Application.DoEvents(); tlStrPrgrssBr.Value = 10;
            if (hmappath == "")
            {
                SMap.GenHgts1(2 * W + 1, 2 * H + 1, ref SMap.myHgtData);
                SMap.HgtToClr(2 * W + 1, 2 * H + 1, SMap.myHgtData, ref SMap.myHgtClr);//heights
            }
            tlStrSttLbl.Text = "ground_types"; Application.DoEvents(); tlStrPrgrssBr.Value = 15;
            SMap.HgtToGTpClr(2 * W + 1, 2 * H + 1, SMap.myHgtData, ref SMap.myGTpClr);//ground_types
            tlStrSttLbl.Text = "climates"; Application.DoEvents(); tlStrPrgrssBr.Value = 20;
            SMap.GenClmClr(2 * W + 1, 2 * H + 1, ref SMap.myClmClr);//climates
            tlStrSttLbl.Text = "roughness"; Application.DoEvents(); tlStrPrgrssBr.Value = 25;
            SMap.GenRghClr(2 * W, 2 * H, ref SMap.myRghClr);//roughness
            tlStrSttLbl.Text = "fog"; Application.DoEvents(); tlStrPrgrssBr.Value = 27;
            SMap.GenFogClr(2 * W + 1, 2 * H + 1, ref SMap.myFogClr);//fog
            tlStrSttLbl.Text = "features"; Application.DoEvents(); tlStrPrgrssBr.Value = 30;
            SMap.GenFtrClr(W, H, ref SMap.myFtrClr);//features
            tlStrSttLbl.Text = "forests"; Application.DoEvents(); tlStrPrgrssBr.Value = 35;
            SMap.GenForests(2 * W + 1, 2 * H + 1);//forests
            tlStrSttLbl.Text = "water_surface"; Application.DoEvents(); tlStrPrgrssBr.Value = 40;
            SMap.GenWSfClr(256, 256, ref SMap.myWSfClr);//water_surface
            tlStrSttLbl.Text = "trade_routes"; Application.DoEvents(); tlStrPrgrssBr.Value = 40;
            SMap.GenTRtClr(W, H, ref SMap.myTRtClr);//trade_routes
            tlStrSttLbl.Text = "disasters"; Application.DoEvents(); tlStrPrgrssBr.Value = 40;
            SMap.GenDisClr(W, H, ref SMap.myDisClr);//disasters
            tlStrSttLbl.Text = "map of available lands"; Application.DoEvents(); tlStrPrgrssBr.Value = 40;
            SMap.GenAvMap(W, H, SMap.myGTpClr, ref SMap.AvMap);
            tlStrSttLbl.Text = "writing maps"; Application.DoEvents(); tlStrPrgrssBr.Value = 45;
            SMap.GenRadarMaps(W, H);

            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myHgtClr, IntDir + "//data//world//maps//base//map_heights" + ".tga");
            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myGTpClr, IntDir + "//data//world//maps//base//map_ground_types" + ".tga");
            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myClmClr, IntDir + "//data//world//maps//base//map_climates" + ".tga");
            WriteNewTGA(2 * W, 2 * H, SMap.myRghClr, IntDir + "//data//world//maps//base//map_roughness" + ".tga");
            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myFogClr, IntDir + "//data//world//maps//base//map_fog" + ".tga");
            WriteNewTGA(W, H, SMap.myFtrClr, IntDir + "//data//world//maps//base//map_features" + ".tga");
            WriteNewTGA(256, 256, SMap.myWSfClr, IntDir + "//data//world//maps//base//water_surface" + ".tga");
            WriteNewTGA(W, H, SMap.myTRtClr, IntDir + "//data//world//maps//base//map_trade_routes" + ".tga");
            WriteNewTGA(W, H, SMap.myDisClr, IntDir + "//data//world//maps//campaign//imperial_campaign//disasters" + ".tga");
            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myHmdClr, IntDir + "//data//world//maps//base//map_rainfall" + ".tga");
            WriteNewTGA(2 * W + 1, 2 * H + 1, SMap.myTmpClr, IntDir + "//data//world//maps//base//map_temperatures" + ".tga");
            WriteNewTGA(W, H, SMap.myAvClr, IntDir + "//data//world//maps//base//AvMap" + ".tga");
            WriteNewTGA(W, H, SMap.myRdrClr1, IntDir + "//data//world//maps//campaign//imperial_campaign//radar_map1" + ".tga");
            WriteNewTGA(2 * W, 2 * H, SMap.myRdrClr2, IntDir + "//data//world//maps//campaign//imperial_campaign//radar_map2" + ".tga");

            //text files
            tlStrSttLbl.Text = "names"; Application.DoEvents(); tlStrPrgrssBr.Value = 50;
            SMap.lettersloaded = LoadLetters();
            if (SMap.new_names)
            {
                GenNames();
            }
            else
            {
                SMap.Names = SMap.Names_init;
                if (File.Exists(IntDir + "//data//text//names.txt"))
                {
                    File.Delete(IntDir + "//data//text//names.txt");
                }
                if (File.Exists(IntDir + "//data//descr_names.txt"))
                {
                    File.Delete(IntDir + "//data//descr_names.txt");
                }
                if (File.Exists(IntDir + "//data//descr_names_lookup.txt"))
                {
                    File.Delete(IntDir + "//data//descr_names_lookup.txt");
                }
            }
            tlStrSttLbl.Text = "empty txt files"; Application.DoEvents(); tlStrPrgrssBr.Value = 55;
            CreateEmptyTxtFile(IntDir + "//data//world//maps//base//descr_disasters.txt");
            CreateEmptyTxtFile(IntDir + "//data//world//maps//campaign//imperial_campaign//descr_events.txt");
            CreateEmptyTxtFile(IntDir + "//data//world//maps//campaign//imperial_campaign//descr_win_conditions.txt");
            CreateEmptyTxtFile(IntDir + "//data//world//maps//base//descr_sounds_music_types.txt");
            SMap.CreateDscTrn(W, H, IntDir + "//data//world//maps//base//descr_terrain.txt");

            //regions
            tlStrSttLbl.Text = "regions"; Application.DoEvents(); tlStrPrgrssBr.Value = 55;
            SMap.GenRegions(W, H, SMap.RN, ref SMap.Regs, ref SMap.myRegClr);
            tlStrSttLbl.Text = "writing text files"; Application.DoEvents(); tlStrPrgrssBr.Value = 85;
            WriteNewTGA(W, H, SMap.myRegClr, IntDir + "//data//world//maps//base//map_regions" + ".tga");
            SMap.GenFacClr(W, H, ref SMap.myFacClr);
            SMap.GenFEClr(W, H);
            WriteNewTGA(W, H, SMap.myFacClr, IntDir + "//data//world//maps//base//map_factions" + ".tga");
            SMap.CreateDscReg(SMap.RN, SMap.Regs, IntDir + "//data//world//maps//base//descr_regions.txt");
            SMap.CreateDscStrat(H, SMap.RN, SMap.FN, SMap.Regs, IntDir + "//data//world//maps//campaign//imperial_campaign//descr_strat.txt");
            SMap.CreateRegsTxt(SMap.RN, IntDir + "//data//text//imperial_campaign_regions_and_settlement_names.txt");
            SMap.CreateRegsLookup(SMap.RN, IntDir + "//data//world//maps//campaign//imperial_campaign//descr_regions_and_settlement_name_lookup.txt");
            WriteNewTGA(W, H, SMap.MainClmClr, IntDir + "//data//world//maps//base//map_main_climates" + ".tga");
            WriteNewTGA(W, H, SMap.myPopClr, IntDir + "//data//world//maps//base//map_population" + ".tga");
            WriteNewTGA(W, H, SMap.myFEClr, IntDir + "//data//world//maps//base//map_FE" + ".tga");

            
            tlStrSttLbl.Text = "drawing maps"; Application.DoEvents(); tlStrPrgrssBr.Value = 85;
            string olds = Directory.GetCurrentDirectory();
            string news = IntDir + "//data//world//maps//base";
            //Directory.SetCurrentDirectory(news);
            //pbx1.Image = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_heights.tga");
            pbx1.Image = LoadTGAImage(IntDir + "/data/world/maps/base/map_heights.tga");
            //pbx2.Image = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_ground_types.tga");
            pbx2.Image = LoadTGAImage(IntDir + "/data/world/maps/base/map_ground_types.tga");
            //pbx3.Image = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_climates.tga");
            pbx3.Image = LoadTGAImage(IntDir + "/data/world/maps/base/map_climates.tga");
            //pbx4.BackgroundImage = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_regions.tga");
            pbx4.BackgroundImage = LoadTGAImage(IntDir + "/data/world/maps/base/map_regions.tga");
            pbx4.BackgroundImageLayout = ImageLayout.Stretch;
            //pbx5.BackgroundImage = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_features.tga");
            pbx5.BackgroundImage = LoadTGAImage(IntDir + "/data/world/maps/base/map_features.tga");
            pbx5.BackgroundImageLayout = ImageLayout.Stretch;
            //pbx6.BackgroundImage = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_factions.tga");
            pbx6.BackgroundImage = LoadTGAImage(IntDir + "/data/world/maps/base/map_factions.tga");
            pbx6.BackgroundImageLayout = ImageLayout.Stretch;
            //pbx7.BackgroundImage = Paloma.TargaImage.LoadTargaImage(IntDir + "/data/world/maps/base/map_rainfall.tga");
            pbx7.BackgroundImage = LoadTGAImage(IntDir + "/data/world/maps/base/map_population.tga");
            pbx7.BackgroundImageLayout = ImageLayout.Stretch;
            //Directory.SetCurrentDirectory(olds);
            int oldy = pbx1.Height;
            pbx1.Height = pbx1.Image.Height;
            pbx1.Width = pbx1.Image.Width;
            pbx2.Height = pbx1.Image.Height;
            pbx2.Width = pbx1.Image.Width;
            pbx3.Height = pbx1.Image.Height;
            pbx3.Width = pbx1.Image.Width;
            pbx4.Height = pbx1.Image.Height - 1;
            pbx4.Width = pbx1.Image.Width - 1;
            pbx5.Height = pbx1.Image.Height - 1;
            pbx5.Width = pbx1.Image.Width - 1;
            pbx6.Height = pbx1.Image.Height;
            pbx6.Width = pbx1.Image.Width;
            pbx7.Height = pbx1.Image.Height;
            pbx7.Width = pbx1.Image.Width;
            tabMaps.Width = pbx1.Width + 8;
            tabMaps.Height = pbx1.Height + 26;
            this.Height += pbx1.Height - oldy;
            //this.Cursor = Cursors.Arrow;
            tlStrSttLbl.Text = "done^^"; Application.DoEvents(); tlStrPrgrssBr.Value = 100;
            tlStrPrgrssBr.Visible = false; //sttStr.Hide();
            hmappath = "";
        }

        private void FFbtn_OK_Click(object sender, EventArgs e)
        {
            int i, k = 0;
            for (i = 0; i < SMap.NFA; i++)
            {
                if (((CheckBox)((Button)sender).Parent.Controls[i]).Checked)
                {
                    k++;
                    SMap.FIs[i] = true;
                }
                else
                {
                    SMap.FIs[i] = false;
                }
            }
            tbx_F.Text = k.ToString();
            ((Form)((Button)sender).Parent).Close();
        }

        private void btn_CstmFacs_Click(object sender, EventArgs e)
        {
            Form FacsForm = new Form();
            int i;
            CheckBox[] FFchbxs = new CheckBox[SMap.NFA];
            for (i = 0; i < SMap.NFA; i++)
            {
                FFchbxs[i] = new CheckBox();
                FFchbxs[i].Left = 35 + 120 * (i / 11);
                FFchbxs[i].Top = 20 + 20 * (i % 11);
                FFchbxs[i].Text = SMap.Facs[i];
                FFchbxs[i].BackColor = SMap.FacClrs[i];
                if (SMap.FIs[i]) FFchbxs[i].Checked = true;
                FacsForm.Controls.Add(FFchbxs[i]);
            }
            Button FFbtn_OK = new Button();
            FFbtn_OK.Left = 150;
            FFbtn_OK.Top = 230;
            FFbtn_OK.Text = "OK";
            FFbtn_OK.Click += FFbtn_OK_Click;
            FacsForm.Controls.Add(FFbtn_OK);
            SMap.fautodef = false;
            FacsForm.ShowDialog();

        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        private void tbx_F_TextChanged(object sender, EventArgs e)
        {
            int x, i;
            if (int.TryParse(tbx_F.Text, out x) && SMap.fautodef)
            {
                if ((x > 2) && (x < 20))
                {
                    for (i = 0; i < x; i++)
                    {
                        SMap.FIs[i] = true;
                    }
                    for (i = x; i < SMap.NFA; i++)
                    {
                        SMap.FIs[i] = false;
                    }
                }
            }
            SMap.fautodef = true;
        }

        private void btn_fExHMap_Click(object sender, EventArgs e)
        {
            if (OFDlg.ShowDialog() == DialogResult.OK)
            {
                hmappath = OFDlg.FileName;
                btnGen.PerformClick();
            }
        }

        private void CCbtn_OK_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < 7; i++)
            {
                SMap.clm_hmd_Dw[i] = int.Parse((((TextBox)((Button)sender).Parent.Controls[i]).Text));
                SMap.clm_hmd_De[i] = int.Parse((((TextBox)((Button)sender).Parent.Controls[7 + i]).Text));
            }
            SMap.clm_tmp_c = int.Parse((((TextBox)((Button)sender).Parent.Controls[14]).Text));
            SMap.clm_tmp_C = int.Parse((((TextBox)((Button)sender).Parent.Controls[15]).Text));
            ((Form)((Button)sender).Parent).Close();
        }

        private void btn_Clm_Click(object sender, EventArgs e)
        {
            Form ClmForm = new Form();
            ClmForm.Height = 350;
            int i;

            TextBox[] HmdTbxs = new TextBox[14];
            for (i = 0; i < 7; i++)
            {
                HmdTbxs[i] = new TextBox();
                HmdTbxs[i].Left = 20;
                HmdTbxs[i].Top = 50 + 20 * i;
                HmdTbxs[i].Text = SMap.clm_hmd_Dw[i].ToString();
                ClmForm.Controls.Add(HmdTbxs[i]);
            }
            for (i = 7; i < 14; i++)
            {
                HmdTbxs[i] = new TextBox();
                HmdTbxs[i].Left = 130;
                HmdTbxs[i].Top = 50 + 20 * (i - 7);
                HmdTbxs[i].Text = SMap.clm_hmd_De[i - 7].ToString();
                ClmForm.Controls.Add(HmdTbxs[i]);
            }

            TextBox tbx_c = new TextBox();
            tbx_c.Left = 20;
            tbx_c.Top = 240;
            tbx_c.Text = SMap.clm_tmp_c.ToString();
            ClmForm.Controls.Add(tbx_c);

            TextBox tbx_C = new TextBox();
            tbx_C.Left = 130;
            tbx_C.Top = 240;
            tbx_C.Text = SMap.clm_tmp_C.ToString();
            ClmForm.Controls.Add(tbx_C);

            Button CCbtn_OK = new Button();
            CCbtn_OK.Left = 150;
            CCbtn_OK.Top = 270;
            CCbtn_OK.Text = "OK";
            CCbtn_OK.Click += CCbtn_OK_Click;
            ClmForm.Controls.Add(CCbtn_OK);

            Label CClbl_r = new Label();
            CClbl_r.Left = 70;
            CClbl_r.Top = 10;
            CClbl_r.Text = "cyclones power";
            ClmForm.Controls.Add(CClbl_r);

            Label CClbl_w = new Label();
            CClbl_w.Left = 20;
            CClbl_w.Top = 30;
            CClbl_w.Text = "west";
            ClmForm.Controls.Add(CClbl_w);

            Label CClbl_e = new Label();
            CClbl_e.Left = 130;
            CClbl_e.Top = 30;
            CClbl_e.Text = "east";
            ClmForm.Controls.Add(CClbl_e);

            Label CClbl_t = new Label();
            CClbl_t.Left = 70;
            CClbl_t.Top = 200;
            CClbl_t.Text = "temperature";
            ClmForm.Controls.Add(CClbl_t);

            Label CClbl_n = new Label();
            CClbl_n.Left = 20;
            CClbl_n.Top = 220;
            CClbl_n.Text = "north";
            ClmForm.Controls.Add(CClbl_n);

            Label CClbl_s = new Label();
            CClbl_s.Left = 130;
            CClbl_s.Top = 220;
            CClbl_s.Text = "south";
            ClmForm.Controls.Add(CClbl_s);

            SMap.fautodef = false;
            ClmForm.ShowDialog();
        }

        private void tbx_Dph_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

