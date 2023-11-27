namespace OpenSocials.App_Code
{
    using System;
    using System.IO;

    public class Media
    {
        private string _mediaTitle;
        private string _mediaDescription;
        private string _mediaType;
        private string _mediaLocalUrl;
        private string _base64;

        public string MediaTitle
        {
            get { return _mediaTitle; }
            set { _mediaTitle = value; }
        }

        public string MediaDescription
        {
            get { return _mediaDescription; }
            set { _mediaDescription = value; }
        }

        public string MediaType
        {
            get { return _mediaType; }
            set { _mediaType = value; }
        }

        public string MediaLocalUrl
        {
            get { return _mediaLocalUrl; }
            set { _mediaLocalUrl = value; }
        }

        public string Base64
        {
            get { return _base64; }
            set { _base64 = value; }
        }

        public Media()
        {

        }
        public Media(string? title, string? description, string localUrl)
        {
            this.MediaTitle = title;
            this.MediaDescription = description;
            this.MediaLocalUrl = localUrl;

            // Determina o tipo de arquivo (foto ou video)
            string fileExtension = Path.GetExtension(localUrl);
            if (fileExtension != null)
            {
                fileExtension = fileExtension.ToLower();
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    MediaType = "image";
					//this.ConvertToBase64();
                }
                else if (fileExtension == ".mp4" || fileExtension == ".avi" || fileExtension == ".mov")
                {
                    MediaType = "video";
                    //this.ConvertToBase64();
                }
                else
                {
                    throw new ArgumentException("Formato de midia nao reconhecido!");
                }
            }
            else
            {
                throw new ArgumentException("Localizacao de arquivo invalida!");
            }
        }

        public void ConvertToBase64()
        {
            using (FileStream fileStream = File.OpenRead(MediaLocalUrl))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, (int)fileStream.Length);
                this.Base64 = Convert.ToBase64String(fileBytes);
            }
        }

        public string DetectMediaType(string base64)
        {
            // Check for common prefixes that indicate media types
            if (base64.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
            {
                return "image";
            }
            else if (base64.StartsWith("data:video/", StringComparison.OrdinalIgnoreCase))
            {
                return "video";
            }

            // Default to unknown
            return "unknown";
        }
    }
}
