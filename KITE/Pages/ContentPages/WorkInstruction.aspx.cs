using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace KITE.Pages.ContentPages
{
    public partial class WorkInstruction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> fileNames = new List<string>() {
                "Work Instruction for KITE Tracing Feature For Beacukai Team.docx",
                "Work Instruction for KITE Tracing Feature For PTA.docx"
            };
            var filesPath = Server.MapPath("~/WorkInstructions/");
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var fileName in fileNames)
                    {
                        var filePath = Path.Combine(filesPath, fileName);

                        if (File.Exists(filePath))
                        {
                            zipArchive.CreateEntryFromFile(filePath, fileName);
                        }
                    }
                }
                // Set response properties for the zip file
                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", $"attachment;filename=WorkInstructions.zip");
                zipStream.Seek(0, SeekOrigin.Begin);
                zipStream.CopyTo(Response.OutputStream);
                Response.End();
            }
        }
    }
}