using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.IO;
using System;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using cjpc.utils.dbutils;
using cjpc.utils;
using System.Windows.Forms;

namespace cjpc.utils
{
    class ExcelHelper
    {
        public class x2003
        {
            #region Excel2003
            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xls)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLS(string file, string sheetname, int cks, int end, DataTable dt)
            {
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = hssfworkbook.GetSheet(sheetname);


                    //表头
                    //IRow header = sheet.GetRow(sheet.FirstRowNum);
                    //List<int> columns = new List<int>();
                    //for (int i = 0; i < header.LastCellNum; i++)
                    //{
                    //    object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                    //    if (obj == null || obj.ToString() == string.Empty)
                    //    {
                    //        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    //        //continue;
                    //    }
                    //    else
                    //        dt.Columns.Add(new DataColumn(obj.ToString()));
                    //    columns.Add(i);
                    //}
                    //数据
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr[j] = i + 1 - cks + 1;
                            }
                            else
                            {
                                dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j - 1) as HSSFCell);
                                if (dr[j] == null)
                                {
                                    dr[j] = DBNull.Value;
                                }
                            }

                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }

                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }
             /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xls),将分数题目导入
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForTitleScoreXLS(string file, string sheetname, int cks, int end, int lks, int lend)
            {
                DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook xssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = lks - 1, a = 0; j < lend; j++, a++)
                        {
                            dr[a] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                            if (dr[a] == null)
                            {
                                dr[a] = DBNull.Value;
                            }

                            if (dr[a] != null && dr[a].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }

                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }

            /// <summary>
            /// 将Excel文件中的数据读出并保存
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static void ExcelToTemplateXLS(string file, string sheetname, string[] sqlwhere, string[] names, int count)
            {
                string tempzd = "'',xm,'',dq,xx,nj,bj,xh,'',df";
                string sqlscb = "select " + tempzd + " from sccj where 1=1 ";
                string sqlscbc = "select count(*) from sccj where 1=1 ";
                DBClass dbclass = new DBClass();

                int rowcot = 396;
                int colcot = 131;
                int colsjsx = 70;
                int colkd = 10;
                byte[] buf = null;
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = hssfworkbook.GetSheet(sheetname);
                    ISheet[] sheetdt = new ISheet[count];
                    for (int i = 0; i < count; i++)
                    {
                        sheetdt[i] = hssfworkbook.CloneSheet(0);
                        int sheetindex = hssfworkbook.GetSheetIndex(sheetdt[i]);
                        hssfworkbook.SetSheetName(sheetindex, names[i]);
                        string tempsqlc = sqlscbc + sqlwhere[i];
                        string tempsql = sqlscb + sqlwhere[i];
                        string cotrowstr = dbclass.GetOneValue(tempsqlc);
                        int cotrow = Convert.ToInt32(cotrowstr);
                        int zcolr = 0;
                        if (cotrow < rowcot)
                        {
                            zcolr = rowcot ;
                        }
                        else
                        {
                            zcolr = cotrow ;
                        }
                        //固定值
                        IRow[] rows = new IRow[zcolr];
                        DataTable temptd = new DataTable();
                        temptd = dbclass.GreatDs(tempsql).Tables[0];
                        for (int x = 2; x < zcolr + 2; x++)
                        {
                            rows[x - 2] = sheetdt[i].CreateRow(x);

                            if (x - 2 < cotrow)
                            {
                                for (int y = 0; y < colkd; y++)
                                {
                                    ICell cell = rows[x - 2].CreateCell(y);
                                    cell.SetCellValue(temptd.Rows[x - 2][y].ToString());
                                }
                            }
                        }

                        //属性值
                        object otempcell = null;
                        string tempcellvalue = "";
                        string temppdsql = "";
                        for (int y = colkd; y < colsjsx; y++)
                        {
                            otempcell = GetValueTypeForXLS(sheet.GetRow(0).GetCell(y) as HSSFCell);
                            if (otempcell != null)
                            {
                                tempcellvalue = otempcell.ToString();
                                if (tempcellvalue != "")
                                {
                                    try
                                    {
                                        temppdsql = "select " + tempcellvalue + " from sccj where 1=1 " + sqlwhere[i];
                                        temptd = dbclass.GreatDs(temppdsql).Tables[0];
                                        for (int x = 0; x < cotrow; x++)
                                        {
                                            ICell cell = rows[x].CreateCell(y);
                                            cell.SetCellValue(temptd.Rows[x][0].ToString());
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                   
                                }
                            }
                        }
                    }

                    //sheet.GetRow(1).GetCell(1).SetCellValue("成功");

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    hssfworkbook.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }

            }

            /// <summary>
            /// 将Excel文件中的数据读出并保存聚合函数
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static void ExcelToTemplateXLSWithJH(string file, string sheetname, string sheetbjname, string[] sqlwhere, string[] names, int count)
            {

                string sqlscbc = "";
                int valcz = 0;
                //满分
                string sqljhmf = "";
                double valjhmf = 0;
                //平均分
                string sqljhavg = "";
                double valjhavg = 0;
                //高分组
                
                double valjhhigh = 0;
                //低分组
                
                double valjhlesser = 0;
                //平均分得分率
                double valpjdfl = 0;
                //中位数得分率
                string sqljhzws = "";
                double valjhzws = 0;
                //中平差值
                double valjhzpcz = 0;
                //分化程度
                string sqljhfhcd = "";
                double valjhfhcd = 0;
                //优秀率
                string sqljhyxl = "";
                double valjhyxl = 0;
                //良好达标lv
                string sqljhdbl = "";
                double valjhdbl = 0;
                //中等率
                string sqljhzdl = "";
                double valjhzdl = 0;
                //及格率
                string sqljhjgl = "";
                double valjhjgl = 0;
                //不及格率
                double valjhbjg = 0;

                //第一段
                string sqljhscore1 = "";
                double valjhscore1 = 0;

                //第二段
                string sqljhscore2 = "";
                double valjhscore2 = 0;

                //第三段
                string sqljhscore3 = "";
                double valjhscore3 = 0;

                //第四段
                string sqljhscore4 = "";
                double valjhscore4 = 0;

                //第五段
                string sqljhscore5 = "";
                double valjhscore5 = 0;

                //第六段
                string sqljhscore6 = "";
                double valjhscore6 = 0;

                //第七段
                string sqljhscorez = "";
                double valjhscore7 = 0;

                DBClass dbclass = new DBClass();


                int colsysks = 48;
                int colsysjs = 52;
                int colsxks = 10;
                int colsxjs = 60;
                //表的列数
                int colzd = 270;
                byte[] buf = null;
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = hssfworkbook.GetSheet(sheetname);
                    ISheet sheetbj = hssfworkbook.GetSheet(sheetbjname);
                    Dictionary<string, int> zidwz = new Dictionary<string, int>();
                    for (int z = 0; z < colzd; z++)
                    {
                        object cellvalbj = GetValueTypeForXLS(sheetbj.GetRow(0).GetCell(z) as HSSFCell);
                        if (cellvalbj != null && cellvalbj.ToString() != "" && !cellvalbj.ToString().Equals("平均分") && !cellvalbj.ToString().Equals("中位数得分率") && !cellvalbj.ToString().Equals("分化程度") && !cellvalbj.ToString().Equals("高分组") && !cellvalbj.ToString().Equals("低分组"))
                        {
                            zidwz.Add(cellvalbj.ToString(), z);
                        }
                    }


                    //ISheet[] sheetdt = new ISheet[count];

                    for (int i = 0; i < count; i++)
                    {
                        //sheetdt[i] = hssfworkbook.CloneSheet(0);
                        //int sheetindex = hssfworkbook.GetSheetIndex(sheetdt[i]);
                        //hssfworkbook.SetSheetName(sheetindex, names[i]);
                        IRow sheetbjr = sheetbj.CreateRow(i + 1);
                        ICell bjcell = null;
                        int wz = -1;
                        object cellval = null;

                        #region four
                        for (int x = colsysks; x < colsysjs; x++)
                        {
                            cellval = GetValueTypeForXLS(sheet.GetRow(0).GetCell(x) as HSSFCell);
                            if (cellval != null && cellval.ToString() != "" && cellval.ToString() != "0.0")
                            {
                                //满分
                                sqljhmf = "select round(" + cellval.ToString() + ",2) from jhb where 分项='满分'";
                                valjhmf = double.Parse(double.Parse(dbclass.GetOneValue(sqljhmf)).ToString("#0.00"));
                                // sheetdt[i].GetRow(x).GetCell(colszkd).SetCellValue(valjhmf);

                                //平均分
                                sqljhavg = "select round(AVG(" + cellval.ToString() + "),2) from sccj where 1=1 " + sqlwhere[i];
                                valjhavg = double.Parse(double.Parse(dbclass.GetOneValue(sqljhavg)).ToString("#0.00"));
                                // sheetdt[i].GetRow(x + 1).GetCell(colszkd).SetCellValue(valjhavg);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_平均分"))
                                {
                                    wz = zidwz[cellval.ToString() + "_平均分"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhavg);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }


                                //平均分得分率
                                valpjdfl = double.Parse(((valjhavg / valjhmf) * 100).ToString("#0.00"));
                                // sheetdt[i].GetRow(x + 2).GetCell(colszkd).SetCellValue(valpjdfl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_平均得分率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_平均得分率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valpjdfl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }


                                //中位数得分率、高分组、低分组
                                sqljhzws = "select " + cellval.ToString() + " from sccj where 1=1 " + sqlwhere[i];
                                DataTable dttemp = dbclass.GreatDs(sqljhzws).Tables[0];
                                double[] tempmed = new double[dttemp.Rows.Count];
                                for (int c = 0; c < dttemp.Rows.Count; c++)
                                {
                                    tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                                }
                                //中位数得分率
                                valjhzws = double.Parse(((Toolimp.Median(tempmed) / valjhmf) * 100).ToString("#0.00"));
                                //高分组
                                valjhhigh = double.Parse(((Toolimp.HighScore(tempmed) / valjhmf)*100).ToString("#0.00"));
                                //低分组
                                valjhlesser = double.Parse(((Toolimp.LesserScore(tempmed) / valjhmf)*100).ToString("#0.00"));
                                // sheetdt[i].GetRow(x + 3).GetCell(colszkd).SetCellValue(valjhzws);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_中位数得分率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_中位数得分率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhzws);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                if (zidwz.Keys.Contains(cellval.ToString() + "_高分组"))
                                {
                                    wz = zidwz[cellval.ToString() + "_高分组"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhhigh);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                if (zidwz.Keys.Contains(cellval.ToString() + "_低分组"))
                                {
                                    wz = zidwz[cellval.ToString() + "_低分组"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhlesser);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }



                                //中平差值
                                valjhzpcz = double.Parse((valjhzws - valpjdfl).ToString("#0.00"));
                                //  sheetdt[i].GetRow(x + 4).GetCell(colszkd).SetCellValue(valjhzpcz);

                                //分化程度
                                sqljhfhcd = "select stdev(" + cellval.ToString() + ") from sccj where 1=1 " + sqlwhere[i];
                                valjhfhcd = double.Parse(((double.Parse(dbclass.GetOneValue(sqljhfhcd)) / valjhmf) * 100).ToString("#0.00"));
                                //  sheetdt[i].GetRow(x + 5).GetCell(colszkd).SetCellValue(valjhfhcd);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_分化程度"))
                                {
                                    wz = zidwz[cellval.ToString() + "_分化程度"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhfhcd);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //总数
                                sqlscbc = "select count(*) from sccj where 1=1 " + sqlwhere[i];
                                valcz = Convert.ToInt32(dbclass.GetOneValue(sqlscbc));
                                if (zidwz.Keys.Contains("班级人数"))
                                {
                                    wz = zidwz["班级人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valcz);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //班级
                                if (zidwz.Keys.Contains("班级"))
                                {
                                    wz = zidwz["班级"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(names[i]);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //优秀率
                                sqljhyxl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.9).ToString() + " " + sqlwhere[i];
                                valjhyxl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhyxl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //  sheetdt[i].GetRow(x + 6).GetCell(colszkd).SetCellValue(valjhyxl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_优秀率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_优秀率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhyxl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //良好率
                                sqljhdbl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.8).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.9).ToString() + " " + sqlwhere[i];
                                valjhdbl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhdbl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_良好率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_良好率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhdbl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //中等率
                                sqljhzdl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.7).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.8).ToString() + " " + sqlwhere[i];
                                valjhzdl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhzdl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_中等率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_中等率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhzdl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //及格率
                                sqljhjgl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.6).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.7).ToString() + " " + sqlwhere[i];
                                valjhjgl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhjgl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_及格率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_及格率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhjgl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //不及格率
                                valjhbjg = double.Parse((100 - valjhyxl - valjhdbl - valjhzdl - valjhjgl).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 8).GetCell(colszkd).SetCellValue(valjhbjg);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_不及格率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_不及格率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhbjg);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第一段
                                sqljhscore1 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 10).ToString() + " " + sqlwhere[i];
                                valjhscore1 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore1));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_一人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_一人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore1);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第二段
                                sqljhscore2 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 20).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 10).ToString() + " " + sqlwhere[i];
                                valjhscore2 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore2));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_二人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_二人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore2);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第三段
                                sqljhscore3 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 30).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 20).ToString() + " " + sqlwhere[i];
                                valjhscore3 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore3));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_三人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_三人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore3);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第四段
                                sqljhscore4 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 40).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 30).ToString() + " " + sqlwhere[i];
                                valjhscore4 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore4));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_四人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_四人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore4);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第五段
                                sqljhscore5 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 50).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 40).ToString() + " " + sqlwhere[i];
                                valjhscore5 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore5));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_五人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_五人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore5);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第六段
                                sqljhscore6 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 60).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 50).ToString() + " " + sqlwhere[i];
                                valjhscore6 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore6));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_六人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_六人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore6);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第七段
                                sqljhscorez = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + sqlwhere[i];
                                valjhscore7 = Convert.ToInt32(dbclass.GetOneValue(sqljhscorez)) - valjhscore1 - valjhscore2 - valjhscore3 - valjhscore4 - valjhscore5 - valjhscore6;
                                if (zidwz.Keys.Contains(cellval.ToString() + "_七人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_七人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore7);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }
                               
                            }
                        }
                        #endregion

                        #region thrity
                        for (int x = colsxks; x < colsxjs; x++)
                        {
                            if (x >= 48 && x < 60)
                            {
                                continue;
                            }
                            cellval = GetValueTypeForXLS(sheet.GetRow(0).GetCell(x) as HSSFCell);
                            if (cellval != null && cellval.ToString() != "" && cellval.ToString() != "0.0")
                            {
                                try
                                {
                                    //满分
                                    sqljhmf = "select round(" + cellval.ToString() + ",2) from jhb where 分项='满分'";
                                    valjhmf = double.Parse(dbclass.GetOneValue(sqljhmf));
                                    //       sheetdt[i].GetRow(y).GetCell(colszkd).SetCellValue(valjhmf);

                                    //平均分
                                    sqljhavg = "select round(AVG(" + cellval.ToString() + "),2) from sccj where 1=1 " + sqlwhere[i];
                                    valjhavg = double.Parse(double.Parse(dbclass.GetOneValue(sqljhavg)).ToString("#0.00"));
                                    //       sheetdt[i].GetRow(y + 1).GetCell(colszkd).SetCellValue(valjhavg);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_平均分"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_平均分"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhavg);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //平均分得分率
                                    valpjdfl = double.Parse(((valjhavg / valjhmf) * 100).ToString("#0.00"));
                                    //       sheetdt[i].GetRow(y + 2).GetCell(colszkd).SetCellValue(valpjdfl);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_平均得分率"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_平均得分率"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valpjdfl);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //中位数、高分组、低分组
                                    sqljhzws = "select " + cellval.ToString() + " from sccj where 1=1 " + sqlwhere[i];
                                    DataTable dttemp = dbclass.GreatDs(sqljhzws).Tables[0];
                                    double[] tempmed = new double[dttemp.Rows.Count];
                                    for (int c = 0; c < dttemp.Rows.Count; c++)
                                    {
                                        tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                                    }
                                    valjhzws = double.Parse(((Toolimp.Median(tempmed) / valjhmf) * 100).ToString("#0.00"));
                                    valjhhigh = double.Parse(((Toolimp.HighScore(tempmed) / valjhmf) * 100).ToString("#0.00"));
                                    valjhlesser = double.Parse(((Toolimp.LesserScore(tempmed)/ valjhmf) * 100).ToString("#0.00"));
                                    //       sheetdt[i].GetRow(y + 3).GetCell(colszkd).SetCellValue(valjhzws);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_中位数得分率"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_中位数得分率"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhzws);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    if (zidwz.Keys.Contains(cellval.ToString() + "_高分组"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_高分组"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhhigh);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    if (zidwz.Keys.Contains(cellval.ToString() + "_低分组"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_低分组"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhlesser);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //中平差值
                                    valjhzpcz = double.Parse((valjhzws - valpjdfl).ToString("#0.00"));
                                    //      sheetdt[i].GetRow(y + 4).GetCell(colszkd).SetCellValue(valjhzpcz);

                                    //分化程度
                                    sqljhfhcd = "select stdev(" + cellval.ToString() + ") from sccj where 1=1 " + sqlwhere[i];
                                    valjhfhcd = double.Parse(((double.Parse(dbclass.GetOneValue(sqljhfhcd)) / valjhmf) * 100).ToString("#0.00"));
                                    //    sheetdt[i].GetRow(y + 5).GetCell(colszkd).SetCellValue(valjhfhcd);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_分化程度"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_分化程度"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhfhcd);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }
                                }
                                catch
                                {
                                    continue;
                                }
                               
                            }
                        }
                        #endregion
                    }

                    //sheet.GetRow(1).GetCell(1).SetCellValue("成功");

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    hssfworkbook.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }

            }

            /// <summary>
            /// 导出班级报告
            /// </summary>
            /// <param name="file">file,班级聚合表，file2，班级表</param>
            /// <returns></returns>
            public static void ExcelJHToBJ(string file, string file2)
            {
                int colcount = 126;
                int colnj = 50;
                int rownj = 2;
                int colbjindex = 4;
                int colnumpindex = 5;
                byte[] buf = null;
                using (FileStream fs1 = new FileStream(file, FileMode.Open, FileAccess.Read), fs2 = new FileStream(file2, FileMode.Open, FileAccess.ReadWrite))
                {
                    HSSFWorkbook hssfworkbook1 = new HSSFWorkbook(fs1);
                    ISheet sheetscore1 = hssfworkbook1.GetSheet("score");
                    ISheet sjmb = hssfworkbook1.GetSheet("sjmb");
                    int rowcount = sheetscore1.LastRowNum;
                    for (int j = 1; j < rowcount; j++)
                    {
                        try
                        {
                            XSSFCell xbj = sheetscore1.GetRow(j).GetCell(colbjindex) as XSSFCell;
                            XSSFCell xrs = sheetscore1.GetRow(j).GetCell(colnumpindex) as XSSFCell;
                        }
                        catch
                        {
                            rowcount = j;
                        }
                    }


                    HSSFWorkbook hssfworkbook2 = new HSSFWorkbook(fs2);
                    ISheet sheetscore2 = hssfworkbook2.GetSheet("score");
                    ISheet sheetpjnj = hssfworkbook2.GetSheet("年级平均");
                    ISheet sheetnj = hssfworkbook2.GetSheet("年级");

                    IRow header1 = sheetscore1.GetRow(sheetscore1.FirstRowNum);
                    IRow header2 = sheetscore2.GetRow(sheetscore2.FirstRowNum);
                    IRow njpj = sheetpjnj.CreateRow(2);

                    object cellval = null;

                    for (int i = 0; i < colcount; i++)
                    {
                        cellval = GetValueTypeForXLS(header1.GetCell(i) as HSSFCell);
                        if (cellval != null && cellval.ToString() != "")
                        {
                            header2.GetCell(i).SetCellValue(cellval.ToString());
                        }
                    }

                    for (int j = 1; j < rowcount - 1; j++)
                    {
                        IRow rowsorce2 = sheetscore2.CreateRow(j);
                        for (int i = 0; i < colcount; i++)
                        {
                            cellval = GetValueTypeForXLS(sheetscore1.GetRow(j + 1).GetCell(i) as HSSFCell);
                            if (cellval != null && cellval.ToString() != "")
                            {
                                ICell cell = rowsorce2.CreateCell(i);
                                cell.SetCellValue(cellval.ToString());
                            }
                        }
                    }

                    for (int i = 8; i < colcount; i++)
                    {
                        cellval = GetValueTypeForXLS(sheetscore1.GetRow(1).GetCell(i) as HSSFCell);
                        if (cellval != null && cellval.ToString() != "")
                        {
                            ICell cell = njpj.CreateCell(i - 1);
                            cell.SetCellValue(cellval.ToString());
                        }
                    }

                    for (int j = 0; j < rownj; j++)
                    {
                        for (int i = 0; i < colnj; i++)
                        {
                            cellval = GetValueTypeForXLS(sjmb.GetRow(j).GetCell(i) as HSSFCell);
                            if (cellval != null && cellval.ToString() != "")
                            {
                                sheetnj.GetRow(j).GetCell(i).SetCellValue(cellval.ToString());
                            }
                        }
                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    hssfworkbook2.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs2 = new FileStream(file2, FileMode.Create, FileAccess.Write))
                {
                    fs2.Write(buf, 0, buf.Length);
                    fs2.Flush();
                }
            }

            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xls)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLSWithHeader(string file, string sheetname, int cks, int end, DataTable dt)
            {
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    ISheet sheet = hssfworkbook.GetSheet(sheetname);


                    //表头
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        columns.Add(i);
                    }
                    //数据
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr[j] = i + 1 - cks;
                            }
                            else
                            {
                                dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j - 1) as HSSFCell);
                                if (dr[j] == null)
                                {
                                    dr[j] = DBNull.Value;
                                }
                            }

                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }

                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }



            /// <summary>
            /// 获取Excel文件Sheet表
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static ArrayList ExcelToSheetForXLS(string file)
            {
                ArrayList TablesList = new ArrayList();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                    foreach (ISheet sheet in hssfworkbook)
                    {
                        TablesList.Add(sheet.SheetName);
                    }
                }
                return TablesList;
            }


            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelForXLS(DataTable dt, string file)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelWithRow2ForXLS(DataTable dt, string file, string[] cellvalue)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                IRow rowinsert = sheet.CreateRow(1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    ICell cellinsert = rowinsert.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cellinsert.SetCellValue(cellvalue[i]);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 2);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TablesToExcelForXLS(DataTable[] dts, string[] names, string file)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                for (int x = 0; x < dts.Length; x++)
                {
                    ISheet sheet = hssfworkbook.CreateSheet(names[x]);

                    //表头
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dts[x].Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dts[x].Columns[i].ColumnName);
                    }

                    //数据
                    for (int i = 0; i < dts[x].Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dts[x].Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dts[x].Rows[i][j].ToString());
                        }
                    }
                }


                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TablesToExcelWithRow2ForXLS(DataTable[] dts, string[] names, string file, string[] cellvalue)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                for (int x = 0; x < dts.Length; x++)
                {
                    ISheet sheet = hssfworkbook.CreateSheet(names[x]);

                    //表头
                    IRow row = sheet.CreateRow(0);
                    IRow row2 = sheet.CreateRow(1);
                    for (int i = 0; i < dts[x].Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        ICell cell2 = row2.CreateCell(i);
                        cell.SetCellValue(dts[x].Columns[i].ColumnName);
                        cell2.SetCellValue(cellvalue[i]);
                    }

                    //数据
                    for (int i = 0; i < dts[x].Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 2);
                        for (int j = 0; j < dts[x].Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dts[x].Rows[i][j].ToString());
                        }
                    }
                }


                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void DGVToExcelForXLS(DataGridView dgv, string file)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dgv.Columns[i].HeaderText);
                }

                //数据
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dgv[j, i].Value.ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xls)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void DGVToExcelWithRow2ForXLS(DataGridView dgv, string file, string[] cellvalue)
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                IRow row2 = sheet.CreateRow(1);
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    ICell cell = row.CreateCell(i);
                    ICell cell2 = row2.CreateCell(i);
                    cell.SetCellValue(dgv.Columns[i].HeaderText);
                    cell2.SetCellValue(cellvalue[i]);
                }

                //数据
                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 2);
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dgv[j, i].Value.ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 获取单元格类型(xls)
            /// </summary>
            /// <param name="cell"></param>
            /// <returns></returns>
            private static object GetValueTypeForXLS(HSSFCell cell)
            {
                if (cell == null)
                    return DBNull.Value;
                switch (cell.CellType)
                {
                    case CellType.Blank: //BLANK:
                        return DBNull.Value;
                    case CellType.Boolean: //BOOLEAN:
                        return cell.BooleanCellValue;
                    case CellType.Numeric: //NUMERIC:
                        return cell.NumericCellValue;
                    case CellType.String: //STRING:
                        return cell.StringCellValue;
                    case CellType.Error: //ERROR:
                        return cell.ErrorCellValue;
                    case CellType.Formula: //FORMULA:
                    default:
                        return "=" + cell.CellFormula;
                }
            }
            #endregion
        }

        public class x2007
        {
            #region Excel2007
            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLSX(string file, string sheetname, int cks, int end, DataTable dt)
            {
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);

                    //表头
                    //IRow header = sheet.GetRow(sheet.FirstRowNum);
                    //List<int> columns = new List<int>();
                    //for (int i = 0; i < header.LastCellNum; i++)
                    //{
                    //    object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                    //    if (obj == null || obj.ToString() == string.Empty)
                    //    {
                    //        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    //        //continue;
                    //    }
                    //    else
                    //        dt.Columns.Add(new DataColumn(obj.ToString()));
                    //    columns.Add(i);
                    //}
                    //数据
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr[j] = i + 1 - cks;
                            }
                            else
                            {
                                dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j - 1) as XSSFCell);
                                if (dr[j] == null)
                                {
                                    dr[j] = DBNull.Value;
                                }
                            }

                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }

                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }
             /// <summary>
            /// 将Excel文件中的数据读出到DataTable中,导入试题题目和分数(xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForTitleScoreXLSX(string file, string sheetname, int cks, int end, int lks, int lend)
            {
                DataTable dt = new DataTable();
                for(int b = lks -1; b < lend; b++)
                {
                    dt.Columns.Add((b).ToString(), Type.GetType("System.String"));
                }

                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = lks-1,a = 0; j < lend; j++,a++)
                        {
                            
                            dr[a] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                            if (dr[a] == null)
                            {
                                dr[a] = DBNull.Value;
                            }

                            if (dr[a] != null && dr[a].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }

                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }

            /// <summary>
            /// 将Excel文件中的数据保存Exception；
            /// (xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static void ExcelToTemplateXLSX(string file, string sheetname, string[] sqlwhere, string[] names, int count)
            {
                string tempzd = "'',xm,'',dq,xx,nj,bj,xh,'',df";
                string sqlscb = "select " + tempzd + " from sccj where 1=1 ";
                string sqlscbc = "select count(*) from sccj where 1=1 ";
                DBClass dbclass = new DBClass();

                int rowcot = 396;
                int colcot = 131;
                int colkd = 10;
                int colsjsx = 70;

                byte[] buf = null;
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);

                    ISheet[] sheetdt = new ISheet[count];
                    for (int i = 0; i < count; i++)
                    {
                        int indexsheetname = xssfworkbook.GetSheetIndex(sheet);
                        sheetdt[i] = xssfworkbook.CloneSheet(indexsheetname);
                        int sheetindex = xssfworkbook.GetSheetIndex(sheetdt[i]);
                        xssfworkbook.SetSheetName(sheetindex, names[i]);
                        string tempsqlc = sqlscbc + sqlwhere[i];
                        string tempsql = sqlscb + sqlwhere[i];
                        string cotrowstr = dbclass.GetOneValue(tempsqlc);
                        int cotrow = Convert.ToInt32(cotrowstr);
                        int zcolr = 0;
                        if (cotrow < rowcot)
                        {
                            zcolr = rowcot ;
                        }
                        else
                        {
                            zcolr = cotrow ;
                        }
                        //固定值
                        IRow[] rows = new IRow[zcolr];
                        DataTable temptd = new DataTable();
                        temptd = dbclass.GreatDs(tempsql).Tables[0];
                        for (int x = 2; x < zcolr + 2; x++)
                        {
                            rows[x - 2] = sheetdt[i].CreateRow(x);

                            if (x - 2 < cotrow)
                            {
                                for (int y = 0; y < colkd; y++)
                                {
                                    ICell cell = rows[x - 2].CreateCell(y);
                                    cell.SetCellValue(temptd.Rows[x - 2][y].ToString());
                                }
                            }
                        }
                        //属性值
                        //属性值
                        object otempcell = null;
                        string tempcellvalue = "";
                        string temppdsql = "";
                        for (int y = colkd; y < colsjsx; y++)
                        {
                            otempcell = GetValueTypeForXLSX(sheet.GetRow(0).GetCell(y) as XSSFCell);
                            if (otempcell != null)
                            {
                                tempcellvalue = otempcell.ToString();
                                if (tempcellvalue != "")
                                {
                                    try
                                    {
                                        temppdsql = "select " + tempcellvalue + " from sccj where 1=1 " + sqlwhere[i];
                                        temptd = dbclass.GreatDs(temppdsql).Tables[0];
                                        for (int x = 0; x < cotrow; x++)
                                        {
                                            ICell cell = rows[x].CreateCell(y);
                                            cell.SetCellValue(temptd.Rows[x][0].ToString());
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }  
                                }
                            }
                        }

                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    xssfworkbook.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将Excel文件中的数据保存Exception；
            /// (xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static void ExcelToTemplateXLSXWithJH(string file, string sheetname, string sheetbjname, string[] sqlwhere, string[] names, int count)
            {
                string sqlscbc = "";
                int valcz = 0;
                //满分
                string sqljhmf = "";
                double valjhmf = 0;
                //平均分
                string sqljhavg = "";
                double valjhavg = 0;
                //高分组
                
                double valjhhigh = 0;
                //低分组
               
                double valjhlesser = 0;
                //平均分得分率
                double valpjdfl = 0;
                //中位数得分率
                string sqljhzws = "";
                double valjhzws = 0;
                //中平差值
                double valjhzpcz = 0;
                //分化程度
                string sqljhfhcd = "";
                double valjhfhcd = 0;
                //优秀率
                string sqljhyxl = "";
                double valjhyxl = 0;
                //良好达标lv
                string sqljhdbl = "";
                double valjhdbl = 0;
                //中等率
                string sqljhzdl = "";
                double valjhzdl = 0;
                //及格率
                string sqljhjgl = "";
                double valjhjgl = 0;
                //不及格率
                double valjhbjg = 0;

                //第一段
                string sqljhscore1 = "";
                double valjhscore1 = 0;

                //第二段
                string sqljhscore2 = "";
                double valjhscore2 = 0;

                //第三段
                string sqljhscore3 = "";
                double valjhscore3 = 0;

                //第四段
                string sqljhscore4 = "";
                double valjhscore4 = 0;

                //第五段
                string sqljhscore5 = "";
                double valjhscore5 = 0;

                //第六段
                string sqljhscore6 = "";
                double valjhscore6 = 0;

                //第七段
                string sqljhscorez = "";
                double valjhscore7 = 0;
                DBClass dbclass = new DBClass();

             
                int colsysks = 48;
                int colsysjs = 52;
                int colsxks = 10;
                int colsxjs = 60;
                //表的列数
                int colzd = 270;

                byte[] buf = null;
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);
                    ISheet sheetbj = xssfworkbook.GetSheet(sheetbjname);
                    Dictionary<string, int> zidwz = new Dictionary<string, int>();
                    for (int z = 0; z < colzd; z++)
                    {
                        object cellvalbj = GetValueTypeForXLSX(sheetbj.GetRow(0).GetCell(z) as XSSFCell);
                        if (cellvalbj != null && cellvalbj.ToString() != "" && !cellvalbj.ToString().Equals("平均分") && !cellvalbj.ToString().Equals("中位数得分率") && !cellvalbj.ToString().Equals("分化程度") && !cellvalbj.ToString().Equals("高分组") && !cellvalbj.ToString().Equals("低分组"))
                        {
                            zidwz.Add(cellvalbj.ToString(), z);
                        }
                    }

                    //ISheet[] sheetdt = new ISheet[count];
                    for (int i = 0; i < count; i++)
                    {
                      //  sheetdt[i] = xssfworkbook.CloneSheet(xssfworkbook.GetSheetIndex(sheet));
                        //int sheetindex = xssfworkbook.GetSheetIndex(sheetdt[i]);
                        
                       
                        //sheetdt[i].SheetName.Replace(sheetdt[i].SheetName, names[i]);
                        IRow sheetbjr = sheetbj.CreateRow(i + 1);
                        ICell bjcell = null;
                        int wz = -1;
                        object cellval = null;

                        #region four
                        for (int x = colsysks; x < colsysjs; x++)
                        {
                            cellval = GetValueTypeForXLSX(sheet.GetRow(0).GetCell(x) as XSSFCell);
                            if (cellval != null && cellval.ToString() != "" && cellval.ToString() != "0.0")
                            {
                                //满分
                                sqljhmf = "select round(" + cellval.ToString() + ",2) from jhb where 分项='满分'";
                                valjhmf = double.Parse(double.Parse(dbclass.GetOneValue(sqljhmf)).ToString("#0.00"));
                               // sheetdt[i].GetRow(x).GetCell(colszkd).SetCellValue(valjhmf);

                                //平均分
                                sqljhavg = "select round(AVG(" + cellval.ToString() + "),2) from sccj where 1=1 " + sqlwhere[i];
                                valjhavg = double.Parse(double.Parse(dbclass.GetOneValue(sqljhavg)).ToString("#0.00"));
                               // sheetdt[i].GetRow(x + 1).GetCell(colszkd).SetCellValue(valjhavg);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_平均分"))
                                {
                                    wz = zidwz[cellval.ToString() + "_平均分"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhavg);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }


                                //平均分得分率
                                valpjdfl = double.Parse(((valjhavg / valjhmf) * 100).ToString("#0.00"));
                                // sheetdt[i].GetRow(x + 2).GetCell(colszkd).SetCellValue(valpjdfl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_平均得分率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_平均得分率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valpjdfl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }


                                //中位数得分率、高分组、低分组
                                sqljhzws = "select " + cellval.ToString() + " from sccj where 1=1 " + sqlwhere[i];
                                DataTable dttemp = dbclass.GreatDs(sqljhzws).Tables[0];
                                double[] tempmed = new double[dttemp.Rows.Count];
                                for (int c = 0; c < dttemp.Rows.Count; c++)
                                {
                                    tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                                }
                                //中位数得分率
                                valjhzws = double.Parse(((Toolimp.Median(tempmed) / valjhmf) * 100).ToString("#0.00"));
                                //高分组
                                valjhhigh = double.Parse(((Toolimp.HighScore(tempmed) / valjhmf)*100).ToString("#0.00"));
                                //低分组
                                valjhlesser = double.Parse(((Toolimp.LesserScore(tempmed) / valjhmf) * 100).ToString("#0.00"));
                               // sheetdt[i].GetRow(x + 3).GetCell(colszkd).SetCellValue(valjhzws);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_中位数得分率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_中位数得分率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhzws);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                if (zidwz.Keys.Contains(cellval.ToString() + "_高分组"))
                                {
                                    wz = zidwz[cellval.ToString() + "_高分组"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhhigh);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                if (zidwz.Keys.Contains(cellval.ToString() + "_低分组"))
                                {
                                    wz = zidwz[cellval.ToString() + "_低分组"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhlesser);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }



                                //中平差值
                                valjhzpcz = double.Parse((valjhzws - valpjdfl).ToString("#0.00"));
                              //  sheetdt[i].GetRow(x + 4).GetCell(colszkd).SetCellValue(valjhzpcz);

                                //分化程度
                                sqljhfhcd = "select stdev(" + cellval.ToString() + ") from sccj where 1=1 " + sqlwhere[i];
                                valjhfhcd = double.Parse(((double.Parse(dbclass.GetOneValue(sqljhfhcd)) / valjhmf) * 100).ToString("#0.00"));
                              //  sheetdt[i].GetRow(x + 5).GetCell(colszkd).SetCellValue(valjhfhcd);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_分化程度"))
                                {
                                    wz = zidwz[cellval.ToString() + "_分化程度"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhfhcd);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //总数
                                sqlscbc = "select count(*) from sccj where 1=1 " + sqlwhere[i];
                                valcz = Convert.ToInt32(dbclass.GetOneValue(sqlscbc));
                                if (zidwz.Keys.Contains("班级人数"))
                                {
                                    wz = zidwz["班级人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valcz);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //班级
                                if (zidwz.Keys.Contains("班级"))
                                {
                                    wz = zidwz["班级"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(names[i]);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //优秀率
                                sqljhyxl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.9).ToString() + " " + sqlwhere[i];
                                valjhyxl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhyxl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                              //  sheetdt[i].GetRow(x + 6).GetCell(colszkd).SetCellValue(valjhyxl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_优秀率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_优秀率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhyxl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //良好率
                                sqljhdbl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.8).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.9).ToString() + " " + sqlwhere[i];
                                valjhdbl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhdbl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                            //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_良好率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_良好率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhdbl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //中等率
                                sqljhzdl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.7).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.8).ToString() + " " + sqlwhere[i];
                                valjhzdl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhzdl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_中等率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_中等率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhzdl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //及格率
                                sqljhjgl = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf * 0.6).ToString() + " and " + cellval.ToString() + " < " + (valjhmf * 0.7).ToString() + " " + sqlwhere[i];
                                valjhjgl = double.Parse(((Convert.ToInt32(dbclass.GetOneValue(sqljhjgl)) * 1.0 / valcz) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(x + 7).GetCell(colszkd).SetCellValue(valjhdbl);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_及格率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_及格率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhjgl);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //不及格率
                                valjhbjg = double.Parse((100 - valjhyxl - valjhdbl - valjhzdl - valjhjgl).ToString("#0.00"));
                            //    sheetdt[i].GetRow(x + 8).GetCell(colszkd).SetCellValue(valjhbjg);
                                if (zidwz.Keys.Contains(cellval.ToString() + "_不及格率"))
                                {
                                    wz = zidwz[cellval.ToString() + "_不及格率"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhbjg);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第一段
                                sqljhscore1 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 10).ToString() + " " + sqlwhere[i];
                                valjhscore1 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore1));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_一人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_一人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore1);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第二段
                                sqljhscore2 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 20).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 10).ToString() + " " + sqlwhere[i];
                                valjhscore2 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore2));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_二人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_二人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore2);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第三段
                                sqljhscore3 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 30).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 20).ToString() + " " + sqlwhere[i];
                                valjhscore3 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore3));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_三人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_三人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore3);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第四段
                                sqljhscore4 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 40).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 30).ToString() + " " + sqlwhere[i];
                                valjhscore4 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore4));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_四人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_四人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore4);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第五段
                                sqljhscore5 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 50).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 40).ToString() + " " + sqlwhere[i];
                                valjhscore5 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore5));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_五人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_五人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore5);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第六段
                                sqljhscore6 = "select count(" + cellval.ToString() + ") from sccj where 1=1 and " + cellval.ToString() + " >= " + (valjhmf - 60).ToString() + " and " + cellval.ToString() + " < " + (valjhmf - 50).ToString() + " " + sqlwhere[i];
                                valjhscore6 = Convert.ToInt32(dbclass.GetOneValue(sqljhscore6));
                                if (zidwz.Keys.Contains(cellval.ToString() + "_六人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_六人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore6);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }

                                //第七段
                                sqljhscorez = "select count(" + cellval.ToString() + ") from sccj where 1=1 " + sqlwhere[i];
                                valjhscore7 = Convert.ToInt32(dbclass.GetOneValue(sqljhscorez)) - valjhscore1 - valjhscore2 - valjhscore3 - valjhscore4 - valjhscore5 - valjhscore6;
                                if (zidwz.Keys.Contains(cellval.ToString() + "_七人数"))
                                {
                                    wz = zidwz[cellval.ToString() + "_七人数"];
                                    bjcell = sheetbjr.CreateCell(wz);
                                    bjcell.SetCellValue(valjhscore7);
                                }
                                else
                                {
                                    bjcell = null;
                                    wz = -1;
                                }
                            }
                        }
                        #endregion

                        #region thrity
                        for (int x = colsxks; x < colsxjs; x++)
                        {
                            if(x>=48 && x < 60 )
                            {
                                continue;
                            }
                            
                            cellval = GetValueTypeForXLSX(sheet.GetRow(0).GetCell(x) as XSSFCell);
                            if (cellval != null && cellval.ToString() != "" && cellval.ToString() != "0.0")
                            {
                                try
                                {
                                    //满分
                                    sqljhmf = "select round(" + cellval.ToString() + ",2) from jhb where 分项='满分'";
                                    valjhmf = double.Parse(dbclass.GetOneValue(sqljhmf));
                             //       sheetdt[i].GetRow(y).GetCell(colszkd).SetCellValue(valjhmf);

                                    //平均分
                                    sqljhavg = "select round(AVG(" + cellval.ToString() + "),2) from sccj where 1=1 " + sqlwhere[i];
                                    valjhavg = double.Parse(double.Parse(dbclass.GetOneValue(sqljhavg)).ToString("#0.00"));
                             //       sheetdt[i].GetRow(y + 1).GetCell(colszkd).SetCellValue(valjhavg);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_平均分"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_平均分"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhavg);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //平均分得分率
                                    valpjdfl = double.Parse(((valjhavg / valjhmf) * 100).ToString("#0.00"));
                             //       sheetdt[i].GetRow(y + 2).GetCell(colszkd).SetCellValue(valpjdfl);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_平均得分率"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_平均得分率"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valpjdfl);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //中位数、高分组、低分组
                                    sqljhzws = "select " + cellval.ToString() + " from sccj where 1=1 " + sqlwhere[i];
                                    DataTable dttemp = dbclass.GreatDs(sqljhzws).Tables[0];
                                    double[] tempmed = new double[dttemp.Rows.Count];
                                    for (int c = 0; c < dttemp.Rows.Count; c++)
                                    {
                                        tempmed[c] = double.Parse(dttemp.Rows[c][0].ToString());
                                    }
                                    valjhzws = double.Parse(((Toolimp.Median(tempmed) / valjhmf) * 100).ToString("#0.00"));
                                    valjhhigh = double.Parse(((Toolimp.HighScore(tempmed)/ valjhmf) * 100).ToString("#0.00"));
                                    valjhlesser = double.Parse(((Toolimp.LesserScore(tempmed) / valjhmf) * 100).ToString("#0.00"));
                             //       sheetdt[i].GetRow(y + 3).GetCell(colszkd).SetCellValue(valjhzws);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_中位数得分率"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_中位数得分率"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhzws);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    if (zidwz.Keys.Contains(cellval.ToString() + "_高分组"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_高分组"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhhigh);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    if (zidwz.Keys.Contains(cellval.ToString() + "_低分组"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_低分组"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhlesser);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                    //中平差值
                                    valjhzpcz = double.Parse((valjhzws - valpjdfl).ToString("#0.00"));
                              //      sheetdt[i].GetRow(y + 4).GetCell(colszkd).SetCellValue(valjhzpcz);

                                    //分化程度
                                    sqljhfhcd = "select stdev(" + cellval.ToString() + ") from sccj where 1=1 " + sqlwhere[i];
                                    valjhfhcd = double.Parse(((double.Parse(dbclass.GetOneValue(sqljhfhcd)) / valjhmf) * 100).ToString("#0.00"));
                                //    sheetdt[i].GetRow(y + 5).GetCell(colszkd).SetCellValue(valjhfhcd);
                                    if (zidwz.Keys.Contains(cellval.ToString() + "_分化程度"))
                                    {
                                        wz = zidwz[cellval.ToString() + "_分化程度"];
                                        bjcell = sheetbjr.CreateCell(wz);
                                        bjcell.SetCellValue(valjhfhcd);
                                    }
                                    else
                                    {
                                        bjcell = null;
                                        wz = -1;
                                    }

                                }
                                catch
                                {
                                    continue;
                                }            
                            }
                        }
                        #endregion

                     //   xssfworkbook.SetSheetName(sheetindex, names[i]);
                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    xssfworkbook.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 导出班级报告
            /// </summary>
            /// <param name="file">file,班级聚合表，file2，班级表</param>
            /// <returns></returns>
            public static void ExcelJHToBJ(string file, string file2)
            {
                int colcount = 126;
                int colnj = 50;
                int rownj = 2;
                int colbjindex = 4;
                int colnumpindex = 5;
                byte[] buf = null;
                using (FileStream fs1 = new FileStream(file, FileMode.Open, FileAccess.Read), fs2 = new FileStream(file2, FileMode.Open, FileAccess.ReadWrite))
                {
                    XSSFWorkbook hssfworkbook1 = new XSSFWorkbook(fs1);
                    ISheet sheetscore1 = hssfworkbook1.GetSheet("score");
                    ISheet sjmb = hssfworkbook1.GetSheet("sjmb");
                    int rowcount = sheetscore1.LastRowNum;
                    for (int j = 1; j < rowcount; j++)
                    {
                        try
                        {
                            XSSFCell xbj = sheetscore1.GetRow(j).GetCell(colbjindex) as XSSFCell;
                            XSSFCell xrs = sheetscore1.GetRow(j).GetCell(colnumpindex) as XSSFCell;
                        }
                        catch
                        {
                            rowcount = j;
                        }
                    }


                    XSSFWorkbook hssfworkbook2 = new XSSFWorkbook(fs2);
                    ISheet sheetscore2 = hssfworkbook2.GetSheet("score");
                    ISheet sheetpjnj = hssfworkbook2.GetSheet("年级平均");
                    ISheet sheetnj = hssfworkbook2.GetSheet("年级");

                    IRow header1 = sheetscore1.GetRow(sheetscore1.FirstRowNum);
                    IRow header2 = sheetscore2.GetRow(sheetscore2.FirstRowNum);
                    IRow njpj = sheetpjnj.CreateRow(2);

                    object cellval = null;

                    for (int i = 0; i < colcount; i++)
                    {
                        cellval = GetValueTypeForXLSX(header1.GetCell(i) as XSSFCell);
                        if (cellval != null && cellval.ToString() != "")
                        {
                            header2.GetCell(i).SetCellValue(cellval.ToString());
                        }
                    }

                    for (int j = 1; j < rowcount - 1; j++)
                    {
                        IRow rowsorce2 = sheetscore2.CreateRow(j);
                        for (int i = 0; i < colcount; i++)
                        {
                            cellval = GetValueTypeForXLSX(sheetscore1.GetRow(j + 1).GetCell(i) as XSSFCell);
                            if (cellval != null && cellval.ToString() != "")
                            {
                                ICell cell = rowsorce2.CreateCell(i);
                                cell.SetCellValue(cellval.ToString());
                            }
                        }
                    }

                    for (int i = 8; i < colcount; i++)
                    {
                        cellval = GetValueTypeForXLSX(sheetscore1.GetRow(1).GetCell(i) as XSSFCell);
                        if (cellval != null && cellval.ToString() != "")
                        {
                            ICell cell = njpj.CreateCell(i - 1);
                            cell.SetCellValue(cellval.ToString());
                        }
                    }

                    for (int j = 0; j < rownj; j++)
                    {
                        for (int i = 0; i < colnj; i++)
                        {
                            cellval = GetValueTypeForXLSX(sjmb.GetRow(j).GetCell(i) as XSSFCell);
                            if (cellval != null && cellval.ToString() != "")
                            {
                                sheetnj.GetRow(j).GetCell(i).SetCellValue(cellval.ToString());
                            }
                        }
                    }

                    //转为字节数组
                    MemoryStream stream = new MemoryStream();
                    hssfworkbook2.Write(stream);
                    buf = stream.ToArray();
                }

                //保存为Excel文件
                using (FileStream fs2 = new FileStream(file2, FileMode.Create, FileAccess.Write))
                {
                    fs2.Write(buf, 0, buf.Length);
                    fs2.Flush();
                }
            }

            /// <summary>
            /// 将Excel文件中的数据读出到DataTable中(xlsx)
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static DataTable ExcelToTableForXLSXWithHeader(string file, string sheetname, int cks, int end, DataTable dt)
            {
                //DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    ISheet sheet = xssfworkbook.GetSheet(sheetname);

                    //表头
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        columns.Add(i);
                    }
                    //数据
                    for (int i = cks - 1; i <= end - 1; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr[j] = i + 1 - cks;
                            }
                            else
                            {
                                dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                            }

                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }

            /// <summary>
            /// 获取Excel文件Sheet表
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static ArrayList ExcelToSheetForXLSX(string file)
            {
                ArrayList TablesList = new ArrayList();
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                    foreach (ISheet sheet in xssfworkbook)
                    {
                        TablesList.Add(sheet.SheetName);
                    }
                }
                return TablesList;
            }
            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelForXLSX(DataTable dt, string file)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TableToExcelWithRow2ForXLSX(DataTable dt, string file, string[] cellvalue)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                IRow row2 = sheet.CreateRow(1);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    ICell cell2 = row2.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell2.SetCellValue(cellvalue[i]);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 2);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TablesToExcelForXLSX(DataTable[] dts, string[] names, string file)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();

                for (int x = 0; x < dts.Length; x++)
                {
                    ISheet sheet = xssfworkbook.CreateSheet(names[x]);

                    //表头
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dts[x].Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        cell.SetCellValue(dts[x].Columns[i].ColumnName);
                    }

                    //数据
                    for (int i = 0; i < dts[x].Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dts[x].Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dts[x].Rows[i][j].ToString());
                        }
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void TablesToExcelWithRow2ForXLSX(DataTable[] dts, string[] names, string file, string[] cellvalue)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();

                for (int x = 0; x < dts.Length; x++)
                {
                    ISheet sheet = xssfworkbook.CreateSheet(names[x]);

                    //表头
                    IRow row = sheet.CreateRow(0);
                    IRow row2 = sheet.CreateRow(1);
                    for (int i = 0; i < dts[x].Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        ICell cell2 = row2.CreateCell(i);
                        cell.SetCellValue(dts[x].Columns[i].ColumnName);
                        cell2.SetCellValue(cellvalue[i]);
                    }

                    //数据
                    for (int i = 0; i < dts[x].Rows.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i + 2);
                        for (int j = 0; j < dts[x].Columns.Count; j++)
                        {
                            ICell cell = row1.CreateCell(j);
                            cell.SetCellValue(dts[x].Rows[i][j].ToString());
                        }
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void DGVToExcelForXLSX(DataGridView dgv, string file)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dgv.Columns[i].HeaderText);
                }

                //数据
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dgv[j, i].Value.ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 将DataTable数据导出到Excel文件中(xlsx)
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="file"></param>
            public static void DGVToExcelWitrhRow2XLSX(DataGridView dgv, string file, string[] cellvalue)
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Test");

                //表头
                IRow row = sheet.CreateRow(0);
                IRow row2 = sheet.CreateRow(1);
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    ICell cell = row.CreateCell(i);
                    ICell cell2 = row2.CreateCell(i);
                    cell.SetCellValue(dgv.Columns[i].HeaderText);
                    cell2.SetCellValue(cellvalue[i]);
                }

                //数据
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 2);
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dgv[j, i].Value.ToString());
                    }
                }

                //转为字节数组
                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }

            /// <summary>
            /// 获取单元格类型(xlsx)
            /// </summary>
            /// <param name="cell"></param>
            /// <returns></returns>
            private static object GetValueTypeForXLSX(XSSFCell cell)
            {
                if (cell == null)
                    return DBNull.Value;
                switch (cell.CellType)
                {
                    case CellType.Blank: //BLANK:
                        return null;
                    case CellType.Boolean: //BOOLEAN:
                        return cell.BooleanCellValue;
                    case CellType.Numeric: //NUMERIC:
                        return cell.NumericCellValue;
                    case CellType.String: //STRING:
                        return cell.StringCellValue;
                    case CellType.Error: //ERROR:
                        return cell.ErrorCellValue;
                    case CellType.Formula: //FORMULA:
                    default:
                        return "=" + cell.CellFormula;
                }
            }
            #endregion
        }

        public static DataTable GetDataTable(string filepath, string sheetname, int cks, int end, DataTable dtx)
        {
            var dt = new DataTable("xls");
            if (filepath.Last() == 's')
            {
                dt = x2003.ExcelToTableForXLS(filepath, sheetname, cks, end, dtx);
            }
            else
            {
                dt = x2007.ExcelToTableForXLSX(filepath, sheetname, cks, end, dtx);
            }
            return dt;
        }

        public static DataTable GetTitleScoreDataTable(string filepath, string sheetname, int cks, int end, int lks, int lend)
        {
            var dt = new DataTable("xls");
            if (filepath.Last() == 's')
            {
                dt = x2003.ExcelToTableForTitleScoreXLS(filepath, sheetname, cks, end, lks, lend);
            }
            else
            {
                dt = x2007.ExcelToTableForTitleScoreXLSX(filepath, sheetname, cks, end, lks, lend);
            }
            return dt;
        }


        public static DataTable GetDataTableWithHeader(string filepath, string sheetname, int cks, int end, DataTable dtx)
        {
            var dt = new DataTable("xls");
            if (filepath.Last() == 's')
            {
                dt = x2003.ExcelToTableForXLSWithHeader(filepath, sheetname, cks, end, dtx);
            }
            else
            {
                dt = x2007.ExcelToTableForXLSXWithHeader(filepath, sheetname, cks, end, dtx);
            }
            return dt;
        }

        public static void ExcelSaveExcel(string filepath, string sheetname, string[] sqlwhere, string[] names, int count)
        {
            if (filepath.Last() == 's')
            {
                x2003.ExcelToTemplateXLS(filepath, sheetname, sqlwhere, names, count);
            }
            else
            {
                x2007.ExcelToTemplateXLSX(filepath, sheetname, sqlwhere, names, count);
            }
        }

        public static void ExcelSaveExcelWithJH(string filepath, string sheetname, string sheetbjname, string[] sqlwhere, string[] names, int count)
        {
            if (filepath.Last() == 's')
            {
                x2003.ExcelToTemplateXLSWithJH(filepath, sheetname, sheetbjname, sqlwhere, names, count);
            }
            else
            {
                x2007.ExcelToTemplateXLSXWithJH(filepath, sheetname, sheetbjname, sqlwhere, names, count);
            }
        }

        public static void ExcelSaveBj(string filepath, string filepath2)
        {
            if (filepath.Last() == 's' && filepath2.Last() == 's')
            {
                x2003.ExcelJHToBJ(filepath, filepath2);
            }
            else
            {
                x2007.ExcelJHToBJ(filepath, filepath2);
            }
        }

        public static ArrayList GetExcelSheet(string filepath)
        {
            ArrayList TablesList = new ArrayList();
            if (filepath.Last() == 's')
            {
                TablesList = x2003.ExcelToSheetForXLS(filepath);
            }
            else
            {
                TablesList = x2007.ExcelToSheetForXLSX(filepath);
            }
            return TablesList;
        }

        public static void SaveExcel(DataGridView dgv, string filepath)
        {
            if (filepath.Last() == 's')
            {
                x2003.DGVToExcelForXLS(dgv, filepath);
            }
            else
            {
                x2007.DGVToExcelForXLSX(dgv, filepath);
            }
        }

        public static void SaveWithRow2Excel(DataGridView dgv, string filepath, string[] cellvalue)
        {
            if (filepath.Last() == 's')
            {
                x2003.DGVToExcelWithRow2ForXLS(dgv, filepath, cellvalue);
            }
            else
            {
                x2007.DGVToExcelWitrhRow2XLSX(dgv, filepath, cellvalue);
            }
        }

        public static void SaveDtExcel(DataTable[] dts, string[] names, string filepath)
        {
            if (filepath.Last() == 's')
            {
                x2003.TablesToExcelForXLS(dts, names, filepath);
            }
            else
            {
                x2007.TablesToExcelForXLSX(dts, names, filepath);
            }
        }

        public static void SaveDtWithRow2Excel(DataTable[] dts, string[] names, string filepath, string[] cellvalue)
        {
            if (filepath.Last() == 's')
            {
                x2003.TablesToExcelWithRow2ForXLS(dts, names, filepath, cellvalue);
            }
            else
            {
                x2007.TablesToExcelWithRow2ForXLSX(dts, names, filepath, cellvalue);
            }
        }
    }
}
