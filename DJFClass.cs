using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cjpc.utils.dbutils
{
    class DJFClass
    {
        //等级分取值直范围
        private double maxdjffw;

        public double Maxdjffw
        {
            get { return maxdjffw; }
            set { maxdjffw = value; }
        }

        private double mindjffw;

        public double Mindjffw
        {
            get { return mindjffw; }
            set { mindjffw = value; }
        }

        //真实分等级范围
        private double maxzsfw;

        public double Maxzsfw
        {
            get { return maxzsfw; }
            set { maxzsfw = value; }
        }
        private double minzsfw;

        public double Minzsfw
        {
            get { return minzsfw; }
            set { minzsfw = value; }
        }
        //真实分数
        private double zscj;

        public double Zscj
        {
            get { return zscj; }
            set { zscj = value; }
        }
        //等级分
        private double djcj;

        public double Djcj
        {
            get { return djcj; }
            set { djcj = value; }
        }

        //学生id
        private int stuid;

        public int Stuid
        {
            get { return stuid; }
            set { stuid = value; }
        }

       


        public DJFClass()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public double Jsdjf()
        {
            double djf,tempzjz,tempzjz1,tempzjz2;

            tempzjz1 = maxzsfw - zscj;
            tempzjz2 = zscj - minzsfw;

            if((tempzjz1 == 0 && tempzjz2==0)||(zscj==maxzsfw && zscj == minzsfw))
            {
                djf = maxdjffw;
            }
            else if (tempzjz2 == 0)
            {
                djf = mindjffw;
            }
            else
            {
                tempzjz = tempzjz1/tempzjz2;

                djf = double.Parse(Math.Round((maxdjffw + mindjffw * tempzjz)/(tempzjz+1),2).ToString());

                
            }

            Djcj = djf;
            return Djcj;
        }


    }
}
