using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security;

namespace SharePointOnlineGetSearchUser
{
    class Program
    {
        static void Main(string[] args)
        {
            string SharePointOnlineUrl = ConfigurationManager.AppSettings["SharepointOnlineUrl"].ToString();
            string SharePointOnlineUserLoginName = ConfigurationManager.AppSettings["SharepointOnlineUserEmail"].ToString();
            string SharePointOnlineUserPassword = ConfigurationManager.AppSettings["SharepointOnlineUserPassword"].ToString();


            List<string> SharePointSearchUserList = new List<string>();

            using (ClientContext clientContext = new ClientContext(SharePointOnlineUrl))
            {
                SecureString passWord = new SecureString();

                foreach (char c in SharePointOnlineUserPassword.ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(SharePointOnlineUserLoginName, passWord);

                int searchCount = 0;
                int cs = 500;

                while (cs > 499)
                {
                    cs = 0;

                    KeywordQuery query = new KeywordQuery(clientContext);
                    query.QueryText = "RefinableInt00:1";  // Change your user filter query
                    query.SourceId = new Guid("B09A7990-05EA-4AF9-81EF-EDFAB16C4E31");
                    query.RowLimit = 500;
                    query.StartRow = searchCount;

                    SearchExecutor search = new SearchExecutor(clientContext);


                    ClientResult<ResultTableCollection> results = search.ExecuteQuery(query);
                    clientContext.ExecuteQuery();


                    foreach (var resultRow in results.Value[0].ResultRows)
                    {
                        SharePointSearchUserList.Add(Convert.ToString(resultRow["WorkEmail"]));
                        cs++;
                    }

                    searchCount += cs;
                }
            }
        }
    }
}
