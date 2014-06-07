using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace Ramses.Utils
{
    /// <summary>
    /// This class contains methods that provides access to session-state strongly typed values
    /// </summary>
    public static class SessionHelper
    {
        #region Properties

        private const string FILESSTATUS = "FILESSTATUS";
        private const string USERPROFILE = "USERPROFILE";

        /// <summary>
        /// Gets the Session object for the current HTTP request
        /// </summary>
        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        #endregion Properties

        /// <summary>
        /// Remove all values from the current session
        /// </summary>
        public static void Clear()
        {
            typeof(SessionHelper).GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(x => x.IsLiteral)
                .ToList()
                .ForEach(f => Session.Remove(f.GetRawConstantValue().ToString()));
        }

        #region Use this as a sample, replace by relevant values)
        // Use constants to define a key for all values, so the Clear methods will work fine
        private const string MYVALUE_KEY = "myValue";

        public static string MyValue
        {
            get { return Session[MYVALUE_KEY] as string; }
            set { Session[MYVALUE_KEY] = value; }
        }

        public static void ClearMyValue()
        {
            Session.Remove(MYVALUE_KEY);
        }
        #endregion 

    }
}
