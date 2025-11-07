using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

// OpenAI namespaces
using OpenAI;
using OpenAI.Audio;


class Program
{
  static async Task Main()
  {
    // List recording devices
    Console.WriteLine("Available recording devices:");
    for (int i = 0; i < WaveInEvent.DeviceCount; i++)
    {
      var deviceInfo = WaveInEvent.GetCapabilities(i);
      Console.WriteLine($"{i}: {deviceInfo.ProductName}");
    }

    Console.Write("\nEnter device number for VB-Cable Output: ");
    int deviceNumber = int.Parse(Console.ReadLine() ?? "0");

    string fileName = "mic.wav";

    // Record 5 seconds of audio
    using var waveIn = new WaveInEvent
    {
      DeviceNumber = deviceNumber,
      WaveFormat = new WaveFormat(44100, 1)
    };
    using var writer = new WaveFileWriter(fileName, waveIn.WaveFormat);
    waveIn.DataAvailable += (s, a) => writer.Write(a.Buffer, 0, a.BytesRecorded);

    waveIn.StartRecording();
    Console.WriteLine("🎙️ Recording for 5 seconds...");
    Thread.Sleep(5000);
    waveIn.StopRecording();
    Console.WriteLine($"✅ Saved to {fileName}");

    // Transcribe audio using Whisper
    string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    var client = new OpenAIClient(new OpenAIAuthentication(apiKey));

    var transcription = await client.Audio.Transcriptions.CreateTranscriptionAsync(
        new FileStream(fileName, FileMode.Open),
        model: "whisper-1"
    );

    Console.WriteLine($"🗣️ You said: {transcription.Text}");
  }
}
