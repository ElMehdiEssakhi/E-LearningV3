using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace E_LearningV3.Services

{
    public class CertificateService
    {
        public byte[] GenerateCertificate(string studentName,string courseName,DateTime issuedAt)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.Background(Colors.White);

                    page.Content()
                        .AlignCenter()
                        .Column(col =>
                        {
                            col.Spacing(20);

                            col.Item().Text("Certificate of Completion")
                                .FontSize(32)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            col.Item().Text($"This certifies that")
                                .FontSize(16);

                            col.Item().Text(studentName)
                                .FontSize(24)
                                .Bold();

                            col.Item().Text("has successfully completed the course")
                                .FontSize(16);

                            col.Item().Text(courseName)
                                .FontSize(22)
                                .Bold();

                            col.Item().PaddingTop(30)
                                .Text($"Issued on {issuedAt:dd MMM yyyy}")
                                .FontSize(12);
                        });
                });
            }).GeneratePdf();
        }

    }
}
