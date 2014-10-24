using System.Collections.Generic;

using KJFramework.ApplicationEngine.Factories;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class FakedInternalResourceFactory : IInternalResourceFactory
    {
        #region Members.

        public readonly Dictionary<string, object> Resources = new Dictionary<string, object>(); 
        
        #endregion
        public void Initialize()
        {
        }

        public object GetResource(string fullname)
        {
            return Resources[fullname];
        }
    }
}