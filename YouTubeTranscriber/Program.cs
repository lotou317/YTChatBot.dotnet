using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
  static async Task Main(string[] args)
  {
    var apiKey = "YOUR_API_KEY_HERE"; // replace with your OpenAI API key

    var openAiService = new OpenAIService(new OpenAiOptions()
    {
      ApiKey = apiKey
    });

    var audioFile = "audio.mp3"; // replace with your file path

    if (!File.Exists(audioFile))
    {
      Console.WriteLine("Audio file not found!");
      return;
    }

    using var stream = File.OpenRead(audioFile);

    var result = await openAiService.Audio.CreateTranscription(new AudioTranscriptionCreateRequest()
    {
      File = stream,
      Model = "whisper-1"
    });

    Console.WriteLine("Transcription Result:");
    Console.WriteLine(result.Text);
  }
}
