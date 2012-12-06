using System;
using System.Threading;

namespace ConsoleApplication1
{
    class Program : IDisposable
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Test();
        }

        #region Object construction/destruction

        /// <summary>
        ///     TODO: Describe this constructor here
        /// </summary>
        public Program()
            : base()
        {
            // TODO: If appropriate, add custom parameters to this constructor and
            // then add here the corresponding extra fields initialization code
        }

        /// <summary>
        ///     This method will be automatically called by the .NET garbage
        ///     collector *sometime*
        /// </summary>
        ~Program()
        {
            // TODO: Add object (undeterministic) destruction code here

        }

        /// <summary>
        ///     Inherited from IDisposable
        /// </summary>
        /// <remarks>
        ///     This method will *not* be called automatically. You will have
        ///     to place an explicit call to it (either directly or through
        ///     IDisposable) in your code when you want to execute the cleanup
        ///     it performs.
        /// </remarks>
        public void Dispose()
        {
            System.GC.SuppressFinalize(this);

            // TODO: Add object disposal code here
        }

        #endregion //(Object construction/destruction)

        /// <summary>
        ///     This call w'll throw some exception, when you pass a fail args.
        /// </summary>
        /// <exception cref="ThreadAbortException">be care for this, will be throwa thread exception</exception>
        public void Test()
        {
            
        }

    }
}
