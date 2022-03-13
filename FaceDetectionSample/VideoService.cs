using System.Reflection;
using FFMpegCore;
using FFMpegCore.Enums;

namespace FaceDetectionSample;

public class VideoService
{
    private string _tmpPath;
    public VideoService(string path)
    {
        var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        _tmpPath = Path.Combine(directoryName, "tmp");
        if (!Directory.Exists(_tmpPath))
        {
            Directory.CreateDirectory(_tmpPath);
        }
        
        FFMpegArguments.FromFileInput(path)
            .OutputToFile($"{_tmpPath}/Frame%05d.png", true, options => { options.WithVideoCodec(VideoCodec.Png); }).ProcessSynchronously();
    }

    public string[] GetFrameFileNames()
    {
        var files = Directory.GetFiles(_tmpPath)
            .Where(f => f.EndsWith(".png"))
            .OrderBy(f => f)
            .ToArray();
        return files;
    }

    public void CreateVideo()
    {

        var files = GetFrameFileNames();
        FFMpegArguments.FromDemuxConcatInput(files).OutputToFile($"{_tmpPath}/result.mpeg", overwrite: true).ProcessSynchronously();
    }
}