using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;

namespace JobCrawler.HTMLParser.HTMLElements
{
    /// <summary>
    /// The HtmlNode is the base for all objects that may appear in HTML. Currently, 
    /// this implemention only supports HtmlText and HtmlElement node types.
    /// </summary>
    public abstract class HtmlNode
    {
        protected HtmlElement mParent;

        /// <summary>
        /// This constructor is used by the subclasses.
        /// </summary>
        protected HtmlNode( )
        {
            mParent = null;
        }

        /// <summary>
        /// This will render the node as it would appear in HTML.
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString( );

        /// <summary>
        /// This will return the parent of this node, or null if there is none.
        /// </summary>
        public HtmlElement Parent
        {
            get
            {
                return mParent;
            }
        }

        /// <summary>
        /// This will return the next sibling node. If this is the last one, it will return null.
        /// </summary>
        public HtmlNode Next
        {
            get
            {
                if ( Index == -1 )
                {
                    return null;
                }
                else
                {
                    if ( Parent.Nodes.Count > Index + 1 )
                    {
                        return Parent.Nodes[ Index + 1 ];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// This will return the previous sibling node. If this is the first one, it will return null.
        /// </summary>
        public HtmlNode Previous
        {
            get
            {
                if ( Index == -1 )
                {
                    return null;
                }
                else
                {
                    if ( Index > 0 )
                    {
                        return Parent.Nodes[ Index - 1 ];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// This will return the first child node. If there are no children, this
        /// will return null.
        /// </summary>
        public HtmlNode FirstChild
        {
            get
            {
                if ( this is HtmlElement )
                {
                    if ( ( ( HtmlElement ) this ).Nodes.Count == 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return ( ( HtmlElement ) this ).Nodes[ 0 ];
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// This will return the last child node. If there are no children, this
        /// will return null.
        /// </summary>
        public HtmlNode LastChild
        {
            get
            {
                if ( this is HtmlElement )
                {
                    if ( ( ( HtmlElement ) this ).Nodes.Count == 0 )
                    {
                        return null;
                    }
                    else
                    {
                        return ( ( HtmlElement ) this ).Nodes[ ( ( HtmlElement ) this ).Nodes.Count - 1 ];
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// This will return the index position within the parent's nodes that this one resides.
        /// If this is not in a collection, this will return -1.
        /// </summary>
        public int Index
        {
            get
            {
                if ( mParent == null )
                {
                    return -1;
                }
                else
                {
                    return mParent.Nodes.IndexOf( this );
                }
            }
        }

        /// <summary>
        /// This will return true if this is a root node (has no parent).
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return mParent == null;
            }
        }

        /// <summary>
        /// This will return true if this is a child node (has a parent).
        /// </summary>
        public bool IsChild
        {
            get
            {
                return mParent != null;
            }
        }

        public bool IsParent
        {
            get
            {
                if ( this is HtmlElement )
                {
                    return ( ( HtmlElement ) this ).Nodes.Count > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// This will return true if the node passed is a descendent of this node.
        /// </summary>
        /// <param name="node">The node that might be the parent or grandparent (etc.)</param>
        /// <returns>True if this node is a descendent of the one passed in.</returns>
        public bool IsDescendentOf( HtmlNode node )
        {
            HtmlNode parent = mParent;
            while ( parent != null )
            {
                if ( parent == node )
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        /// <summary>
        /// This will return true if the node passed is one of the children or grandchildren of this node.
        /// </summary>
        /// <param name="node">The node that might be a child.</param>
        /// <returns>True if this node is an ancestor of the one specified.</returns>
        public bool IsAncestorOf( HtmlNode node )
        {
            return node.IsDescendentOf( this );
        }

        /// <summary>
        /// This will return the ancstor that is common to this node and the one specified.
        /// </summary>
        /// <param name="node">The possible node that is relative</param>
        /// <returns>The common ancestor, or null if there is none</returns>
        public HtmlNode GetCommonAncestor( HtmlNode node )
        {
            HtmlNode thisParent = this;
            while ( thisParent != null )
            {
                HtmlNode thatParent = node;
                while ( thatParent != null )
                {
                    if ( thisParent == thatParent )
                    {
                        return thisParent;
                    }
                    thatParent = thatParent.Parent;
                }
                thisParent = thisParent.Parent;
            }
            return null;
        }

        /// <summary>
        /// This will remove this node and all child nodes from the tree. If this
        /// is a root node, this operation will do nothing.
        /// </summary>
        public void Remove( )
        {
            if ( mParent != null )
            {
                mParent.Nodes.RemoveAt( this.Index );
            }
        }

        /// <summary>
        /// Internal method to maintain the identity of the parent node.
        /// </summary>
        /// <param name="parentNode">The parent node of this one</param>
        internal void SetParent( HtmlElement parentNode )
        {
            mParent = parentNode;
        }

        /// <summary>
        /// This will return the full HTML to represent this node (and all child nodes).
        /// </summary>
        public abstract string HTML { get; }

        /// <summary>
        /// This will return the full XHTML to represent this node (and all child nodes)
        /// </summary>
        public abstract string XHTML { get; }

        public bool IsText( )
        {
            return this is HtmlText;
        }
        
        public bool IsElement( )
        {
            return this is HtmlElement;
        }
    }

    /// <summary>
    /// This object represents a collection of HtmlNodes, which can be either HtmlText
    /// or HtmlElement objects. The order in which the nodes occur directly corresponds
    /// to the order in which they appear in the original HTML document.
    /// </summary>
    public class HtmlNodeCollection : CollectionBase
    {
        private HtmlElement mParent;

        // Public constructor to create an empty collection.
        public HtmlNodeCollection( )
        {
            mParent = null;
        }

        /// <summary>
        /// A collection is usually associated with a parent node (an HtmlElement, actually)
        /// but you can pass null to implement an abstracted collection.
        /// </summary>
        /// <param name="parent">The parent element, or null if it is not appropriate</param>
        internal HtmlNodeCollection( HtmlElement parent )
        {
            mParent = parent;
        }

        /// <summary>
        /// This will add a node to the collection.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int Add( HtmlNode node )
        {
            if ( mParent != null ) node.SetParent( mParent );
            return base.List.Add( node );
        }

        /// <summary>
        /// This is used to identify the index of this node as it appears in the collection.
        /// </summary>
        /// <param name="node">The node to test</param>
        /// <returns>The index of the node, or -1 if it is not in this collection</returns>
        public int IndexOf( HtmlNode node )
        {
            return base.List.IndexOf( node );
        }

        /// <summary>
        /// This will insert a node at the given position
        /// </summary>
        /// <param name="index">The position at which to insert the node.</param>
        /// <param name="node">The node to insert.</param>
        public void Insert( int index, HtmlNode node )
        {
            if ( mParent != null ) node.SetParent( mParent );
            base.InnerList.Insert( index, node );
        }

        /// <summary>
        /// This property allows you to change the node at a particular position in the
        /// collection.
        /// </summary>
        public HtmlNode this[ int index ]
        {
            get
            {
                return ( HtmlNode ) base.InnerList[ index ];
            }
            set
            {
                if ( mParent != null ) value.SetParent( mParent );
                base.InnerList[ index ] = value;
            }
        }

        /// <summary>
        /// This allows you to directly access the first element in this colleciton with the given name.
        /// If the node does not exist, this will return null.
        /// </summary>
        public HtmlNode this[ string name ]
        {
            get
            {
                List<HtmlElement> results = FindByName( name, false );
                if ( results.Count > 0 )
                {
                    return results[ 0 ];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// This will search though this collection of nodes for all elements with the
        /// specified name. If you want to search the subnodes recursively, you should
        /// pass True as the parameter in searchChildren. This search is guaranteed to
        /// return nodes in the order in which they are found in the document.
        /// </summary>
        /// <param name="name">The name of the element to find</param>
        /// <returns>A list of all the nodes that macth.</returns>
        public List<HtmlElement> FindByName( string name )
        {
            return FindByName( name, true );
        }

        /// <summary>
        /// This will search though this collection of nodes for all elements with the
        /// specified name. If you want to search the subnodes recursively, you should
        /// pass True as the parameter in searchChildren. This search is guaranteed to
        /// return nodes in the order in which they are found in the document.
        /// </summary>
        /// <param name="name">The name of the element to find</param>
        /// <param name="searchChildren">True if you want to search sub-nodes, False to
        /// only search this collection.</param>
        /// <returns>A list of all the nodes that macth.</returns>
        public List<HtmlElement> FindByName( string name, bool searchChildren )
        {
            List<HtmlElement> results = new List<HtmlElement>( );

            //convert to lower case
            name = name.ToLower();

            foreach ( HtmlNode node in base.List )
            {
                if ( node is HtmlElement )
                {
                    HtmlElement elementNode = (HtmlElement)node;

                    if ( ( elementNode ).Name.Equals( name ) )
                    {
                        results.Add( elementNode );
                    }
                    if ( searchChildren )
                    {
                        foreach ( HtmlElement matchedChild in elementNode.Nodes.FindByName( name, searchChildren ) )
                        {
                            results.Add( matchedChild );
                        }
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// This will search though this collection of nodes for all text nodes.
        /// This search is guaranteed to return nodes in the order in which they 
        /// are found in the document.
        /// </summary>
        /// <returns>List of text nodes found in the collection</returns>
        public List<HtmlText> findHtmlTextNodes()
        {
            List<HtmlText> results = new List<HtmlText>();

            foreach (HtmlNode node in base.List)
            {
                if (node is HtmlText)
                {
                    results.Add((HtmlText) node);
                }
                else if(node is HtmlElement)
                {
                    HtmlElement elementNode = (HtmlElement)node;

                    foreach (HtmlText matchedChild in elementNode.Nodes.findHtmlTextNodes())
                    {
                        results.Add(matchedChild);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// This will search though this collection of nodes for all elements with the an
        /// attribute with the given name. 
        /// </summary>
        /// <param name="name">The name of the attribute to find</param>
        /// <returns>A list of all the elements that macth.</returns>
        public List<HtmlElement> FindByAttributeName( string attributeName )
        {
            return FindByAttributeName( attributeName, true );
        }

        /// <summary>
        /// This will search though this collection of nodes for all elements with the an
        /// attribute with the given name. 
        /// </summary>
        /// <param name="name">The name of the attribute to find</param>
        /// <param name="searchChildren">True if you want to search sub-nodes, False to
        /// only search this collection.</param>
        /// <returns>A list of all the elements that match.</returns>
        public List<HtmlElement> FindByAttributeName( string attributeName, bool searchChildren )
        {
            List<HtmlElement> results = new List<HtmlElement>( );

            attributeName = attributeName.ToLower();

            foreach ( HtmlNode node in base.List )
            {
                if ( node is HtmlElement )
                {
                    HtmlElement elementNode = (HtmlElement)node;

                    foreach ( HtmlAttribute attribute in ( elementNode ).Attributes )
                    {
                        if ( attribute.Name.Equals( attributeName ) )
                        {
                            results.Add(elementNode);
                            break;
                        }
                    }
                    if ( searchChildren )
                    {
                        foreach ( HtmlElement matchedChild in ( elementNode ).Nodes.FindByAttributeName( attributeName, searchChildren ) )
                        {
                            results.Add( matchedChild );
                        }
                    }
                }
            }
            return results;
        }
    }
}
