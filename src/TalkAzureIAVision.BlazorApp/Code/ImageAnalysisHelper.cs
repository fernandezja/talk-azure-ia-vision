using Azure.AI.Vision.ImageAnalysis;
using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.IO;
using Microsoft.Extensions.Options;

namespace TalkAzureIAVision.BlazorApp.Code
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/call-analyze-image-40
    /// </summary>
    public class ImageAnalysisHelper
    {
        
        private TalkAzureIAVisionConfig _config;

        /// <summary>
        /// https://aka.ms/cv-languages 
        /// </summary>
        public string Language { get; set; } = "en"; //en|es

        public ImageAnalysisHelper(TalkAzureIAVisionConfig config)
        {
            _config = config;
        }

        public ImageAnalysisResult AnalyzeBuffer(Stream imageBuffer) {

            // This sample assumes you have an image in a memory buffer. Here we simply load it from file.
            //Memory<byte> imageBuffer = System.IO.File.ReadAllBytes("sample.jpg").AsMemory();

            var imageData = BinaryData.FromStream(imageBuffer);
            return AnalyzeBuffer(_config.Endpoint, _config.Key, imageData);
        }

        public ImageAnalysisResult AnalyzeBuffer(BinaryData imageData)
        {
            return AnalyzeBuffer(_config.Endpoint, _config.Key, imageData);
        }

        public ImageAnalysisResult AnalyzeBuffer(string endpoint, string key, BinaryData imageData)
        {
            // Create an Image Analysis client.
            var client = new ImageAnalysisClient(
                new Uri(endpoint),
                new AzureKeyCredential(key));

            VisualFeatures visualFeatures = VisualFeatures.Tags;

            if (Language == "en")
            {
                visualFeatures = VisualFeatures.Caption |
                                            VisualFeatures.DenseCaptions |
                                            VisualFeatures.Objects |
                                            VisualFeatures.Read |
                                            VisualFeatures.Tags |
                                            VisualFeatures.People |
                                            VisualFeatures.SmartCrops;

            }

            if (Language == "es")
            {
                visualFeatures = VisualFeatures.Objects |
                                    VisualFeatures.Tags |
                                    VisualFeatures.People;
            }




            //// Create an ImageSourceBuffer, and copy the input image into it
            //using var imageSourceBuffer = new ImageSourceBuffer();
            //imageSourceBuffer.GetWriter().Write(imageBuffer);

            //// Create your VisionSource from the ImageSourceBuffer
            //using var imageSource = VisionSource.FromImageSourceBuffer(imageSourceBuffer);

            ImageAnalysisOptions analysisOptions = new ImageAnalysisOptions
            {
                GenderNeutralCaption = true,
                Language = Language, 
                SmartCropsAspectRatios = new float[] { 0.9F, 1.33F }
            };

           
            ImageAnalysisResult result = client.Analyze(imageData,
                                                        visualFeatures,
                                                        analysisOptions);


            return result;
        }

    }
}
