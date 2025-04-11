namespace BugTracker.Data.models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public int BugId { get; set; }
        public Bug Bug { get; set; }
    }
}
