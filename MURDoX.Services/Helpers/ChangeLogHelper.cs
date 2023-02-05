using MURDoX.Services.Models;
using Remora.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MURDoX.Services.Helpers
{
    public class ChangeLogHelper 
    {
        #region PRIVATE FIELDS

        #endregion

        #region CONSTANTS

        private static string changeLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml", "changelog.xml");

        #endregion

        #region SAVE CHANGELOG TO FILE
        public async Task<int> SaveChangelogToFileAsync(ChangeLog changeLog)// we return a 0 for success
        {
            var ct = new CancellationTokenSource();
            if (!File.Exists(changeLogPath))
            {
                var doc = new XDocument(
                            new XDeclaration("1.0", "utf-8", "yes"),
                                new XElement("logs",
                                new XElement("log",
                                    new XAttribute("id", changeLog.Id),
                                    new XAttribute("name", changeLog.Name),
                                    new XAttribute("content", changeLog.Content),
                                    new XAttribute("status", changeLog.Status),
                                    new XAttribute("timestamp", changeLog.Created_Timestamp))));
                using (FileStream fs = new FileStream(changeLogPath, FileMode.Create, FileAccess.ReadWrite))
                    await doc.SaveAsync(fs, SaveOptions.None, ct.Token);
                return 0;
            }
            else 
            {
                var doc = XDocument.Load(changeLogPath);
                var logElementNode = doc.Descendants("log")
                    .Where(x => x.Name.Equals(changeLog.Name))
                    .ToList();

                if (logElementNode.Count > 0) return 1;

                var log_element = new XElement("log",
                                    new XAttribute("id", changeLog.Id),
                                    new XAttribute("name", changeLog.Name),
                                    new XAttribute("content", changeLog.Content),
                                    new XAttribute("status", changeLog.Status),
                                    new XAttribute("timestamp", changeLog.Created_Timestamp));
                doc.Root!.Add(log_element);
                using (FileStream fs = new FileStream(changeLogPath, FileMode.Create, FileAccess.ReadWrite))
                    await doc.SaveAsync(fs, SaveOptions.None, ct.Token);
                return 0;
            }
        }
        #endregion
    }
}
