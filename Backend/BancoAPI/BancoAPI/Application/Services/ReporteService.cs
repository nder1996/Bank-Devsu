using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.Text;

namespace BancoAPI.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
        }

        public async Task<ReporteEstadoCuentaResponseDto> GenerarEstadoCuentaAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var datos = await _reporteRepository.GetEstadoCuentaAsync(clienteId, fechaInicio, fechaFin);
            var (totalDebitos, totalCreditos) = await _reporteRepository.GetTotalesMovimientosAsync(clienteId, fechaInicio, fechaFin);

            return new ReporteEstadoCuentaResponseDto
            {
                Datos = datos,
                TotalDebitos = totalDebitos,
                TotalCreditos = totalCreditos,
                TotalMovimientos = datos.Count(),
                FechaGeneracion = DateTime.Now
            };
        }

        public async Task<byte[]> GenerarEstadoCuentaPdfAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reporte = await GenerarEstadoCuentaAsync(clienteId, fechaInicio, fechaFin);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 70, 110);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                // Agregar evento para header y footer
                var headerFooter = new HeaderFooterPageEvent(clienteId, fechaInicio, fechaFin, reporte.FechaGeneracion);
                writer.PageEvent = headerFooter;

                document.Open();

                // Colores pasteles profesionales para banca
                BaseColor primaryColor = new BaseColor(72, 101, 145); // Azul corporativo suave
                BaseColor secondaryColor = new BaseColor(248, 250, 252); // Gris perláceo
                BaseColor accentColor = new BaseColor(135, 159, 189); // Azul pastel
                BaseColor headerColor = new BaseColor(91, 115, 148); // Azul header suave
                BaseColor successColor = new BaseColor(76, 144, 99); // Verde mint
                BaseColor dangerColor = new BaseColor(185, 94, 94); // Rojo rosa suave
                BaseColor textColor = new BaseColor(64, 74, 84); // Gris carbón suave
                BaseColor alternateRow = new BaseColor(252, 253, 254); // Blanco perlado

                // Espacio para el header (se maneja automáticamente)
                document.Add(new Paragraph(" ") { SpacingAfter = 15 });

                // Información del reporte en una tabla
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 1f, 1f });
                infoTable.SpacingAfter = 20;

                Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, textColor);
                Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, textColor);

                // Obtener nombre del cliente del primer registro si existe
                var nombreCliente = reporte.Datos.FirstOrDefault()?.Cliente ?? $"Cliente #{clienteId}";

                AddInfoCell(infoTable, "CLIENTE:", nombreCliente, labelFont, valueFont, secondaryColor);
                AddInfoCell(infoTable, "PERÍODO:", $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}", labelFont, valueFont, secondaryColor);
                AddInfoCell(infoTable, "FECHA GENERACIÓN:", reporte.FechaGeneracion.ToString("dd/MM/yyyy HH:mm:ss"), labelFont, valueFont, secondaryColor);
                AddInfoCell(infoTable, "TOTAL MOVIMIENTOS:", reporte.TotalMovimientos.ToString("N0"), labelFont, valueFont, secondaryColor);

                document.Add(infoTable);

                // Sección de resumen financiero
                Font summaryTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, primaryColor);
                Paragraph summaryTitle = new Paragraph("RESUMEN EJECUTIVO", summaryTitleFont);
                summaryTitle.SpacingAfter = 15;
                summaryTitle.SpacingBefore = 10;
                
                // Línea decorativa suave bajo el título
                PdfPTable titleLine = new PdfPTable(1);
                titleLine.WidthPercentage = 100;
                titleLine.SpacingAfter = 15;
                PdfPCell lineCell = new PdfPCell();
                lineCell.BackgroundColor = accentColor;
                lineCell.FixedHeight = 1.5f;
                lineCell.Border = Rectangle.NO_BORDER;
                titleLine.AddCell(lineCell);
                
                document.Add(summaryTitle);
                document.Add(titleLine);

                PdfPTable summaryTable = new PdfPTable(3);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 1f, 1f, 1f });
                summaryTable.SpacingAfter = 25;

                // Totales con colores
                AddSummaryCell(summaryTable, "TOTAL CRÉDITOS", $"${reporte.TotalCreditos:N2}", successColor);
                AddSummaryCell(summaryTable, "TOTAL DÉBITOS", $"${Math.Abs(reporte.TotalDebitos):N2}", dangerColor);
                AddSummaryCell(summaryTable, "BALANCE NETO", $"${(reporte.TotalCreditos + reporte.TotalDebitos):N2}", primaryColor);

                document.Add(summaryTable);

                // Título de detalle de transacciones
                Font movimientosTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, primaryColor);
                Paragraph movTitle = new Paragraph("DETALLE DE TRANSACCIONES", movimientosTitle);
                movTitle.SpacingAfter = 15;
                movTitle.SpacingBefore = 20;
                
                // Línea decorativa suave
                PdfPTable movTitleLine = new PdfPTable(1);
                movTitleLine.WidthPercentage = 100;
                movTitleLine.SpacingAfter = 15;
                PdfPCell movLineCell = new PdfPCell();
                movLineCell.BackgroundColor = accentColor;
                movLineCell.FixedHeight = 1.5f;
                movLineCell.Border = Rectangle.NO_BORDER;
                movTitleLine.AddCell(movLineCell);
                
                document.Add(movTitle);
                document.Add(movTitleLine);

                // Tabla de transacciones con mejor espaciado
                if (reporte.Datos.Any())
                {
                    PdfPTable table = new PdfPTable(8);
                    table.WidthPercentage = 100;
                    // Optimizar anchos para fuentes más pequeñas
                    table.SetWidths(new float[] { 11f, 20f, 14f, 9f, 14f, 9f, 13f, 14f });
                    table.SpacingBefore = 10;
                    table.SpacingAfter = 30;

                    // Encabezados profesionales más compactos
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE);
                    string[] headers = { "FECHA", "TITULAR", "No. CUENTA", "TIPO", "SALDO INICIAL", "STATUS", "TRANSACCIÓN", "SALDO FINAL" };

                    foreach (string header in headers)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont));
                        headerCell.BackgroundColor = headerColor;
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.Padding = 8;
                        headerCell.BorderColor = new BaseColor(255, 255, 255);
                        headerCell.BorderWidth = 0.8f;
                        table.AddCell(headerCell);
                    }

                    // Datos con fuentes más pequeñas para mejor visualización
                    Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, textColor);
                    Font dataBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, textColor);
                    Font numberFont = FontFactory.GetFont(FontFactory.HELVETICA, 7.5f, textColor);
                    Font headerDataFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.WHITE);
                    bool isEvenRow = false;

                    foreach (var item in reporte.Datos)
                    {
                        BaseColor rowColor = isEvenRow ? alternateRow : BaseColor.WHITE;
                        isEvenRow = !isEvenRow;

                        // Usar fuentes más pequeñas para datos compactos
                        Font smallDateFont = FontFactory.GetFont(FontFactory.HELVETICA, 7, textColor);
                        Font smallTextFont = FontFactory.GetFont(FontFactory.HELVETICA, 7, textColor);
                        Font smallNumberFont = FontFactory.GetFont(FontFactory.HELVETICA, 6.5f, textColor);
                        Font smallBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, textColor);
                        
                        AddDataCellProfessional(table, item.Fecha, smallDateFont, rowColor, Element.ALIGN_CENTER);
                        AddDataCellProfessional(table, item.Cliente.ToUpper(), smallTextFont, rowColor, Element.ALIGN_LEFT);
                        AddDataCellProfessional(table, $"****{item.NumeroCuenta.Substring(Math.Max(0, item.NumeroCuenta.Length - 4))}", smallBoldFont, rowColor, Element.ALIGN_CENTER);
                        AddDataCellProfessional(table, item.Tipo.ToUpper(), smallTextFont, rowColor, Element.ALIGN_CENTER);
                        AddDataCellProfessional(table, $"${item.SaldoInicial:N2}", smallNumberFont, rowColor, Element.ALIGN_RIGHT);
                        
                        // Estado con colores suaves y fuente pequeña
                        Font statusFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6.5f, item.Estado ? successColor : dangerColor);
                        string statusText = item.Estado ? "ACTIVA" : "SUSPENDIDA";
                        AddDataCellProfessional(table, statusText, statusFont, rowColor, Element.ALIGN_CENTER);
                        
                        // Transacción con colores pasteles y fuente pequeña
                        BaseColor transactionColor = item.Movimiento >= 0 ? successColor : dangerColor;
                        Font transFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, transactionColor);
                        string transText = item.Movimiento >= 0 ? $"+${item.Movimiento:N2}" : $"${item.Movimiento:N2}";
                        AddDataCellProfessional(table, transText, transFont, rowColor, Element.ALIGN_RIGHT);
                        
                        AddDataCellProfessional(table, $"${item.SaldoDisponible:N2}", smallBoldFont, rowColor, Element.ALIGN_RIGHT);
                    }

                    document.Add(table);
                    
                    // Espacio adicional antes del footer para aislarlo
                    document.Add(new Paragraph(" ") { SpacingAfter = 25 });
                }
                else
                {
                    Font noDataFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12, new BaseColor(120, 130, 140));
                    Paragraph noData = new Paragraph("No se registraron transacciones en el período consultado.", noDataFont);
                    noData.Alignment = Element.ALIGN_CENTER;
                    noData.SpacingBefore = 25;
                    noData.SpacingAfter = 15;
                    
                    // Marco decorativo para el mensaje
                    PdfPTable noDataBox = new PdfPTable(1);
                    noDataBox.WidthPercentage = 60;
                    noDataBox.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell noDataCell = new PdfPCell();
                    noDataCell.AddElement(noData);
                    noDataCell.BackgroundColor = secondaryColor;
                    noDataCell.BorderColor = accentColor;
                    noDataCell.BorderWidth = 1f;
                    noDataCell.Padding = 20;
                    noDataBox.AddCell(noDataCell);
                    
                    document.Add(noDataBox);
                }

                document.Close();
                return ms.ToArray();
            }
        }

        private void AddInfoCell(PdfPTable table, string label, string value, Font labelFont, Font valueFont, BaseColor backgroundColor)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
            labelCell.BackgroundColor = backgroundColor;
            labelCell.Padding = 8;
            labelCell.Border = Rectangle.NO_BORDER;
            labelCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
            valueCell.BackgroundColor = backgroundColor;
            valueCell.Padding = 8;
            valueCell.Border = Rectangle.NO_BORDER;
            valueCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(valueCell);
        }

        private void AddSummaryCell(PdfPTable table, string label, string value, BaseColor color)
        {
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(64, 74, 84));
            Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, color);

            PdfPCell cell = new PdfPCell();
            cell.AddElement(new Paragraph(label, labelFont) { Alignment = Element.ALIGN_CENTER });
            cell.AddElement(new Paragraph(value, valueFont) { Alignment = Element.ALIGN_CENTER, SpacingBefore = 6 });
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 18;
            cell.BackgroundColor = new BaseColor(248, 250, 252);
            cell.BorderColor = new BaseColor(200, 215, 235);
            cell.BorderWidth = 1.5f;
            table.AddCell(cell);
        }

        private void AddDataCell(PdfPTable table, string text, Font font, BaseColor backgroundColor, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = backgroundColor;
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            cell.BorderColor = new BaseColor(220, 220, 220);
            cell.BorderWidth = 0.5f;
            table.AddCell(cell);
        }

        private void AddDataCellProfessional(PdfPTable table, string text, Font font, BaseColor backgroundColor, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = backgroundColor;
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 6;
            cell.BorderColor = new BaseColor(220, 230, 240);
            cell.BorderWidth = 0.3f;
            cell.PaddingLeft = alignment == Element.ALIGN_RIGHT ? 8 : 6;
            cell.PaddingRight = alignment == Element.ALIGN_LEFT ? 8 : 6;
            cell.PaddingTop = 5;
            cell.PaddingBottom = 5;
            table.AddCell(cell);
        }

        public async Task<string> GenerarEstadoCuentaJsonAsync(long clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reporte = await GenerarEstadoCuentaAsync(clienteId, fechaInicio, fechaFin);
            return JsonConvert.SerializeObject(reporte, Formatting.Indented);
        }
    }

    public class HeaderFooterPageEvent : PdfPageEventHelper
    {
        private readonly long _clienteId;
        private readonly DateTime _fechaInicio;
        private readonly DateTime _fechaFin;
        private readonly DateTime _fechaGeneracion;

        public HeaderFooterPageEvent(long clienteId, DateTime fechaInicio, DateTime fechaFin, DateTime fechaGeneracion)
        {
            _clienteId = clienteId;
            _fechaInicio = fechaInicio;
            _fechaFin = fechaFin;
            _fechaGeneracion = fechaGeneracion;
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            
            PdfContentByte canvas = writer.DirectContent;
            
            // Líneas decorativas suaves
            // Línea superior principal - azul suave
            /*canvas.SetRGBColorStroke(72, 101, 145);
            canvas.SetLineWidth(3f);
            canvas.MoveTo(document.Left, document.Top - 8);
            canvas.LineTo(document.Right, document.Top - 8);
            canvas.Stroke();*/
            
            // Línea inferior delgada - azul pastel
            /*canvas.SetRGBColorStroke(135, 159, 189);
            canvas.SetLineWidth(1f);
            canvas.MoveTo(document.Left, document.Top - 10);
            canvas.LineTo(document.Right, document.Top - 10);
            canvas.Stroke();*/

            // Header corporativo con colores suaves
            Font bankNameFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, new BaseColor(72, 101, 145));
            Font documentTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, new BaseColor(91, 115, 148));
            Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, new BaseColor(120, 130, 140));
            
            // Nombre del banco
            Phrase bankName = new Phrase("BANCO DEVSU", bankNameFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, bankName, document.Left, document.Top - 30, 0);
            
            // Título del documento
            Phrase docTitle = new Phrase("ESTADO DE CUENTA BANCARIO", documentTitleFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, docTitle, document.Left, document.Top - 45, 0);

            // Fecha y hora de generación
            Phrase datePhrase = new Phrase($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", dateFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, datePhrase, document.Right, document.Top - 30, 0);
            
            // Número de documento
            Phrase docNumber = new Phrase($"Doc. No: EC-{_clienteId}-{DateTime.Now:yyyyMMdd}", dateFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, docNumber, document.Right, document.Top - 45, 0);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            
            // Footer
            PdfContentByte canvas = writer.DirectContent;
            
            // Líneas decorativas inferiores completamente aisladas
            /*canvas.SetRGBColorStroke(135, 159, 189);
            canvas.SetLineWidth(1f);
            canvas.MoveTo(document.Left, document.Bottom + 75);
            canvas.LineTo(document.Right, document.Bottom + 75);
            canvas.Stroke();*/
            
           /* canvas.SetRGBColorStroke(72, 101, 145);
            canvas.SetLineWidth(1.5f);
            canvas.MoveTo(document.Left, document.Bottom + 73);
            canvas.LineTo(document.Right, document.Bottom + 73);
            canvas.Stroke();*/

            // Footer completamente aislado de la tabla
            /*Font footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 7, new BaseColor(120, 130, 140));
            string footerText = $"Período: {_fechaInicio:dd/MM/yyyy} - {_fechaFin:dd/MM/yyyy} • Emitido: {_fechaGeneracion:dd/MM/yyyy HH:mm:ss}";
            Phrase footer = new Phrase(footerText, footerFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, footer, 
                (document.Right + document.Left) / 2, document.Bottom + 55, 0);

            // Número de página aislado
            Font pageFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, new BaseColor(72, 101, 145));
            Phrase pagePhrase = new Phrase($"Página {writer.PageNumber}", pageFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, pagePhrase, document.Right, document.Bottom + 35, 0);

            // Información de confidencialidad aislada
            Font confidentialFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 6, new BaseColor(130, 140, 150));
            Phrase confidential = new Phrase("Confidencial - Documento bancario de uso exclusivo del titular", confidentialFont);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, confidential, document.Left, document.Bottom + 35, 0);*/
        }
    }
}