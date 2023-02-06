namespace Mono.BlazorServer.Shared.Constants
{
    public static class FileConstant
    {
        /*
         * Allowed file extensions: jpg, jpeg, png, gif, mp3, xls, xlsx, ppt, pptx, doc, docx, pdf, rar, zip, 7z, txt, csv, mp4, mov, avi, wmv
        */
        public static readonly string[] AllowedContentTypes = new string[] { 
            "image/jpeg", "image/png", "image/gif", "audio/mpeg", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/pdf", "application/vnd.rar", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "application/zip", "text/plain", "text/csv", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/x-7z-compressed", "video/mp4", "video/quicktime", "video/x-ms-wmv", "video/x-msvideo"
        };      
        public static readonly string[] ImageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
        public static readonly long MaximumFileSize = 1024 * 1024 * 100; // 100 Mb
    }
}
