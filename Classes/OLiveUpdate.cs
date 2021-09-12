/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.IO;
using System.Net;
using System.Threading;

using K2host.Services.Delegates;
using K2host.Threading.Classes;
using K2host.Threading.Extentions;
using K2host.Threading.Interface;

namespace K2host.Services.Classes
{

    public class OLiveUpdate : IDisposable
    {

        public OnFinishUpdateEventHandler OnFinishUpdate;

        private bool IsRunning { set; get; }

        public double CurrentVersion { set; get; }

        public string UrlUpdateVersion { set; get; }

        public string UrlUpdateFile { set; get; }

        public string UpdateFile { set; get; }

        public string ServiceName { set; get; }

        public bool HasUpdated { set; get; }

        public int UpdateTimeOut { set; get; }

        public IThreadManager ThreadManager { get; }

        public OLiveUpdate(IThreadManager threadManager)
        {
            ThreadManager = threadManager;
        }

        public void Start()
        {

            if (File.Exists(UpdateFile)) // if there is the update file is still here and has not been removed.
            {
                HasUpdated = true;

                OnFinishUpdate?.Invoke(this, "Live Update: There is a update ready to install, would you like to install this update now?");

            }

            IsRunning = true;

            ThreadManager.Add(
                new OThread(
                    new ThreadStart(Updateloop)
                )
            ).Start(null);

        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void Updateloop()
        {
            while (IsRunning)
            {
                Update();
                Thread.Sleep(UpdateTimeOut);
            }
        }

        private void Update()
        {
            if (HasUpdated)
                return;

            string SyncKey = Guid.NewGuid().ToString();
            double ServerVersion = GetServrPatchVerion(SyncKey);


            if (CurrentVersion < ServerVersion)
            {

                HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(UrlUpdateFile);

                wr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

                wr.Headers.Add("synckey", SyncKey);
                wr.Headers.Add("service", ServiceName);

                HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();
                Stream st = ws.GetResponseStream();

                byte[] inBuf = new byte[ws.ContentLength];
                int bytesToRead = Convert.ToInt32(inBuf.Length);
                int bytesRead = 0;

                while (bytesToRead > 0)
                {
                    if (!IsRunning)
                        break;

                    int n = st.Read(inBuf, bytesRead, bytesToRead);

                    if (n == 0)
                        break;

                    bytesRead += n;

                    bytesToRead -= n;

                    Thread.Sleep(1);

                }

                if (IsRunning)
                {

                    FileStream fs = new(UpdateFile, FileMode.Create, FileAccess.Write);
                    fs.Write(inBuf, 0, bytesRead);
                    st.Close();
                    fs.Close();

                    HasUpdated = true;

                    OnFinishUpdate?.Invoke(this, "Live Update: Download completed, update version " + ServerVersion + ", would you like to restart and install this update now?");

                }
            }
            else
            {
                HasUpdated = false;
                if (File.Exists(UpdateFile))
                    File.Delete(UpdateFile);
            }
        }

        public void Cancel()
        {

            IsRunning = false;

            if (File.Exists(UpdateFile))
                File.Delete(UpdateFile);

        }

        private double GetServrPatchVerion(string SyncKey)
        {
            try
            {

                string s = "0.0";
                WebClient c;
                Stream d;
                StreamReader r;

                try
                {
                    c = new WebClient()
                    {
                        CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
                    };

                    c.Headers.Add("user-agent", "OLiveUpdateService/1.0 (Windows; .NET 4+;)");
                    c.Headers.Add("synckey", SyncKey);
                    c.Headers.Add("service", ServiceName);

                    d = c.OpenRead(UrlUpdateVersion);
                    r = new StreamReader(d);
                    s = r.ReadToEnd();

                    d.Close();
                    r.Close();
                    c.Dispose();

                    c = null;

                }
                catch
                {
                    c = new WebClient()
                    {
                        CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
                    };

                    c.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    c.Headers.Add("synckey", SyncKey);
                    c.Headers.Add("service", ServiceName);

                    d = c.OpenRead(UrlUpdateVersion + "?d=" + SyncKey);
                    r = new StreamReader(d);
                    s = r.ReadToEnd();

                    d.Close();
                    r.Close();
                    c.Dispose();

                    c = null;

                }

                s = s.Trim();

                if (s.Contains("="))
                    s = s[(s.IndexOf("=") + 1)..];

                return Convert.ToDouble(s);

            }
            catch
            {
                return -1;
            }
        }
       
        #region Deconstuctor

        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
                if (disposing)
                {




                }
            IsDisposed = true;
        }

        #endregion

    }

}
