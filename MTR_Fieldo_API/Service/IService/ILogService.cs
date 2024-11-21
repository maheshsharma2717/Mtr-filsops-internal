namespace MTR_Fieldo_API.Service.IService
{
    public interface ILogService
    {
        void ErrorLog(string sPathName, string sErrMsg);
        void writeLog(string path = "", String strlog = "", int? id = null);
    }
}
