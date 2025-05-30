using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms; // Required for RichTextBox, Point, Application.DoEvents

namespace bookservice.FB2Logic
{
    public class BookDocument
    {
        public List<BookChapter> Chapters { get; private set; }
        public int TotalPagesInBook { get; private set; }
        public Size PageDimensions { get; private set; } // Physical dimensions of the RTB used for pagination

        public BookDocument()
        {
            Chapters = new List<BookChapter>();
            TotalPagesInBook = 0;
        }

        public bool LoadAndProcessFile(string filePath, Size pageDimensions)
        {
            this.PageDimensions = pageDimensions;
            FB2Parser parser = new FB2Parser();

            List<BookChapter> parsedChapters = parser.ParseFb2File(filePath);
            if (parsedChapters == null || !parsedChapters.Any())
            {
                Console.WriteLine($"File {filePath} could not be parsed or contains no chapters.");
                return false;
            }
            this.Chapters = parsedChapters;

            return PaginateBook();
        }

        private bool PaginateBook()
        {
            TotalPagesInBook = 0;
            if (this.PageDimensions.Width <= 0 || this.PageDimensions.Height <= 0)
            {
                Console.WriteLine("Pagination failed: Page dimensions are invalid.");
                return false;
            }

            // Use a RichTextBox instance for pagination measurements.
            // This RichTextBox should not be visible to the user.
            using (RichTextBox tempRtb = new RichTextBox())
            {
                // Critical: Apply the same properties as the display RichTextBox
                tempRtb.Size = this.PageDimensions;
                tempRtb.BorderStyle = BorderStyle.None; // Match display
                tempRtb.ScrollBars = RichTextBoxScrollBars.None; // Match display
                tempRtb.WordWrap = true; // Essential for pagination
                // Font setting here is tricky. The RTF content itself defines fonts.
                // Setting a default font on the RTB might affect measurement if RTF is missing font info,
                // but generally, the RTF's \deffN and \fN tags will override.
                // tempRtb.Font = new Font("Segoe UI", 14F); // Default from RTF header

                foreach (var chapter in Chapters)
                {
                    if (chapter == null || string.IsNullOrEmpty(chapter.FullRtfContent))
                    {
                        // Add a placeholder for empty chapters if necessary
                        if (chapter != null) chapter.PagesRtf.Add(FB2Parser.GetRtfHeader() + @"\par Empty Chapter" + FB2Parser.GetRtfFooter());
                        continue;
                    }
                    PaginateChapter(chapter, tempRtb);
                    TotalPagesInBook += chapter.PagesRtf.Count;
                }
            }
            if (TotalPagesInBook == 0 && Chapters.Any(c => c != null && !string.IsNullOrEmpty(c.FullRtfContent)))
            {
                Console.WriteLine("Pagination resulted in zero total pages, though chapters have content. Check pagination logic or content complexity.");
                // As a fallback, if pagination fails to produce pages but content exists,
                // add the full unpaginated RTF as a single page per chapter.
                // This is a failsafe and indicates an issue with PaginateChapter.
                TotalPagesInBook = 0; // Reset
                foreach (var chapter in Chapters.Where(c => c != null && !string.IsNullOrEmpty(c.FullRtfContent)))
                {
                    chapter.PagesRtf.Clear();
                    chapter.PagesRtf.Add(chapter.FullRtfContent);
                    TotalPagesInBook++;
                }
                if (TotalPagesInBook > 0) return true; // Fallback succeeded
            }
            return TotalPagesInBook > 0;
        }

        private void PaginateChapter(BookChapter chapter, RichTextBox tempRtb)
        {
            chapter.PagesRtf.Clear();
            if (string.IsNullOrEmpty(chapter.FullRtfContent))
            {
                chapter.PagesRtf.Add(FB2Parser.GetRtfHeader() + @"\par " + FB2Parser.GetRtfFooter()); // Empty page
                return;
            }

            tempRtb.Rtf = chapter.FullRtfContent;
            Application.DoEvents(); // Allow RTB to process RTF

            int currentPosition = 0;
            while (currentPosition < tempRtb.TextLength)
            {
                tempRtb.Select(currentPosition, tempRtb.TextLength - currentPosition);
                string remainingRtf = tempRtb.SelectedRtf; // Get RTF of the remaining text

                // Load remaining RTF into a working RTB to measure one page
                using (RichTextBox pageMeasureRtb = new RichTextBox())
                {
                    pageMeasureRtb.Size = tempRtb.Size;
                    pageMeasureRtb.BorderStyle = tempRtb.BorderStyle;
                    pageMeasureRtb.ScrollBars = tempRtb.ScrollBars;
                    pageMeasureRtb.WordWrap = tempRtb.WordWrap;
                    // pageMeasureRtb.Font = tempRtb.Font; // Match font if needed
                    pageMeasureRtb.Rtf = remainingRtf;
                    Application.DoEvents();

                    int charCountForPage = 0;
                    // Find how many characters fit on one page
                    // GetCharIndexFromPosition gives the char *before* which the point lies.
                    // So, a point at the bottom-right client edge gives an index.
                    // We want the character *at* that position or the one just before the text wraps/overflows.
                    Point bottomRight = new Point(pageMeasureRtb.ClientSize.Width - 1, pageMeasureRtb.ClientSize.Height - 1);
                    int lastVisibleCharIndexGlobal = pageMeasureRtb.GetCharIndexFromPosition(bottomRight);

                    if (lastVisibleCharIndexGlobal == 0 && pageMeasureRtb.TextLength > 1) // If RTB shows only 1st char, but more exists
                    {
                        // This edge case means the first character (or its formatting) is too large.
                        // We must take at least one character.
                        // Or the content is extremely minimal.
                        lastVisibleCharIndexGlobal = 0; // will take 1 char (index 0)
                    }


                    // If all remaining text fits on this page
                    if (lastVisibleCharIndexGlobal >= pageMeasureRtb.TextLength - 1)
                    {
                        charCountForPage = pageMeasureRtb.TextLength;
                    }
                    else
                    {
                        // Try to find a good break point (space, end of paragraph)
                        // Search backwards from lastVisibleCharIndexGlobal
                        int breakScanIndex = lastVisibleCharIndexGlobal;
                        bool breakFound = false;
                        for (int i = breakScanIndex; i >= 0; i--)
                        {
                            if (char.IsWhiteSpace(pageMeasureRtb.Text[i]))
                            {
                                // Break after this whitespace character
                                lastVisibleCharIndexGlobal = i;
                                breakFound = true;
                                break;
                            }
                        }
                        // If no whitespace found, we have to break mid-word or take what RTB gave.
                        // RichTextBox's GetCharIndexFromPosition handles character wrapping.
                        // So, lastVisibleCharIndexGlobal is the character just before it would overflow.
                        charCountForPage = lastVisibleCharIndexGlobal + 1;
                    }
                    if (charCountForPage <= 0 && pageMeasureRtb.TextLength > 0) // Ensure at least one char if content exists
                    {
                        charCountForPage = 1;
                    }


                    if (charCountForPage > 0)
                    {
                        pageMeasureRtb.Select(0, charCountForPage);
                        chapter.PagesRtf.Add(pageMeasureRtb.SelectedRtf);
                        currentPosition += charCountForPage;
                    }
                    else // No characters fit, or no text left (should be caught by while loop)
                    {
                        if (pageMeasureRtb.TextLength > 0 && chapter.PagesRtf.Count == 0)
                        {
                            // Failsafe: if stuck and no pages added, add remaining as one page
                            chapter.PagesRtf.Add(pageMeasureRtb.Rtf);
                        }
                        break; // Exit loop
                    }
                }
                Application.DoEvents();
            }

            if (chapter.PagesRtf.Count == 0 && !string.IsNullOrEmpty(chapter.FullRtfContent))
            {
                // If pagination somehow yields no pages but there was content, add the whole thing as one page.
                // This is a fallback.
                Console.WriteLine($"Warning: Pagination for chapter '{chapter.Title}' resulted in 0 pages. Adding full content as one page.");
                chapter.PagesRtf.Add(chapter.FullRtfContent);
            }
            else if (chapter.PagesRtf.Count == 0 && string.IsNullOrEmpty(chapter.FullRtfContent))
            {
                chapter.PagesRtf.Add(FB2Parser.GetRtfHeader() + @"\par " + FB2Parser.GetRtfFooter()); // Truly empty chapter
            }
        }


        public (int chapterIndex, int pageInChapterIndex) GetChapterAndPageIndices(int absolutePageNumber)
        {
            if (TotalPagesInBook == 0) return (0, 0);
            if (absolutePageNumber < 1) absolutePageNumber = 1;
            if (absolutePageNumber > TotalPagesInBook) absolutePageNumber = TotalPagesInBook;

            int pagesTraversed = 0;
            for (int i = 0; i < Chapters.Count; i++)
            {
                if (Chapters[i] == null || Chapters[i].PagesRtf == null || !Chapters[i].PagesRtf.Any())
                {
                    // If it's the only chapter and it's empty, target it.
                    if (Chapters.Count == 1) return (i, 0);
                    continue; // Skip empty chapters if there are others
                }

                if (absolutePageNumber <= pagesTraversed + Chapters[i].PagesRtf.Count)
                {
                    return (i, absolutePageNumber - pagesTraversed - 1);
                }
                pagesTraversed += Chapters[i].PagesRtf.Count;
            }

            // Fallback: if absolutePageNumber is out of bounds due to empty chapters or other issues
            if (Chapters.Any())
            {
                for (int i = Chapters.Count - 1; i >= 0; i--) // Find last valid chapter
                {
                    if (Chapters[i] != null && Chapters[i].PagesRtf != null && Chapters[i].PagesRtf.Any())
                    {
                        return (i, Chapters[i].PagesRtf.Count - 1); // Last page of last valid chapter
                    }
                }
                // If all chapters are empty
                return (0, 0);
            }
            return (0, 0); // Should not be reached if TotalPagesInBook > 0
        }

        public int GetAbsolutePageNumber(int chapterIndex, int pageInChapterIndex)
        {
            if (TotalPagesInBook == 0) return 1; // Or 0 if preferred for "no page"
            if (chapterIndex < 0 || chapterIndex >= Chapters.Count) return 1; // Invalid chapter index

            int absolutePage = 0;
            for (int i = 0; i < chapterIndex; i++)
            {
                if (Chapters[i] != null && Chapters[i].PagesRtf != null)
                {
                    absolutePage += Chapters[i].PagesRtf.Count;
                }
            }

            // Ensure pageInChapterIndex is valid for the given chapter
            if (Chapters[chapterIndex] != null && Chapters[chapterIndex].PagesRtf != null)
            {
                if (pageInChapterIndex < 0) pageInChapterIndex = 0;
                if (pageInChapterIndex >= Chapters[chapterIndex].PagesRtf.Count)
                {
                    pageInChapterIndex = Math.Max(0, Chapters[chapterIndex].PagesRtf.Count - 1);
                }
            }
            else
            {
                pageInChapterIndex = 0; // Chapter is null or has no pages
            }

            absolutePage += pageInChapterIndex + 1;
            return Math.Max(1, absolutePage); // Ensure it's at least 1
        }
    }
}