namespace Com.ValuePlus.FileAttach.localhost
{
    using Com.ValuePlus.FileAttach.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;

    [WebServiceBinding(Name="UploadSoap", Namespace="http://tempuri.org/"), GeneratedCode("System.Web.Services", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code")]
    public class Upload : SoapHttpClientProtocol
    {
        private SendOrPostCallback filedeleteOperationCompleted;
        private SendOrPostCallback filedownloadOperationCompleted;
        private SendOrPostCallback fileuploadOperationCompleted;
        private SendOrPostCallback isExistOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event filedeleteCompletedEventHandler filedeleteCompleted;

        public event filedownloadCompletedEventHandler filedownloadCompleted;

        public event fileuploadCompletedEventHandler fileuploadCompleted;

        public event isExistCompletedEventHandler isExistCompleted;

        public Upload()
        {
            this.Url = Settings.Default.HRSystem_FileAttach_localhost_Upload;
            if (this.IsLocalFileSystemWebService(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public Upload(string myurl)
        {
            this.Url = myurl;
            if (this.IsLocalFileSystemWebService(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        [SoapDocumentMethod("http://tempuri.org/filedelete", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public bool filedelete(string filename, string strpath)
        {
            return (bool) base.Invoke("filedelete", new object[] { filename, strpath })[0];
        }

        public void filedeleteAsync(string filename, string strpath)
        {
            this.filedeleteAsync(filename, strpath, null);
        }

        public void filedeleteAsync(string filename, string strpath, object userState)
        {
            if (this.filedeleteOperationCompleted == null)
            {
                this.filedeleteOperationCompleted = new SendOrPostCallback(this.OnfiledeleteOperationCompleted);
            }
            base.InvokeAsync("filedelete", new object[] { filename, strpath }, this.filedeleteOperationCompleted, userState);
        }

        [return: XmlElement(DataType="base64Binary")]
        [SoapDocumentMethod("http://tempuri.org/filedownload", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public byte[] filedownload(string filename, string strpath)
        {
            return (byte[]) base.Invoke("filedownload", new object[] { filename, strpath })[0];
        }

        public void filedownloadAsync(string filename, string strpath)
        {
            this.filedownloadAsync(filename, strpath, null);
        }

        public void filedownloadAsync(string filename, string strpath, object userState)
        {
            if (this.filedownloadOperationCompleted == null)
            {
                this.filedownloadOperationCompleted = new SendOrPostCallback(this.OnfiledownloadOperationCompleted);
            }
            base.InvokeAsync("filedownload", new object[] { filename, strpath }, this.filedownloadOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/fileupload", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public bool fileupload(string filename, [XmlElement(DataType="base64Binary")] byte[] postArray, string strpath)
        {
            return (bool) base.Invoke("fileupload", new object[] { filename, postArray, strpath })[0];
        }

        public void fileuploadAsync(string filename, byte[] postArray, string strpath)
        {
            this.fileuploadAsync(filename, postArray, strpath, null);
        }

        public void fileuploadAsync(string filename, byte[] postArray, string strpath, object userState)
        {
            if (this.fileuploadOperationCompleted == null)
            {
                this.fileuploadOperationCompleted = new SendOrPostCallback(this.OnfileuploadOperationCompleted);
            }
            base.InvokeAsync("fileupload", new object[] { filename, postArray, strpath }, this.fileuploadOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/isExist", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public bool isExist(string filename, string strpath)
        {
            return (bool) base.Invoke("isExist", new object[] { filename, strpath })[0];
        }

        public void isExistAsync(string filename, string strpath)
        {
            this.isExistAsync(filename, strpath, null);
        }

        public void isExistAsync(string filename, string strpath, object userState)
        {
            if (this.isExistOperationCompleted == null)
            {
                this.isExistOperationCompleted = new SendOrPostCallback(this.OnisExistOperationCompleted);
            }
            base.InvokeAsync("isExist", new object[] { filename, strpath }, this.isExistOperationCompleted, userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(url);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        private void OnfiledeleteOperationCompleted(object arg)
        {
            if (this.filedeleteCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.filedeleteCompleted(this, new filedeleteCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnfiledownloadOperationCompleted(object arg)
        {
            if (this.filedownloadCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.filedownloadCompleted(this, new filedownloadCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnfileuploadOperationCompleted(object arg)
        {
            if (this.fileuploadCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.fileuploadCompleted(this, new fileuploadCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnisExistOperationCompleted(object arg)
        {
            if (this.isExistCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.isExistCompleted(this, new isExistCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        public string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if (!((!this.IsLocalFileSystemWebService(base.Url) || this.useDefaultCredentialsSetExplicitly) || this.IsLocalFileSystemWebService(value)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}

