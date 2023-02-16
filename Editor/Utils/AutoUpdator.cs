#if (UNITY_EDITOR)

using Codice.CM.Client.Differences.Graphic;
using Codice.CM.Common;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using UnityEngine;

namespace Wakatime
{
    public class AutoUpdator
    {
        private Logger Logger { get; }
        private Settings Settings { get; set; }
        public AutoUpdator(Logger logger, Settings settings)
        {
            Logger = logger;
            Settings = settings;
        }



    }
        
}


#endif
