using System;

namespace Microsoft.Services.Tools.BizTalkOM
{
    public enum CertificateUsageType : int
    {
        None = 0,
        Encryption = 1,
        Signature = 2,
        SigningAndEncryption = 3,
    }

    public class EncryptionCert : BizTalkBaseObject
    {
        private string longName;
        private string shortName;
        private string thumbPrint;
        private CertificateUsageType usage;

        public EncryptionCert()
        {
        }

        public EncryptionCert(BizTalk.ExplorerOM.CertificateInfo certInfo)
            : this()
        {
            if (certInfo != null)
            {
                this.LongName = certInfo.LongName;
                this.ShortName = certInfo.ShortName;
                this.ThumbPrint = certInfo.ThumbPrint;
                this.Usage = (CertificateUsageType)Enum.Parse(typeof(CertificateUsageType), certInfo.UsageType.ToString("d"));
            }
        }

        public string LongName
        {
            get { return this.longName; }
            set { this.longName = value; }
        }

        public string ShortName
        {
            get { return this.shortName; }
            set { this.shortName = value; }
        }

        public string ThumbPrint
        {
            get { return this.thumbPrint; }
            set { this.thumbPrint = value; }
        }

        public CertificateUsageType Usage
        {
            get { return this.usage; }
            set { this.usage = value; }
        }
    }
}
