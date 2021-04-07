using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace CostModelDataStream
{   
    public class CostModelLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void InfoLogger(string _info)
        {
            logger.Info(_info);
        }
        public static void ErrorLogger(string _Error)
        {
            logger.Error(_Error);
        }
    }
}
