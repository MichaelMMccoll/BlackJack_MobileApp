using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_2425.ViewModels
{
    public class TakingImage
    {
        //Takes an image 
        public async Task<ImageInfo> TakePicture()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                try
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // Get a local storage path
                        var localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                        // Save the photo
                        using var stream = await photo.OpenReadAsync();
                        using var newStream = File.OpenWrite(localPath);
                        await stream.CopyToAsync(newStream);

                        Console.WriteLine($"Photo saved to: {localPath}");
                        return new ImageInfo()
                        { 
                            ImagePath = localPath,
                            Image = photo.FileName
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error taking photo: {ex.Message}");
                }
            }
            return new ImageInfo();
        }
    }
            public class ImageInfo()
            {
                public string ImagePath { get; set; } = "";
                public string Image { get; set; } = "";
            }
}
