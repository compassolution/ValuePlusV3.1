namespace Com.ValuePlus.FileAttach.localhost
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.42")]
    public class filedownloadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal filedownloadCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (byte[]) this.results[0];
            }
        }
    }
}

