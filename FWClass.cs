using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cjpc.utils.dbutils
{
    class FWClass<T>
    {
        private T minqz;

        public T Minqz
        {
            get { return minqz; }
            set { minqz = value; }
        }
        private T maxqz;

        public T Maxqz
        {
            get { return maxqz; }
            set { maxqz = value; }
        }

        public FWClass(T _minqz, T _maxqz)
        {
            Minqz = _minqz;
            Maxqz = _maxqz;
        }
    }
}
