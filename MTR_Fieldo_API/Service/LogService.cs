using Application.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using NLog;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Service
{
    public class LogService: ILogService
    {
        #region <Private Variables>

        private readonly MtrContext  _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private IConfiguration _configuration;
        private string sLogFormat;
        private string sErrorTime;
        #endregion
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region <Constructor>

        public LogService(MtrContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;

            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sHur = DateTime.Now.Hour.ToString();
            string sMin = DateTime.Now.Minute.ToString();
            sErrorTime = sYear + sMonth + sDay;// + sHur + sMin;
        }


        #endregion

        #region <Methods>

        public void ErrorLog(string sPathName, string sErrMsg)
        {
            try
            {
                #region MyRegion
                //StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
                //sw.WriteLine(sLogFormat + sErrMsg);
                //sw.Flush();
                //sw.Close();
                //if (!File.Exists(Path))
                //{
                //    FileStream FS = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
                //    StreamWriter writer = new StreamWriter(FS);
                //    writer.Write(sErrMsg);
                //    writer.Close();
                //}
                #endregion
                if (!Directory.Exists(sPathName))
                {
                    Directory.CreateDirectory(sPathName);
                }
                String Path = sPathName + sErrorTime + ".txt";
                if (File.Exists(Path))
                {
                    using (StreamWriter writer = new StreamWriter(Path, true))
                    {
                        writer.WriteLine(sErrMsg + " " + DateTime.Now);
                    }
                }
                else
                {
                    StreamWriter writer = File.CreateText(Path);
                    writer.WriteLine(sErrMsg + " " + DateTime.Now);
                    writer.Close();
                }
                logger.Error(sErrMsg + DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error(_env.WebRootPath, ex.ToString());
            }



        }
        public void writeLog(string path = "", String strlog = "", int? id = null)
        {
            _context.Fieldo_Log.Add(new Fieldo_Log
            {
               
                LogMessage = strlog,
                CreatedAt = DateTime.Now,
                UserId = id

            });
            _context.SaveChanges();
            //ErrorLog(path + "Logs/ErrorLogTrace", strlog);

        }

        #endregion
    }
}
