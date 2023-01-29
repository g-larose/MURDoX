using MURDoX.Services.Interfaces;
using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Services
{
    public class XmlDataService : IXmlDataService
    {
        #region PRIVATE FIELDS

        private string _xmlDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Xml");

        #endregion

        #region CREATE SERVER MEMBER

        public ServerMember CreateServerMember(ServerMember member)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
