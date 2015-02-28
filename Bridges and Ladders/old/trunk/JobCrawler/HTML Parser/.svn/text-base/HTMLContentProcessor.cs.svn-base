using System;
using System.Collections.Generic;
using System.Text;
using JobCrawler.HTMLParser.HTMLContentFilters.Filters;
using JobCrawler.HTMLParser.HTMLElements;
using JobCrawler.HTMLParser.HTMLContentFilters.Interface;
using DatabaseHandler;

namespace JobCrawler.HTMLParser
{
    public class HTMLContentExtractor
    {
        private static readonly string[] CONTENT_SEPERATION_ELEMENTS = { "h1", "h2", "h3", "h4", "h5", "h6", "caption",
                                                                  "p", "td", "div" };
        private static readonly string HTML_LINE_BREAK_ELEMENT = "br";

        //the following characters indicate lack of "real" content
        private static readonly char[] HTML_TEXT_NO_CONTENT_IND = { '|' };

        private static List<IHTMLFilter> _filters;

        private static char[] _sentenceTerminators = { '.', '?', '!' };

        static HTMLContentExtractor()
        {
            _filters = new List<IHTMLFilter>();

            //add filters
            _filters.Add(new EmbedObjectFilter());
            _filters.Add(new FormattingFilter());
            _filters.Add(new ImageFilter());
            _filters.Add(new ScriptFilter());
            _filters.Add(new LinkFilter());
        }

        public static HtmlDocument extractWebDocument(string htmlArticle_, DatabaseHandler.DatabaseHandler dataHandler_)
        {
            //parse HTML to create a DOM tree
            HtmlDocument htmlDoc = HtmlDocument.Create(htmlArticle_, false);

            //retrieve nodes in the DOM Tree
            HtmlNodeCollection nodeCollection = htmlDoc.Nodes;

            //filter HTML
            foreach (IHTMLFilter htmlFilter in _filters)
            {
                htmlFilter.filterHTML(nodeCollection, dataHandler_);
            }

            return htmlDoc;
        }

        public static string extractArticleContent(string htmlArticle_, DatabaseHandler.DatabaseHandler dataHandler_)
        {
            string articleContent = "";

            //parse HTML to create a DOM tree
            HtmlDocument htmlDoc = HtmlDocument.Create(htmlArticle_, false);

            //retrieve nodes in the DOM Tree
            HtmlNodeCollection nodeCollection = htmlDoc.Nodes;

            //filter HTML
            foreach (IHTMLFilter htmlFilter in _filters)
            {
                htmlFilter.filterHTML(nodeCollection, dataHandler_);
            }

            //extract article text
            articleContent = extractArticleText(nodeCollection);

            return articleContent;
        }

        private static string extractArticleText(HtmlNodeCollection htmlTree_)
        {
            StringBuilder articleText = new StringBuilder();

            string remainingTextFromPriorNode = "";

            //retrieve all text nodes in the Html Tree
            List<HtmlText> HtmltextNodes = htmlTree_.findHtmlTextNodes();

            for (int i = 0; HtmltextNodes != null && i < HtmltextNodes.Count; ++i)
            {
                string text = remainingTextFromPriorNode + " " + HtmltextNodes[i].Text;

                //first check if text contains any character that allow us to infer that the text is not actual content
                if (textHasNoContentIndicator(text))
                {
                    //ignore this text!
                    continue;
                }

                //find all termination characters in the sentence in order to extract sentences
                List<int> terminationIndexes = getSentenceTerminationIndexes(text);

                int lastTerminationIndex = -1;

                foreach (int terminationIndex in terminationIndexes) //retrieve sentences based on termination indexes found
                {
                    articleText.Append(text.Substring(lastTerminationIndex + 1, terminationIndex - lastTerminationIndex).Trim() + " ");

                    lastTerminationIndex = terminationIndex;
                }

                if (lastTerminationIndex < (text.Length - 1)) //are there remaining characters in the text?
                {
                    remainingTextFromPriorNode = text.Substring(lastTerminationIndex + 1).Trim();

                    //is there text remaining outside the sentences we found (from above) and 
                    //is there a subsequent text node?
                    if (!remainingTextFromPriorNode.Equals(string.Empty) && i < (HtmltextNodes.Count - 1))
                    {
                        //there is remaining text

                        //determine if we should prepend remaining text to subsequent text node
                        if (!shouldPrependTextToSubsequentTextNode(HtmltextNodes[i], HtmltextNodes[i + 1]))
                        {
                            remainingTextFromPriorNode = "";
                        }
                    }
                    else
                    {
                        remainingTextFromPriorNode = "";
                    }
                }
            }

            return articleText.ToString().Trim();
        }

        private static bool textHasNoContentIndicator(string text_)
        {
            bool result = false;

            foreach (char indicator in HTML_TEXT_NO_CONTENT_IND)
            {
                if (text_.ToLower().IndexOf(indicator) >= 0)
                {
                    result = true;
                }
            }

            return result;
        }

        private static bool shouldPrependTextToSubsequentTextNode(HtmlText textNodeWithRemainingText_, HtmlText subsequentTextNode_)
        {
            bool result = true; //by default, say yes

            //check if there is a line break element between the text node with remaining text and the subsequent text node
            //since that indicates seperation of content
            if (isSeperatedByLineBreak(textNodeWithRemainingText_, subsequentTextNode_))
            {
                result = false;
            }

            //check if the text node containing the remaining text and subsequent text node have a common
            //ancestor that designates seperation between content on Html pages
            if (!haveCommonSeperationElement(textNodeWithRemainingText_, subsequentTextNode_))
            {
                result = false;
            }

            return result;
        }

        private static bool isSeperatedByLineBreak(HtmlText firstTextNode_, HtmlText secondTextNode_)
        {
            bool result = false;

            HtmlNode currentNode = firstTextNode_;

            while (currentNode != null && currentNode != (HtmlNode)secondTextNode_ && !(currentNode is HtmlElement && ((HtmlElement)currentNode).Name.ToLower().Equals(HTML_LINE_BREAK_ELEMENT.ToLower())))
            {
                //move to next node in preorder traversal
                currentNode = getPreOrderNextHtmlNode(currentNode);
            }

            if (currentNode != null && currentNode != (HtmlNode)secondTextNode_)
            {
                //we found a line break
                result = true;
            }

            return result;
        }

        private static HtmlNode getPreOrderNextHtmlNode(HtmlNode startPositionNode_)
        {
            if (startPositionNode_ is HtmlElement) //is this an Html Element
            {
                HtmlElement startPosElement = (HtmlElement) startPositionNode_;
                if (startPosElement.IsParent) //does this node have children?
                {
                    return startPosElement.Nodes[0]; //return the first left child
                }
            }

            //go up the heirarchy until we find an ancestor that has a child on the right side
            while (startPositionNode_.Parent != null && startPositionNode_.Index == (startPositionNode_.Parent.Nodes.Count - 1))
            {
                //this was the last child node of the parent!
                
                //go up one heirarchy
                startPositionNode_ = startPositionNode_.Parent;
            }

            if (startPositionNode_.Parent != null)
            {
                //return the right sibling of this node
                return startPositionNode_.Parent.Nodes[startPositionNode_.Index + 1];
            }

            //didn't find any node that follows and so return null
            return null;
        }

        private static bool haveCommonSeperationElement(HtmlText firstTextNode_, HtmlText secondTextNode_)
        {
            bool result = false;

            //retrieve the first seperation element (from the bottom of the tree) ancestors in the heirarchy of each of the text nodes
            HtmlElement nearestFirstTextNodeSeperationAncestor = getNearestSeperationAncestor(firstTextNode_);
            HtmlElement nearestSecondTextNodeSeperationAncestor = getNearestSeperationAncestor(secondTextNode_);

            //do we have a match between the seperation ancestors?
            if (nearestFirstTextNodeSeperationAncestor == null && nearestSecondTextNodeSeperationAncestor == null)
            {
                result = true;
            }
            else if (nearestFirstTextNodeSeperationAncestor != null && nearestSecondTextNodeSeperationAncestor != null &&
                nearestFirstTextNodeSeperationAncestor == nearestSecondTextNodeSeperationAncestor)
            {
                result = true;
            }

            return result;
        }

        private static HtmlElement getNearestSeperationAncestor(HtmlText sourceToLookIn_)
        {
            HtmlElement ancestor = sourceToLookIn_.Parent;

            //keep going up heirarchy until we find a match
            while (ancestor != null)
            {
                //check if this ancestor matches any of the seperation element html tags
                foreach (string seperationElement in CONTENT_SEPERATION_ELEMENTS)
                {
                    if (ancestor.Name.ToLower().Equals(seperationElement.ToLower()))
                    {
                        //we found a ancestor that is a seperation element
                        return ancestor;
                    }
                }

                ancestor = ancestor.Parent;
            }

            //if we reach here, we didn't find any match
            return ancestor;
        }

        private static List<int> getSentenceTerminationIndexes(string text_)
        {
            List<int> terminatorIndexes = new List<int>();

            int searchIndex = 0; //represents the index from where to search from in the text string

            while (searchIndex < text_.Length)
            {
                //does this sentence contain any of the sentence terminators?
                int index = text_.IndexOfAny(_sentenceTerminators, searchIndex);

                //did we find the terminator?
                if (index != -1)
                {
                    /* we found a terminator */

                    //first, ensure that the text has atleast 2 words since a sentence cannot be formed with just one word
                    if (text_.Substring(searchIndex, index - searchIndex).Trim().IndexOf(' ') > 0) //accomplished via checking for atleast one occurence of SPACE character
                    {
                        //now check if this is actually a terminator or occurs
                        //by coincidence (eg. period in the specification of money : $9.99)
                        if ((index + 1) < text_.Length)
                        {
                            if (text_[index + 1] == ' ')
                            {
                                //this terminator is okay and thus designates the 
                                //end of a sentence
                                terminatorIndexes.Add(index);
                            }
                        }
                        else
                        {
                            //add the termination index to the list since we're at the end of the sentence and thus,
                            //the terminator is okay
                            terminatorIndexes.Add(index);
                        }
                    }

                    //increment the search index for next iteration
                    searchIndex = index + 1;

                    //reset index variable for next iteration
                    index = -1;
                }
                else
                {
                    break;
                }
            }

            return terminatorIndexes;
        }
    }
}
