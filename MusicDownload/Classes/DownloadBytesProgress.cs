namespace MusicDownload.Classes
{
    public class DownloadBytesProgress
    {
        public DownloadBytesProgress(string fileName, long bytesReceived, long totalBytes)
        {
            Filename = fileName;
            BytesReceived = bytesReceived;
            TotalBytes = totalBytes;
        }

        public long TotalBytes { get; private set; }

        public long BytesReceived { get; private set; }

        public float PercentComplete { get { return (float)BytesReceived / TotalBytes; } }

        public string Filename { get; private set; }

        public bool IsFinished { get { return BytesReceived == TotalBytes; } }
    }

}



