using System.IO;
using System;

namespace OrderBot
{
    public class DB
    {
        public static string GetConnectionString(){
            string sFName = "/Orders.db";
            string sPrefix = "Data Source=";
            string sPath = Directory.GetCurrentDirectory();
            string[] subs = sPath.Split(Path.DirectorySeparatorChar);
            for(int n = subs.Length - 1; n > 1; n-- ){
                // skip first empty path
                string sResult = "";
                for(int nsubs = 1; nsubs < n; nsubs++){ 
                    sResult += Path.DirectorySeparatorChar + subs[nsubs];
                }
                string[] aFiles = Directory.GetFiles(sResult, "*.sln", System.IO.SearchOption.TopDirectoryOnly);
                if(aFiles.Length > 0){
                    return sPrefix + sResult + sFName;
                }
            }
            return sPrefix + Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + sFName;
        }
    }
}
