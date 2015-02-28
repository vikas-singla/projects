using System;
using System.Collections.Generic;
using System.Text;
using JobCrawler.HTMLParser.HTMLContentFilters.Interface;
using JobCrawler.HTMLParser.HTMLElements;
using DatabaseHandler;

namespace JobCrawler.HTMLParser.HTMLContentFilters.Filters
{
    internal abstract class AbstractFilter : IHTMLFilter
    {
        public abstract void filterHTML(HtmlNodeCollection htmlTree_, DatabaseHandler.DatabaseHandler dataHandler_);

        protected void filterTag(HtmlNodeCollection htmlTree_, string tagName, bool keepChildren_)
        {
            List<HtmlElement> nodesFound = htmlTree_.FindByName(tagName, true);

            foreach (HtmlElement elementNode in nodesFound)
            {
                if (keepChildren_)
                {
                    int elementIndex = -1;

                    HtmlNodeCollection childrenNodes = elementNode.Nodes;

                    HtmlNodeCollection parentNodeCollection = null;

                    if (elementNode.Parent != null)
                    {
                        parentNodeCollection = elementNode.Parent.Nodes;

                        elementIndex = elementNode.Index;
                    }
                    else
                    {
                        parentNodeCollection = htmlTree_;

                        elementIndex = htmlTree_.IndexOf(elementNode);
                    }

                    //remove the element from the parent first
                    parentNodeCollection.RemoveAt(elementIndex);

                    //add all children of the element to the parent element's children collection
                    foreach (HtmlNode node in childrenNodes)
                    {
                        parentNodeCollection.Insert(elementIndex, node);

                        //increment index for next child node
                        ++elementIndex;
                    }
                }
                else
                {
                    if (elementNode.Parent != null)
                    {
                        //remove this node and all its child nodes from the HTML Tree
                        elementNode.Remove();
                    }
                    else
                    {
                        //remove from top level collection
                        htmlTree_.RemoveAt(htmlTree_.IndexOf(elementNode));
                    }
                }
            }
        }

        protected void filterElement(HtmlElement element_, HtmlNodeCollection htmlTree_, bool keepChildren_)
        {
            if (keepChildren_)
            {
                int elementIndex = -1;

                HtmlNodeCollection childrenNodes = element_.Nodes;

                HtmlNodeCollection parentNodeCollection = null;

                if (element_.Parent != null)
                {
                    parentNodeCollection = element_.Parent.Nodes;

                    elementIndex = element_.Index;
                }
                else
                {
                    parentNodeCollection = htmlTree_;

                    elementIndex = htmlTree_.IndexOf(element_);
                }

                //remove the element from the parent first
                parentNodeCollection.RemoveAt(elementIndex);

                //add all children of the element to the parent element's children collection
                foreach (HtmlNode node in childrenNodes)
                {
                    parentNodeCollection.Insert(elementIndex, node);
                    
                    //increment index for next child node
                    ++elementIndex;
                }
            }
            else
            {
                if (element_.Parent != null)
                {
                    //remove this node and all its child nodes from the HTML Tree
                    element_.Remove();
                }
                else
                {
                    //remove from top level collection
                    htmlTree_.RemoveAt(htmlTree_.IndexOf(element_));
                }
            }
        }
    }
}
