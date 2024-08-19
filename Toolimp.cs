using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Authentication;
using cjpc.utils.dbutils;
using System.Net;
using System.Net.Sockets;


namespace cjpc.utils
{
    class Toolimp
    {
        public static DBClass dbclass = new DBClass();

        /// <summary>
        /// 计算中位数
        /// </summary>
        /// <param name="arr">数组</param>
        /// <returns></returns>
        public static double Median(double[] arr)
        {
            //为了不修改arr值，对数组的计算和修改在tempArr数组中进行
            double[] tempArr = new double[arr.Length];
            arr.CopyTo(tempArr, 0);

            //对数组进行排序
            double temp;
            for (int i = 0; i < tempArr.Length; i++)
            {
                for (int j = i; j < tempArr.Length; j++)
                {
                    if (tempArr[i] > tempArr[j])
                    {
                        temp = tempArr[i];
                        tempArr[i] = tempArr[j];
                        tempArr[j] = temp;
                    }
                }
            }


            //针对数组元素的奇偶分类讨论
            if (tempArr.Length % 2 != 0)
            {
                return tempArr[arr.Length / 2 + 1];
            }
            else
            {
                return (tempArr[tempArr.Length / 2] +
                    tempArr[tempArr.Length / 2 + 1]) / 2;
            }
        }

        public static double Round(double v, int x)
        {
            bool isNegative = false;
            //如果是负数
            if (v < 0)
            {
                isNegative = true;
                v = -v;
            }
            int IValue = 1;
            for (int i = 1; i <= x; i++)
            {
                IValue = IValue * 10;
            }
            double Int = Math.Round(v * IValue + 0.5, 0);
            v = Int / IValue;
            if (isNegative)
            {
                v = -v;
            }
            return v;
        }

        //高分组
        public static double HighScore(double[] arr)
        {
            //为了不修改arr值，对数组的计算和修改在tempArr数组中进行
            double[] tempArr = new double[arr.Length];
            arr.CopyTo(tempArr, 0);

            //对数组进行排序
            double temp;
            for (int i = 0; i < tempArr.Length; i++)
            {
                for (int j = i; j < tempArr.Length; j++)
                {
                    if (tempArr[i] > tempArr[j])
                    {
                        temp = tempArr[i];
                        tempArr[i] = tempArr[j];
                        tempArr[j] = temp;
                    }
                }
            }

            int a = Convert.ToInt32(arr.Length * 0.73);
            return tempArr[a];
        }

        //低分组
        public static double LesserScore(double[] arr)
        {
            //为了不修改arr值，对数组的计算和修改在tempArr数组中进行
            double[] tempArr = new double[arr.Length];
            arr.CopyTo(tempArr, 0);

            //对数组进行排序
            double temp;
            for (int i = 0; i < tempArr.Length; i++)
            {
                for (int j = i; j < tempArr.Length; j++)
                {
                    if (tempArr[i] > tempArr[j])
                    {
                        temp = tempArr[i];
                        tempArr[i] = tempArr[j];
                        tempArr[j] = temp;
                    }
                }
            }

            int a = Convert.ToInt32(arr.Length * 0.27);
            return tempArr[a];
        }

        //排序
        public static double[] Sort(double[] arr)
        {
            //为了不修改arr值，对数组的计算和修改在tempArr数组中进行
            double[] tempArr = new double[arr.Length];
            arr.CopyTo(tempArr, 0);

            //对数组进行排序
            double temp;
            for (int i = 0; i < tempArr.Length; i++)
            {
                for (int j = i; j < tempArr.Length; j++)
                {
                    if (tempArr[i] > tempArr[j])
                    {
                        temp = tempArr[i];
                        tempArr[i] = tempArr[j];
                        tempArr[j] = temp;
                    }
                }
            }

            return tempArr;
        }

        //选出最大值
        public static double MaxNum(double[] arr)
        {
           
            double temp=-1;
            for (int i = 0; i < arr.Length; i++)
            {
                if(arr[i] > temp)
                {
                    temp = arr[i];
                }
            }

            return temp;
        }


        //平均分
        public static double Avg(double[] arr, int nstart, int nend)
        {
            double sum = 0;
            int len = 0;
            for (int i = nstart; i < nend; i++)
            {
                sum += arr[i];
                len++;
            }

            try
            {
                return sum / len;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //获取本机ip
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }



        //判断光盘是否存在
        public static bool IsThereAnyCDRom()
        {
            bool flgResult = false;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "CDRom")
                {
                    if (d.IsReady)
                    {
                        flgResult = true;
                        break;
                    }
                }
            }
            return flgResult;
        }

        //获取所有的cdrom
        public static List<DriveInfo> AllCDRomNames()
        {
            List<DriveInfo> allCDRomNames = new List<DriveInfo>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "CDRom")
                {
                    if (d.IsReady)
                    {
                        allCDRomNames.Add(d);
                    }
                }
            }

            return allCDRomNames;
        }

        //判断当前运行程序是否在光盘中运行
        public static bool IsAppInCDRom()
        {
            bool flgResult = false;
            String strDir = System.IO.Directory.GetCurrentDirectory();
            String rootDir = strDir.Substring(0, 1);
            List<DriveInfo> allCDRomNames = AllCDRomNames();
            foreach (DriveInfo cdRom in allCDRomNames)
            {
                if (cdRom.Name.ToString().Equals(rootDir))
                {
                    flgResult = true;
                    break;
                }
            }

            return flgResult;
        }

        //获得绝对路径
        public static String getSystemMainDir()
        {
            String strDir = AppDomain.CurrentDomain.BaseDirectory;
            int indexLastDir = strDir.LastIndexOf(@"\cjpc\");
            return strDir.Substring(0, indexLastDir + 5);
        }

        //MD5加密
        public static String EncryptCode(String message)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(message);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            String tt = BitConverter.ToString(hashedBytes).Replace("-", "");
            // MessageBox.Show(tt.Length.ToString());            
            return tt;
        }

        public static String get32MD5(String s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }

        //md5
        //private static string getMd5Str(string ConvertString)
        //{
        //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //    string md5Str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
        //    return md5Str.Replace("-", "");
        //}

        static public int GetBrothersClassID(int bclassid)
        {
            string sql = "SELECT pclassid FROM fxzclass WHERE classid=" + bclassid.ToString();
            string pids = dbclass.GetOneValue(sql);
            int pidi = -3;
            int maxid = -3;
            try
            {
                pidi = Convert.ToInt32(pids);
                maxid = GetChildZClassID(pidi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return maxid;
        }

        //是否验证
        public static bool IsYzm(string strYzm)
        {
            bool flag = false;

            string inputyzm = strYzm;
            string inma = inputyzm.Replace("-", "");
            string inyama = inma.Substring(16, 8) + inma.Substring(8, 8);
            string key = Toolimp.keygenWithoutDeadline(inyama);
            if (inputyzm.Equals(key))
            {
                flag = true;
            }

            return flag;
        }

        //产生序列号
        public static string keygenWithoutDeadline(string machineCode)
        {
            String str4Keygen = getMd5Str(machineCode).Substring(0, 8) + machineCode.Substring(8, 8) + machineCode.Substring(0, 8) + getMd5Str(machineCode).Substring(8, 8);
            char[] charArr4Keygen = str4Keygen.ToCharArray();

            StringBuilder sb4Keygen = new StringBuilder();
            for (int i = 0; i < charArr4Keygen.Length; i++)
            {

                if (i % 8 == 0 && i != 0)
                {
                    sb4Keygen.Append("-");
                }

                sb4Keygen.Append(charArr4Keygen[i]);
            }

            return sb4Keygen.ToString();
        }
        public static String getMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string md5Str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            return md5Str.Replace("-", "");
        }

        //是否超时
        public static bool IsCs(string strTime)
        {
            bool flag = false;
            string sysj = strTime;
            if (sysj == null || "".Equals(sysj))
            {
                flag = false;
            }
            else
            {
                if (sysj.Length == 30)
                {
                    sysj = sysj.Substring(11, 1) + sysj.Substring(5, 2) + sysj.Substring(8, 1) + sysj.Substring(20, 1) + sysj.Substring(19, 1) + sysj.Substring(15, 2);
                    int day = Toolimp.StringConvInt(Toolimp.GetDay());
                    int jzday = 0;
                    try
                    {
                        jzday = Toolimp.StringConvInt(sysj);
                        if (day < jzday)
                        {
                            flag = true;
                        }
                    }
                    catch
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        static public int StringConvInt(string val)
        {
            return int.Parse(val);
        }
        static public string GetDay()
        {
            return DateTime.Now.ToString("yyyyMMdd");        // 20080904
        }

        static public int GetMaxNum()
        {
            string sql = "SELECT MAX(classid) AS maxid FROM fxclass WHERE ";
            string tjsql = "";
            string maxi = "";
            int maxid = -3;

            tjsql = " (classid > 20000) AND (classid < 29999)";
            maxi = dbclass.GetOneValue(sql + tjsql);
            try
            {
                maxid = Convert.ToInt32(maxi) + 1;
            }
            catch
            {
                maxid = 20001;
            }

            return maxid;
        }

        static public int GetChildClassID(int pclassid)
        {
            string sql = "SELECT MAX(classid) AS maxid FROM fxzclass WHERE ";
            string tjsql = "";
            string maxi = "";
            int maxid = -3;
            string pci = pclassid.ToString();
            if (pci.Length > 5)
            {
                maxid = -1;
            }
            else if (pci.Length < 5 && pci.Length != 1)
            {
                maxid = -2;
            }
            else if (pci.Length == 1 && pci.Equals("0"))
            {
                tjsql = " (classid > 10000) AND (classid < 19999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 10001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("1"))
            {
                tjsql = " (classid > 20000) AND (classid < 29999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 20001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("2"))
            {
                tjsql = " (classid > 30000) AND (classid < 39999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 30001;
                }

            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("3"))
            {
                tjsql = " (classid > 40000) AND (classid < 49999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 40001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("4"))
            {
                tjsql = " (classid > 50000) AND (classid < 59999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 50001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("5"))
            {
                tjsql = " (classid > 60000) AND (classid < 69999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 60001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("6"))
            {
                tjsql = " (classid > 70000) AND (classid < 79999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 70001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("7"))
            {
                tjsql = " (classid > 80000) AND (classid < 89999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 80001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("8"))
            {
                tjsql = " (classid > 90000) AND (classid < 99999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 90001;
                }
            }

            return maxid;
        }


        static public int GetChildZClassID(int pclassid)
        {
            string sql = "SELECT MAX(classid) AS maxid FROM fxzclass WHERE ";
            string tjsql = "";
            string maxi = "";
            int maxid = -3;
            string pci = pclassid.ToString();
            if (pci.Length > 5)
            {
                maxid = -1;
            }
            else if (pci.Length < 5 && pci.Length != 1)
            {
                maxid = -2;
            }
            else if (pci.Length == 1 && pci.Equals("0"))
            {
                tjsql = " (classid > 10000) AND (classid < 19999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 10001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("1"))
            {
                tjsql = " (classid > 20000) AND (classid < 29999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 20001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("2"))
            {
                tjsql = " (classid > 30000) AND (classid < 39999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 30001;
                }

            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("3"))
            {
                tjsql = " (classid > 40000) AND (classid < 49999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 40001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("4"))
            {
                tjsql = " (classid > 50000) AND (classid < 59999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 50001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("5"))
            {
                tjsql = " (classid > 60000) AND (classid < 69999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 60001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("6"))
            {
                tjsql = " (classid > 70000) AND (classid < 79999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 70001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("7"))
            {
                tjsql = " (classid > 80000) AND (classid < 89999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 80001;
                }
            }
            else if (pci.Length == 5 && pci.Substring(0, 1).Equals("8"))
            {
                tjsql = " (classid > 90000) AND (classid < 99999)";
                maxi = dbclass.GetOneValue(sql + tjsql);
                try
                {
                    maxid = Convert.ToInt32(maxi) + 1;
                }
                catch
                {
                    maxid = 90001;
                }
            }

            return maxid;
        }
    }
}
