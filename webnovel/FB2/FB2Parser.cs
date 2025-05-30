using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace bookservice.FB2Logic
{
    public class FB2Parser
    {
        private List<BookChapter> _chapters;
        private XNamespace _fb2Ns;

        public List<BookChapter> ParseFb2File(string filePath)
        {
            _chapters = new List<BookChapter>();
            _fb2Ns = "http://www.gribuser.ru/xml/fictionbook/2.0";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found at {filePath}");
                return null; // Or throw new FileNotFoundException("FB2 file not found.", filePath);
            }

            XDocument doc;
            try
            {
                // Load with LoadOptions.PreserveWhitespace to potentially help with paragraph spacing if issues arise,
                // though RTF conversion handles most explicit spacing.
                doc = XDocument.Load(filePath, LoadOptions.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading FB2 XML from {filePath}: {ex.Message}");
                return null; // Or throw new XmlException($"Error parsing FB2 XML: {ex.Message}", ex);
            }

            XElement fictionBook = doc.Element(_fb2Ns + "FictionBook");
            if (fictionBook == null)
            {
                Console.WriteLine("FB2 file is missing the root <FictionBook> element.");
                return null;
            }


            XElement body = fictionBook.Element(_fb2Ns + "body");
            if (body == null)
            {
                // Some FB2 files might have multiple bodies (e.g. main, notes).
                // For simplicity, we take the first one. If specific body handling is needed by name/title, this logic would expand.
                body = fictionBook.Elements(_fb2Ns + "body").FirstOrDefault();
                if (body == null)
                {
                    Console.WriteLine("FB2 file is missing the <body> element.");
                    return null;
                }
            }

            ProcessBodyElement(body);

            // If no chapters were parsed (e.g., body only had <p> tags directly, no <section>),
            // create a single chapter with the body content.
            if (!_chapters.Any() && body.Elements().Any(el => el.Name.LocalName != "title")) // Check if body has content other than its own title
            {
                StringBuilder bodyRtfBuilder = new StringBuilder();
                bodyRtfBuilder.Append(GetRtfHeader());
                ProcessElementsRecursive(body, bodyRtfBuilder, skipTitle: true); // Skip body's own title if it exists
                bodyRtfBuilder.Append(GetRtfFooter());

                string bodyTitle = "Содержание книги";
                XElement bodyTitleElement = body.Element(_fb2Ns + "title");
                if (bodyTitleElement != null)
                {
                    bodyTitle = GetPlainText(bodyTitleElement).Trim();
                    if (string.IsNullOrWhiteSpace(bodyTitle)) bodyTitle = "Содержание книги";
                }
                _chapters.Add(new BookChapter(bodyTitle, bodyRtfBuilder.ToString()));
            }


            return _chapters;
        }

        private void ProcessBodyElement(XElement element)
        {
            foreach (XElement sectionElement in element.Elements(_fb2Ns + "section"))
            {
                ProcessSection(sectionElement);
            }
            // If no sections found directly under this body, but there are paragraphs, treat the whole body as a single section.
            // This was moved to the caller ParseFb2File to ensure it's a fallback.
        }

        private void ProcessSection(XElement sectionElement)
        {
            string title = "Без названия";
            XElement titleElement = sectionElement.Element(_fb2Ns + "title");
            if (titleElement != null)
            {
                title = GetPlainText(titleElement).Trim();
                if (string.IsNullOrWhiteSpace(title)) title = "Глава"; // Default if title is empty
            }
            else
            {
                // Try to generate a title from the first paragraph if no <title> element exists
                XElement firstP = sectionElement.Descendants(_fb2Ns + "p").FirstOrDefault();
                if (firstP != null)
                {
                    title = GetPlainText(firstP).Trim();
                    if (title.Length > 80) title = title.Substring(0, 80) + "...";
                    if (string.IsNullOrWhiteSpace(title)) title = "Глава";
                }
                else
                {
                    title = $"Глава {_chapters.Count + 1}"; // Fallback title
                }
            }

            StringBuilder chapterRtfBuilder = new StringBuilder();
            chapterRtfBuilder.Append(GetRtfHeader());

            // Add chapter title to RTF content, making it bold and slightly larger
            chapterRtfBuilder.Append(@"\par\pard\qc\b\fs32 "); // Centered, Bold, 16pt (fs32)
            chapterRtfBuilder.Append(EscapeRtfText(title));
            chapterRtfBuilder.Append(@"\b0\fs28\par\pard\ql\par "); // Reset to default (14pt, fs28), left-aligned, add a paragraph break

            ProcessElementsRecursive(sectionElement, chapterRtfBuilder, skipTitle: true);

            chapterRtfBuilder.Append(GetRtfFooter());
            _chapters.Add(new BookChapter(title, chapterRtfBuilder.ToString()));
        }

        private void ProcessElementsRecursive(XElement parentElement, StringBuilder rtfBuilder, bool skipTitle = false)
        {
            foreach (XNode node in parentElement.Nodes())
            {
                if (node is XElement element)
                {
                    if (skipTitle && element.Name.LocalName == "title") continue;

                    if (element.Name.LocalName == "section")
                    {
                        // Nested sections are processed by creating new chapter entries.
                        // The content of a nested section itself is not directly appended here,
                        // but rather ProcessSection is called for it.
                        // For a simpler model where nested sections append to current, this would change.
                        // Current model: each <section> is a distinct chapter.
                        ProcessSection(element); // This will add it as a new chapter.
                        continue;
                    }
                    rtfBuilder.Append(GetElementRtf(element));
                }
                else if (node is XText textNode)
                {
                    rtfBuilder.Append(EscapeRtfText(textNode.Value));
                }
            }
        }

        private string GetElementRtf(XElement element)
        {
            StringBuilder sb = new StringBuilder();
            string tagName = element.Name.LocalName;

            // Common block elements that should start a new paragraph in RTF
            bool isBlock = new[] { "p", "subtitle", "epigraph", "text-author", "poem", "stanza", "v", "title" }.Contains(tagName);
            if (isBlock && tagName != "v") // 'v' (verse line) usually doesn't start a new RTF \par itself if inside a poem structure
            {
                sb.Append(@"\par\pard "); // Reset paragraph formatting before applying specific styles
            }


            switch (tagName)
            {
                case "p":
                    sb.Append(@"\ql "); // Default to left-aligned
                    break;
                case "strong":
                case "b": // FB2.1 uses <strong>, FB2.0 might use <b>
                    sb.Append(@"\b ");
                    break;
                case "emphasis":
                case "i": // FB2.1 uses <emphasis>, FB2.0 might use <i>
                    sb.Append(@"\i ");
                    break;
                case "strikethrough":
                    sb.Append(@"\strike ");
                    break;
                case "sub":
                    sb.Append(@"\sub ");
                    break;
                case "sup":
                    sb.Append(@"\super ");
                    break;
                case "code": // Monospaced font for code
                    sb.Append(@"\f1\fs24 "); // Assuming f1 is Courier New or similar, 12pt
                    break;
                case "subtitle":
                    sb.Append(@"\qc\b\fs30 "); // Centered, Bold, 15pt
                    break;
                case "epigraph":
                    sb.Append(@"\li720\ri720\i "); // Indent left/right, italic
                    break;
                case "text-author": // Typically for epigraphs or poem authors
                    sb.Append(@"\qr\i "); // Right-aligned, italic
                    break;
                case "poem":
                    sb.Append(@"\li360\qc "); // Indent left, centered (stanza content will override alignment)
                    break;
                case "stanza":
                    sb.Append(@"\li720 "); // Further indent for stanzas within a poem
                    break;
                case "v": // Verse line
                    sb.Append(@"\ql "); // Verses are typically left-aligned within their stanza
                    break;
                case "title": // Usually handled by ProcessSection for chapter titles, but if encountered elsewhere
                    sb.Append(@"\qc\b\fs32 ");
                    break;
                case "empty-line":
                    sb.Append(@"\par "); // Just a paragraph break
                    break;
                // TODO: Add handling for images, tables, links if necessary
                default:
                    // For unknown tags, we'll process their children without applying specific formatting.
                    break;
            }

            foreach (XNode node in element.Nodes())
            {
                if (node is XElement childElement) sb.Append(GetElementRtf(childElement));
                else if (node is XText textNode) sb.Append(EscapeRtfText(textNode.Value));
            }

            // Reset formatting
            switch (tagName)
            {
                case "strong": case "b": sb.Append(@"\b0 "); break;
                case "emphasis": case "i": sb.Append(@"\i0 "); break;
                case "strikethrough": sb.Append(@"\strike0 "); break;
                case "sub": sb.Append(@"\nosupersub "); break;
                case "sup": sb.Append(@"\nosupersub "); break;
                case "code": sb.Append(@"\f0\fs28 "); break; // Reset to default font/size
                case "subtitle":
                case "epigraph":
                case "text-author":
                case "poem":
                case "stanza":
                case "title":
                    sb.Append(@"\par\pard\ql\fs28 "); // Reset to default paragraph, left-aligned, default font size
                    break;
            }
            if (tagName == "v") sb.Append(@"\line "); // Add a line break after each verse line

            return sb.ToString();
        }


        private string GetPlainText(XElement element)
        {
            StringBuilder sb = new StringBuilder();
            foreach (XNode node in element.Nodes())
            {
                if (node is XElement childElement)
                {
                    if (childElement.Name.LocalName == "p" && sb.Length > 0) sb.Append(" ");
                    sb.Append(GetPlainText(childElement));
                }
                else if (node is XText textNode) sb.Append(textNode.Value);
            }
            return System.Net.WebUtility.HtmlDecode(sb.ToString().Replace("\n", " ").Replace("\r", "").Trim());
        }

        private string EscapeRtfText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = System.Net.WebUtility.HtmlDecode(text); // Decode HTML entities first

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("\\", "\\\\");
            sb.Replace("{", "\\{");
            sb.Replace("}", "\\}");
            // Standard ANSI characters are fine. For extended characters, RTF uses control words.
            // Example: é becomes \'e9
            // This simple replacement is often enough for Cyrillic if ansicpg1251 is set.
            // For full Unicode support, a more complex mapping to RTF \uXXXX? control words is needed.
            // For Cyrillic with \ansicpg1251, direct characters are usually fine.
            // Let's ensure common problematic characters are handled.
            sb.Replace("\n", @"\line "); // \line is better than \par for soft breaks within a logical paragraph
            sb.Replace("\r", "");       // Remove CR if not already handled by \n
            sb.Replace("\t", @"\tab ");

            // Convert non-ASCII to RTF escape sequences if needed, though ansci cpg usually handles it.
            // For simplicity, this step is omitted, relying on \ansicpg1251 for Cyrillic.
            // If other charsets are common, this would need expansion.

            return sb.ToString();
        }

        public static string GetRtfHeader()
        {
            // Default Segoe UI 14pt (fs28), Cyrillic (ansicpg1251, lang1049)
            // Added Calibri as f1 for potential use (e.g. code blocks)
            return @"{\rtf1\ansi\deff0\ansicpg1251\nouicompat{\fonttbl{\f0\fnil\fcharset204 Segoe UI;}{\f1\fnil\fcharset204 Courier New;}}\pard\sa200\sl276\slmult1\f0\fs28\lang1049 ";
        }

        public static string GetRtfFooter()
        {
            return "}";
        }
    }
}