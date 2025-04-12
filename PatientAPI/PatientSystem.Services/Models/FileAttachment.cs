namespace PatientSystem.Services.Models
{
    public class FileAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string AttachmentContext { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
    }
}
