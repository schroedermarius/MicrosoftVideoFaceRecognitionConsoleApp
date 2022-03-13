using System.Drawing;
using FaceDetectionSample;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

string SUBSCRIPTION_KEY = "YOUR_SUBSCRIPTION_KEY";
string ENDPOINT = "https://yourfacerecognitioninstance.cognitiveservices.azure.com/";

try
{
    var videoService = new VideoService("PATH_TO_YOUR_MP4");
    var fileNames = videoService.GetFrameFileNames();

    IFaceClient client = new FaceClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY)) { Endpoint = ENDPOINT };

    foreach (var fileName in fileNames)
    {
        IList<DetectedFace> faces;
        using(var file = File.OpenRead(fileName))
        {
            faces = await client.Face.DetectWithStreamAsync(file, returnFaceLandmarks: true, detectionModel: DetectionModel.Detection03);
        }
        Image img = null;
        try
        {
            img = Image.FromFile(fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        using var graphics = Graphics.FromImage(img);
        foreach (var face in faces)
        {
            var faceRect = new Rectangle(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width, face.FaceRectangle.Height);
            graphics.DrawRectangle(new Pen(Color.Red, 2), faceRect);

            DrawPoints(graphics, face.FaceLandmarks.MouthLeft);
            DrawPoints(graphics, face.FaceLandmarks.MouthRight);
            DrawPoints(graphics, face.FaceLandmarks.PupilLeft);
            DrawPoints(graphics, face.FaceLandmarks.PupilRight);
            DrawPoints(graphics, face.FaceLandmarks.NoseTip);
            DrawPoints(graphics, face.FaceLandmarks.EyebrowLeftOuter); 
            DrawPoints(graphics, face.FaceLandmarks.EyebrowLeftInner); 
            DrawPoints(graphics, face.FaceLandmarks.EyeLeftOuter); 
            DrawPoints(graphics, face.FaceLandmarks.EyeLeftTop); 
            DrawPoints(graphics, face.FaceLandmarks.EyeLeftBottom); 
            DrawPoints(graphics, face.FaceLandmarks.EyeLeftInner); 
            DrawPoints(graphics, face.FaceLandmarks.EyebrowRightInner); 
            DrawPoints(graphics, face.FaceLandmarks.EyebrowRightOuter); 
            DrawPoints(graphics, face.FaceLandmarks.EyeRightInner);
            DrawPoints(graphics, face.FaceLandmarks.EyeRightTop);
            DrawPoints(graphics, face.FaceLandmarks.EyeRightBottom);
            DrawPoints(graphics, face.FaceLandmarks.EyeRightOuter );
            DrawPoints(graphics, face.FaceLandmarks.NoseRootLeft);
            DrawPoints(graphics, face.FaceLandmarks.NoseRootRight);
            DrawPoints(graphics, face.FaceLandmarks.NoseLeftAlarTop);
            DrawPoints(graphics, face.FaceLandmarks.NoseRightAlarTop);
            DrawPoints(graphics, face.FaceLandmarks.NoseLeftAlarOutTip);
            DrawPoints(graphics, face.FaceLandmarks.NoseRightAlarOutTip);
            DrawPoints(graphics, face.FaceLandmarks.UpperLipTop); 
            DrawPoints(graphics, face.FaceLandmarks.UpperLipBottom); 
            DrawPoints(graphics, face.FaceLandmarks.UnderLipTop);
            DrawPoints(graphics, face.FaceLandmarks.UnderLipBottom);
        }
        
        img.Save(fileName);
        
        videoService.CreateVideo();
    }

    
    // Wait for keypress to stop
    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


void DrawPoints(Graphics g, Coordinate coordinate)
{
    g.DrawCircle(new Pen(Color.Black, 2f), (float) coordinate.X, (float) coordinate.Y, 3f);
    g.FillCircle(new SolidBrush(Color.Aqua), (float) coordinate.X, (float) coordinate.Y, 3f);
}