using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace blog.web.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {

        private readonly IConfiguration _configuration;

        // calling cloudinary ac which take 3 par
        private readonly Account account;

        public CloudinaryImageRepository(IConfiguration configuration)
        {

            this._configuration = configuration;

            // in order to test this function go for cloudinary website and create a cloudinar account then copy the name , apikey and apisecret key into appsettings.
           
            // Note: this key may wouldnt work for you because I might going to generate new key.
            account = new Account(

                _configuration.GetSection("Cloudinary")["CloudName"],
                _configuration.GetSection("Cloudinary")["ApiKey"],
                _configuration.GetSection("Cloudinary")["ApiSecret"]

                );
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var cleint = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName,file.OpenReadStream()),DisplayName = file.FileName
               
            };
            var uploadResult = await cleint.UploadAsync(uploadParams);

            
            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK) 
            {
             return uploadResult.SecureUri.ToString();

            }
            return null;
        }
    }
}
