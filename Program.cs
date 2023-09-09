using System;

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel;

class Program
{
    static string link = "https://i.pinimg.com/474x/3c/d0/04/3cd0049b0614a721442dfeb1c6524483.jpg";
    static string link1 = "https://static.chipdip.ru/lib/549/DOC001549488.pdf";
    static string link2 = "https://presentation-creation.ru/index.php?option=com_attachments&task=download&id=212";
    static string path = @"D:\";
    static string filename = "1.png";
    static string filename1 = "2.pdf";
    static string filename2 = "3.pptx";


    private static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            Console.WriteLine("File download cancelled.");
        }

        if (e.Error != null)
        {
            Console.WriteLine(e.Error.ToString());
        }
    }
    /*
    private static void DownloadFile(string address)
    {
        WebClient client = new WebClient();
        Uri uri = new Uri(address);
        Console.WriteLine("Downloading File from {0} .......\n\n", link);
        client.DownloadFile(uri, path + filename);
        Console.WriteLine("End download");
    }
    public static void DownLoadFileInBackground(string address)
    {
        WebClient client = new WebClient();
        Uri uri = new Uri(address);

        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        client.DownloadFileAsync(uri, path + filename);
    }
    */
    async static Task Main()
    {

        //запуск через WebClient
      /*  var DownloadTask = DownLoadFile(link, path, filename);
        var DownloadTask1 = DownLoadFile(link1, path, filename1);
        var DownloadTask2 = DownLoadFile(link2, path, filename2);

        await DownloadTask;
        await DownloadTask1;
        await DownloadTask2;
      */
        //запуск через HttpClient
        await DownloadAndSave(link1, path, filename1);
       await DownloadAndSave(link2, path, filename2);
        await DownloadAndSave(link, path, filename);


        async Task DownloadAndSave(string sourceFile, string destinationFolder, string destinationFileName)
        {
            Stream fileStream = await GetFileStream(sourceFile);

            if (fileStream != Stream.Null)
            {
                await SaveStream(fileStream, destinationFolder, destinationFileName);
            }
        }
        async Task<Stream> GetFileStream(string fileUrl)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                Stream fileStream = await httpClient.GetStreamAsync(fileUrl);
                return fileStream;
            }
            catch (Exception ex)
            {
                return Stream.Null;
            }
        }
        async Task SaveStream(Stream fileStream, string destinationFolder, string destinationFileName)
        {
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string path = Path.Combine(destinationFolder, destinationFileName);

            using (FileStream outputFileStream = new FileStream(path, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(outputFileStream);
            }
        }

 /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        async Task DownLoadFile(string address,string pathname, string fname)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(address);

            await Task.Run(() => client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback));
            //await Task.Run(() => client.DownloadFileAsync(uri, path + filename));
            //await Task.Run(async () => await client.DownloadFileTaskAsync(uri, path + filename));
            await client.DownloadFileTaskAsync(uri, pathname+fname);

        }

    }
}