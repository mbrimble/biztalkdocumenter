
namespace Microsoft.Services.Tools.BiztalkDocumenter.Publishers.Word
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class WordXmlWriter
    {
        private Stream stream;

        public WordXmlWriter(Stream stream)
        {
            this.stream = stream;
        }

        public Stream BaseStream
        {
            get { return this.stream; }
            set { this.stream = value; }
        }

        public void WriteStartDocument()
        {
            this.WriteStartDocument("Arial");
        }

        public void WriteStartDocument(string defaultFontName)
        {
            this.WriteToStream("<w:wordDocument xmlns:w='http://schemas.microsoft.com/office/word/2003/wordml' xmlns:wx='http://schemas.microsoft.com/office/word/2003/auxHint' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:aml='urn:schemas.microsoft.com/aml/2001/core' xmlns:o='urn:schemas-microsoft-com:office:office' xml:space='preserve'>");
            this.WriteToStream(string.Format("<w:fonts><w:defaultFonts w:ascii='{0}' w:fareast='{0}' w:h-ansi='{0}' w:cs='{0}'/></w:fonts>", defaultFontName));

            this.WriteToStream("<w:docPr><w:view w:val='print'/><w:validateAgainstSchema/><w:saveInvalidXML w:val='off'/><w:showXMLTags w:val='off'/><w:ignoreMixedContent/></w:docPr>");

        }

        public void WriteEndDocument()
        {
            this.WriteToStream("</w:wordDocument>");
            this.BaseStream.Flush();
        }


        public void WriteStartBody()
        {
            this.WriteToStream("<w:body>");
        }

        private string GetImgRunData(string data, int height, int width)
        {
            string id = Guid.NewGuid().ToString();
            return "<w:r><w:pict><v:shapetype id='" + id + "' coordsize='21600,21600' o:spt='75' o:preferrelative='t'/><w:binData w:name='http://" + id + ".gif'>" + data + "</w:binData><v:shape id='_x0000_i1025' type='" + id + "' style='v-align: center;width:" + width.ToString() + "pt;height:" + height.ToString() + "pt'><v:imagedata src='http://" + id + ".gif' o:title='FolderN' /></v:shape></w:pict></w:r>";
        }

        public void WritePicture(string data, int height, int width)
        {
            this.WriteToStream("<w:p>" + GetImgRunData(data, height, width) + "</w:p>");
        }

        public void WriteNewLine()
        {
            this.WriteBodyText("", string.Empty, true);
        }


        public void WritePageBreak()
        {
            this.WriteToStream("<w:p><w:r><w:br w:type='page'/></w:r></w:p>");
        }


        public void WriteBodyText(string text)
        {
            this.WriteBodyText(text, string.Empty);
        }

        public void WriteBodyTextWithPreceedingImage(string text, string styleName, bool pageBreakBefore, string imgData, int height, int width)
        {
            string pageBreak = pageBreakBefore ? "on" : "off";

            StringBuilder sb = new StringBuilder();
            sb.Append("<w:p>");

            if (styleName != null && styleName != string.Empty)
            {
                sb.AppendFormat("<w:pPr><w:pStyle w:val='{0}'/><w:pageBreakBefore w:val='{1}'/></w:pPr>", styleName, pageBreak);
            }

            if (imgData != String.Empty)
            {
                sb.AppendFormat(GetImgRunData(imgData, height, width));
            }

            sb.AppendFormat("<w:r><w:t>{0}</w:t></w:r></w:p>", text);

            this.WriteToStream(sb.ToString());
        }

        public void WriteBodyText(string text, string styleName, bool pageBreakBefore)
        {
            WriteBodyTextWithPreceedingImage(text, styleName, pageBreakBefore, string.Empty, 0, 0);
        }

        public void WriteBodyText(string text, string styleName)
        {
            this.WriteBodyText(text, styleName, false);
        }

        public void WriteEndBody()
        {
            this.WriteToStream("</w:body>");
        }

        public void WriteStartStyles()
        {
            this.WriteToStream("<w:styles>");
        }

        public void WriteStartSection()
        {
            this.WriteToStream("<wx:sect>");
        }

        public void WriteEndSection()
        {
            this.WriteToStream("</wx:sect>");
        }

        public void WriteStartParagraph()
        {
            this.WriteToStream("<w:p>");
        }

        public void WriteEndParagraphn()
        {
            this.WriteToStream("</w:p>");
        }

        public void WriteStartSubSection()
        {
            this.WriteToStream("<wx:sub-section>");
        }

        public void WriteEndSubSection()
        {
            this.WriteToStream("</wx:sub-section>");
        }

        public void WriteEndStyles()
        {
            this.WriteToStream("</w:styles>");
        }

        public void WriteStyle(string styleName, string scope, string fontName, int fontSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<w:style w:type='{0}' w:styleId='{1}'><w:rPr>", scope, styleName);
            sb.AppendFormat("<w:rFonts w:ascii='{0}' w:h-ansi='{0}'/>", fontName);
            sb.AppendFormat("<w:sz w:val='{0}'/>", fontSize * 2);
            sb.AppendFormat("</w:rPr>");
            sb.AppendFormat("</w:style>");
            this.WriteToStream(sb.ToString());
        }

        public void WriteStyle(string styleName, string scope, string fontName, int fontSize, bool underline, bool bold, Color foreColor, Alignment alignment)
        {
            this.WriteStyle(styleName, scope, fontName, fontSize, underline, bold, foreColor, Color.White, alignment);
        }

        public void WriteStyle(string styleName, string scope, string fontName, int fontSize, bool underline, bool bold, Color foreColor, Color backgroundColor, Alignment alignment)
        {
            this.WriteStyle(styleName, scope, fontName, fontSize, underline, bold, foreColor, Color.White, alignment, -1);
        }

        public void WriteStyle(string styleName, string scope, string fontName, int fontSize, bool underline, bool bold, Color foreColor, Color backgroundColor, Alignment alignment, int outlineLevel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<w:style w:type='{0}' w:styleId='{1}'><w:rPr>", scope, styleName);
            sb.AppendFormat("<w:rFonts w:ascii='{0}' w:h-ansi='{0}'/>", fontName);
            sb.AppendFormat("<w:sz w:val='{0}'/>", fontSize * 2);
            //sb.AppendFormat("<w:shadow w:val='on'/>");

            if (underline)
            {
                sb.AppendFormat("<w:u w:val='single'/>");
            }

            if (bold)
            {
                sb.AppendFormat("<w:b w:val='on'/>");
            }
            else
            {
                sb.AppendFormat("<w:b w:val='off'/>");
            }

            sb.AppendFormat("<w:color w:val='{0}'/>", ColorTranslator.ToHtml(foreColor));
            sb.AppendFormat("</w:rPr>");

            sb.Append("<w:pPr>");
            sb.AppendFormat("<w:shd w:val='clear' w:color='auto' w:fill='{0}'/>", ColorTranslator.ToHtml(backgroundColor));
            sb.AppendFormat("<w:jc w:val='{0}' />", alignment.ToString().ToLower());
            if (outlineLevel != -1)
            {
                sb.AppendFormat("<w:outlineLvl w:val='{0}'/>", outlineLevel);
            }
            sb.Append("</w:pPr>");

            sb.AppendFormat("</w:style>");
            this.WriteToStream(sb.ToString());
        }

        public void WriteFooterString(string text)
        {
            this.WriteFooterString(text, string.Empty);
        }

        public void WriteFooterString(string text, string styleName)
        {
            this.WriteToStream("<w:ftr>");
            this.WriteBodyText(text, styleName);
            this.WriteToStream("</w:ftr>");
        }

        public void WriteStartBookmark(string id)
        {
            this.WriteToStream("<aml:annotation aml:id='0' w:type='Word.Bookmark.Start' w:name='" + id + "' />");
        }

        public void WriteEndBookmark()
        {
            this.WriteToStream("<aml:annotation aml:id='0' w:type='Word.Bookmark.End' />");
        }

        public void WriteStartTable(string styleName)
        {
            this.WriteToStream("<w:tbl><w:tblPr><w:tblW w:w='0' w:type='auto'/><w:tblStyle w:val='" + styleName + "'/></w:tblPr>");
        }

        public void WriteTableRowData(params string[] cellValues)
        {
            TableCellData[] tcd = new TableCellData[cellValues.Length];

            for (int i = 0; i < cellValues.Length; i++)
            {
                tcd[i] = new TableCellData(cellValues[i]);
            }
            this.WriteTableRow(tcd);
        }

        public void WriteTableRow(params TableCellData[] cellData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<w:tr>");

            foreach (TableCellData data in cellData)
            {
                string bgColor = ColorTranslator.ToHtml(data.BackgroundColor);
                string fgColor = ColorTranslator.ToHtml(data.ForeColor);
                string bold = data.Bold ? "on" : "off";

                sb.AppendFormat("<w:tc>");
                sb.AppendFormat("<w:tcPr><w:shd w:val='clear' w:color='auto' w:fill='{0}'/></w:tcPr>", bgColor);
                sb.AppendFormat("<w:p><w:r><w:rPr><w:color w:val='{0}'/><w:b w:val='{1}'/></w:rPr><w:t>{2}</w:t></w:r></w:p>", fgColor, bold, data.Text);
                sb.AppendFormat("</w:tc>");
            }

            sb.Append("</w:tr>");
            this.WriteToStream(sb.ToString());
        }

        public void WriteEndTable()
        {
            this.WriteToStream("</w:tbl>");
        }

        private byte[] TextToBytes(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        private void WriteToStream(string data)
        {
            if (this.stream.CanWrite)
            {
                byte[] buffer = this.TextToBytes(data);
                this.stream.Write(buffer, 0, buffer.Length);
            }
        }
    }

    public enum Alignment : int
    {
        Left = 1,
        Center = 2,
        Right = 3,
    }

    public class TableCellData
    {
        public bool Bold;
        public Color ForeColor = Color.Black;
        public Color BackgroundColor = Color.White;
        public string Text;

        public TableCellData()
        {
        }

        public TableCellData(string text)
            : this()
        {
            this.Text = text;
        }

        public TableCellData(string text, bool bold)
            : this(text)
        {
            this.Bold = bold;
        }

        public TableCellData(string text, bool bold, Color bgColor)
            : this(text, bold)
        {
            this.BackgroundColor = bgColor;
        }

        public TableCellData(string text, bool bold, Color bgColor, Color fgColor)
            : this(text, bold, bgColor)
        {
            this.ForeColor = fgColor;
        }
    }
}
