using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public class ProjectManagerService
    {
        public static int CreateProjectManager(string PM)
        {
            PM = PM.Trim();
            //string[] PMFNLN = PM.Split(',');
            //PM = PMFNLN[0].Trim()+" "+PMFNLN[1];
            if (PM.Contains(','))
            {
                string[] PMFNLN = PM.Split(',');
                PM = PMFNLN[1].Trim()+" "+PMFNLN[0];
                if(PM == "Nazari Azhar")
                {
                    PM = "Azhar Nazari";
                }
            }
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var _iSPMEXIST = db.ProjectManagers.Where(x => x.ProjectManagerName.Contains(PM)).FirstOrDefault();

                //foreach (ProjectManagers pm in list)
                //{
                //    if (pm.ProjectManagerName.Contains(PMFNLN[0]))
                //    {
                //        return pm.Id;
                //    }
                //}

                if (_iSPMEXIST == null)
                {
                    var add = new ProjectManagers()
                    {
                        ProjectManagerName = PM,
                        IsActive = true,
                        CandidateId = GetCandidateId(PM)
                    };
                    var Project = db.ProjectManagers.Add(add);
                    db.SaveChanges();
                    returnID = Project.Id;
                }
                else
                {
                    returnID = _iSPMEXIST.Id;
                }
            }          
        
            return returnID;             
        }

        private static int GetCandidateId(string PM)
        {
            string connString = ConfigurationSettings.AppSettings["costmodel"].Trim();

            string sql = "select id from candidate where lower(CandidateName)  like '%" + PM.ToLower().Trim() + "%'";
            int newProdID = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    newProdID = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (int)newProdID;
        }

    }
}
